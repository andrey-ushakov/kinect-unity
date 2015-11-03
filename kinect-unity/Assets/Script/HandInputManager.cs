using System;
using UnityEngine;
using System.Collections;


public enum HandMotion { 
    NON,
    RIGHT_HAND_WAVE_OUT,
    LEFT_HAND_WAVE_OUT,
    RIGHT_HAND_RISE,
    LEFT_HAND_RISE,
    TWO_HAND_RISE
}


public class HandMotionDetectedEventArgs: EventArgs{
    public HandMotion motion = HandMotion.NON;
}

public delegate void HandMotionDetectedEventHandler(object sender, HandMotionDetectedEventArgs arg);

public class HandInputManager : MonoBehaviour {

    public event HandMotionDetectedEventHandler handMotionDetected;

    private SkeletonWrapper sw;
    private int PlayerId = 0;

    public float detectPositionDuration = 1f;

    private Vector3 rightHandPos; // right hand position
    private Vector3 leftHandPos; // left hand position
    private Vector3 rightHandPosPre = Vector3.zero; // previous right hand position
    private Vector3 leftHandPosPre = Vector3.zero; // previous left hand position

    public float detectMotionDuration = 0.5f; // the duration of time of hand motion
    public float detectMotionDistance = 0.3f; // the distance of hand motion for dectction. use this with detectMotionDuration to restrict the average speed 
    private Vector3 rightHandVelo; // velocity of the right hand (no real velocity, because only the signs are used to identify the motion direction)
    private Vector3 leftHandVelo; // velocity of the left hand
    private Vector3 motionStartRightHandPos;
    private Vector3 motionStartLeftHandPos;
    private float motionRightHandStartTime = 0; // record the hand motion start time
    private float motionLeftHandStartTime = 0;
    private HandMotion currentRightHandMotion = HandMotion.NON;
    private HandMotion currentLeftHandMotion = HandMotion.NON;

    public float accuracy = 0.05f;

    private static HandInputManager _instance;

    public static HandInputManager Instance 
    {
        get {
            _instance = FindObjectOfType(typeof(HandInputManager)) as HandInputManager;
            if (_instance == null) {
                _instance = GameObject.Instantiate(GlobalVariables.GO_HAND_INPUT_MANAGER) as HandInputManager;
            }
            return _instance;
        }
    }

	// Use this for initialization
    void Awake() {
        sw = SkeletonWrapper.Instance;
    }

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (sw.pollSkeleton()) {
            float currentTime = Time.time;
            this.rightHandPos = sw.bonePos[PlayerId, 11];
            this.leftHandPos = sw.bonePos[PlayerId, 7];
            
            // get right hand velocity
            this.rightHandVelo = rightHandPos - rightHandPosPre;
            // get left hand velocity
            this.leftHandVelo = leftHandPos - leftHandPosPre;
            // right hand motion detection
            if (this.rightHandVelo.x > accuracy && Math.Abs(this.rightHandVelo.x) > Math.Abs(this.rightHandVelo.y))
            {
                if (currentRightHandMotion != HandMotion.RIGHT_HAND_WAVE_OUT)
                {
                    currentRightHandMotion = HandMotion.RIGHT_HAND_WAVE_OUT;
                    motionRightHandStartTime = currentTime;
                    motionStartRightHandPos = rightHandPos;
                }
            }
            else if (this.rightHandVelo.y > accuracy && Math.Abs(this.rightHandVelo.x) < Math.Abs(this.rightHandVelo.y))
            {
                if (currentRightHandMotion != HandMotion.RIGHT_HAND_RISE)
                {
                    currentRightHandMotion = HandMotion.RIGHT_HAND_RISE;
                    motionRightHandStartTime = currentTime;
                    motionStartRightHandPos = rightHandPos;
                }
            }
            else
            {
                currentRightHandMotion = HandMotion.NON;
            }

            // left hand motion detection
            if (this.leftHandVelo.x < -accuracy && Math.Abs(this.leftHandVelo.x) > Math.Abs(this.leftHandVelo.y))
            {
                if (currentLeftHandMotion != HandMotion.LEFT_HAND_WAVE_OUT)
                {
                    currentLeftHandMotion = HandMotion.LEFT_HAND_WAVE_OUT;
                    motionLeftHandStartTime = currentTime;
                    motionStartLeftHandPos = leftHandPos;
                }
            }
            else if (this.leftHandVelo.y > accuracy && Math.Abs(this.leftHandVelo.x) < Math.Abs(this.leftHandVelo.y))
            {
                if (currentLeftHandMotion != HandMotion.LEFT_HAND_RISE)
                {
                    currentLeftHandMotion = HandMotion.LEFT_HAND_RISE;
                    motionLeftHandStartTime = currentTime;
                    motionStartLeftHandPos = leftHandPos;
                }
            }
            else
            {
                currentLeftHandMotion = HandMotion.NON;
            }

            // validation of hand motion
            bool rightHandRiseValide = false;
            bool leftHandRiseValide = false;
            // right
            if (currentRightHandMotion == HandMotion.RIGHT_HAND_WAVE_OUT
                && currentTime - motionRightHandStartTime <= detectMotionDuration
                && rightHandPos.x - motionStartRightHandPos.x >= detectMotionDistance)
            {
                // right hand wave out detected
                // rise motion detected event
                OnHandMotionDetected(HandMotion.RIGHT_HAND_WAVE_OUT);
                // reset timer for new detection
                motionRightHandStartTime = currentTime;
                motionStartRightHandPos = rightHandPos;
            }
            else if (currentRightHandMotion == HandMotion.RIGHT_HAND_RISE
                    && currentTime - motionRightHandStartTime <= detectMotionDuration
                    && rightHandPos.y - motionStartRightHandPos.y >= detectMotionDistance)
            {
                rightHandRiseValide = true;
            }
            // left
            if (currentLeftHandMotion == HandMotion.LEFT_HAND_WAVE_OUT
                && currentTime - motionLeftHandStartTime <= detectMotionDuration
                && motionStartLeftHandPos.x - leftHandPos.x >= detectMotionDistance)
            {
                // left hand wave out detected
                // rise motion detected event
                OnHandMotionDetected(HandMotion.LEFT_HAND_WAVE_OUT);
                // reset timer for new detection
                motionLeftHandStartTime = currentTime;
                motionStartLeftHandPos = leftHandPos;
            }
            else if (currentLeftHandMotion == HandMotion.LEFT_HAND_RISE
                    && currentTime - motionLeftHandStartTime <= detectMotionDuration
                    && leftHandPos.y - motionStartLeftHandPos.y >= detectMotionDistance)
            {
                leftHandRiseValide = true;
            }
            // double hand
            if (rightHandRiseValide && leftHandRiseValide)
            {
                // double hands rise detected
                // rise motion detected event
                OnHandMotionDetected(HandMotion.TWO_HAND_RISE);
                motionRightHandStartTime = currentTime;
                motionLeftHandStartTime = currentTime;
                motionStartRightHandPos = rightHandPos;
                motionStartLeftHandPos = leftHandPos;
            }
            
            rightHandPosPre = rightHandPos;
            leftHandPosPre = leftHandPos;

        }
	}

    protected virtual void OnHandMotionDetected(HandMotion motion)
    {
        if (handMotionDetected != null)
        {
            HandMotionDetectedEventArgs e = new HandMotionDetectedEventArgs();
            e.motion = motion;
            Debug.Log(e.motion);
            handMotionDetected(this, e);
        }
    }
}
