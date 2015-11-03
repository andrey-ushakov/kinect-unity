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


public class HandMotionDetectedEventArg: EventArgs{
    public HandMotion motion = HandMotion.NON;
}

public delegate void HandMotionDetectedEventHandler(object sender, HandMotionDetectedEventArg arg);

public class HandInputManager : MonoBehaviour {

    public event HandMotionDetectedEventHandler handMostionDetected;

    private SkeletonWrapper sw;


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
            
        }
	}
}
