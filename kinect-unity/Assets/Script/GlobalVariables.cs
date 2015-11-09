using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {
	public static GameMode gameMode;

	public static SkeletonWrapper GO_KINECT_PREFAB;
    public SkeletonWrapper kinectPrefab;

    public static HandInputManager GO_HAND_INPUT_MANAGER;
    public HandInputManager handInputManager;

	public static BodyInputManager GO_BODY_INPUT_MANAGER;
	public BodyInputManager bodyInputManager;

    void Awake() {
        GO_KINECT_PREFAB = kinectPrefab;
        GO_HAND_INPUT_MANAGER = handInputManager;
		GO_BODY_INPUT_MANAGER = bodyInputManager;
    }
    // Use this for initialization
    void Start()
    {
    }
}

