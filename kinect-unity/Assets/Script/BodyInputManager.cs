using System;
using UnityEngine;
using System.Collections;

public enum BodyMotion { 
	NON,
	BODY_UP,
	RIGHT_FOOT_UP,
	LEFT_FOOT_UP,
	JUMP,
	SUPER_JUMP
}

public class BodyMotionDetectedEventArgs: EventArgs{
	public BodyMotion motion = BodyMotion.NON;
}

public delegate void BodyMotionDetectedEventHandler(object sender, BodyMotionDetectedEventArgs arg);


public class BodyInputManager : MonoBehaviour {

	public static event BodyMotionDetectedEventHandler bodyMotionDetected;
	
	private SkeletonWrapper sw;
	private int PlayerId = 0;
	private bool firstTime = true;
	
	public float detectPositionDuration = 1f;

	private Vector3 rightFootPos;
	private Vector3 leftFootPos;
	private Vector3 spinePos;
	private Vector3 rightFootPosPre = Vector3.zero;
	private Vector3 leftFootPosPre = Vector3.zero;
	private Vector3 spinePosPre = Vector3.zero;

	public float detectMotionDuration = 0.4f; 
	public float detectMotionDistance = 0.2f;

	private Vector3 rightFootVelo;
	private Vector3 leftFootVelo;
	private Vector3 spineVelo;

	private Vector3 motionStartRightFootPos;
	private Vector3 motionStartLeftFootPos;
	private Vector3 motionStartSpineSPos;
	private float motionRightFootStartTime = 0;
	private float motionLeftFootStartTime = 0;
	private float motionSpineStartTime = 0;

	private BodyMotion currentRightFootMotion = BodyMotion.NON;
	private BodyMotion currentLeftFootMotion = BodyMotion.NON;
	private BodyMotion currentSpineMotion = BodyMotion.NON;


	public float accuracy = 0.05f;
	
	private static BodyInputManager _instance;
	
	public static BodyInputManager Instance 
	{
		get {
			_instance = FindObjectOfType(typeof(BodyInputManager)) as BodyInputManager;
			if (_instance == null) {
				_instance = GameObject.Instantiate(GlobalVariables.GO_BODY_INPUT_MANAGER) as BodyInputManager;
			}
			return _instance;
		}
	}
	

	// Use this for initialization
	void Start () {
		sw = SkeletonWrapper.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		if (sw.pollSkeleton ()) {
			float currentTime = Time.time;
			this.rightFootPos = sw.bonePos[PlayerId, 19];
			this.leftFootPos = sw.bonePos[PlayerId, 15];
			this.spinePos = sw.bonePos[PlayerId, 1];

			if (this.firstTime){
				this.rightFootPosPre = this.rightFootPos;
				this.leftFootPosPre = this.leftFootPos;
				this.spinePosPre = this.spinePos;
				this.firstTime = false;
			}

			this.rightFootVelo = this.rightFootPos - this.rightFootPosPre;
			this.leftFootVelo = this.leftFootPos - this.leftFootPosPre;
			this.spineVelo = this.spinePos - this.spinePosPre;

			if (this.rightFootVelo.y > accuracy && Math.Abs(this.rightFootVelo.y) > Math.Abs(this.rightFootVelo.x))
			{
				if (currentRightFootMotion != BodyMotion.RIGHT_FOOT_UP)
				{
					currentRightFootMotion = BodyMotion.RIGHT_FOOT_UP;
					motionRightFootStartTime = currentTime;
					motionStartRightFootPos = leftFootPos;
				}
			}
			else {
				currentRightFootMotion = BodyMotion.NON;
			}

			if (this.leftFootVelo.y > accuracy && Math.Abs(this.leftFootVelo.y) > Math.Abs(this.leftFootVelo.x))
			{
				if (currentLeftFootMotion != BodyMotion.LEFT_FOOT_UP)
				{
					currentLeftFootMotion = BodyMotion.LEFT_FOOT_UP;
					motionLeftFootStartTime = currentTime;
					motionStartLeftFootPos = leftFootPos;
				}
			}
			else {
				currentLeftFootMotion = BodyMotion.NON;
			}

			if (this.spineVelo.y > accuracy && Math.Abs(this.spineVelo.y) > Math.Abs(this.spineVelo.x))
			{
				if (currentLeftFootMotion != BodyMotion.LEFT_FOOT_UP)
				{
					currentSpineMotion = BodyMotion.BODY_UP;
					motionSpineStartTime = currentTime;
					motionStartLeftFootPos = spinePos;
				}
			}
			else {
				currentSpineMotion = BodyMotion.NON;
			}
//			currentRightHandMotion == HandMotion.RIGHT_HAND_WAVE_OUT
//				&& currentTime - motionRightHandStartTime <= detectMotionDuration
//					&& rightHandPos.x - motionStartRightHandPos.x >= detectMotionDistance

			if (currentLeftFootMotion == BodyMotion.LEFT_FOOT_UP
			    && currentRightFootMotion == BodyMotion.RIGHT_FOOT_UP
			    && currentSpineMotion == BodyMotion.BODY_UP
			    && currentTime - motionRightFootStartTime <= detectMotionDuration
			    && currentTime - motionLeftFootStartTime <= detectMotionDuration
			    && currentTime - motionSpineStartTime <= detectMotionDuration
			    && rightFootPos.y - motionStartRightFootPos.y >= detectMotionDistance
			    && leftFootPos.y - motionStartLeftFootPos.y >= detectMotionDistance
			    && spinePos.y - motionStartSpineSPos.y >= detectMotionDistance)
			{
				if (rightFootVelo.y - spineVelo.y >= detectMotionDistance
				    && leftFootVelo.y - spineVelo.y >= detectMotionDistance){
					OnBodyMotionDetected(BodyMotion.SUPER_JUMP);
				}
				else
				{
					OnBodyMotionDetected(BodyMotion.JUMP);
				}

				motionLeftFootStartTime = currentTime;
				motionRightFootStartTime = currentTime;
				motionSpineStartTime = currentTime;
				motionStartLeftFootPos = leftFootPos;
				motionStartRightFootPos = rightFootPos;
				motionStartSpineSPos = spinePos;
			}

			rightFootPosPre = rightFootPos;
			leftFootPosPre = leftFootPos;
			spinePosPre = spinePos;
		}
	}

	protected virtual void OnBodyMotionDetected(BodyMotion motion)
	{
		if (bodyMotionDetected != null)
		{
			BodyMotionDetectedEventArgs e = new BodyMotionDetectedEventArgs();
			e.motion = motion;
			bodyMotionDetected(this, e);
		}
	}
}
