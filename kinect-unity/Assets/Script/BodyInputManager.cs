using System;
using UnityEngine;
using System.Collections;

public enum BodyMotion { 
	NON,
	JUMP,
	SUPER_JUMP
}

public class BodyMotionDetectedEventArgs: EventArgs{
	public BodyMotion motion = BodyMotion.NON;
}

public delegate void BodyMotionDetectedEventHandler(object sender, BodyMotionDetectedEventArgs arg);


public class BodyInputManager : MonoBehaviour {

	public static event BodyMotionDetectedEventHandler handMotionDetected;
	
	private SkeletonWrapper sw;
	private int PlayerId = 0;
	
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
		
		}
	}
}
