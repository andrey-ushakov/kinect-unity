using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour
{

    public static SkeletonWrapper GO_KINECT_PREFAB;
    public SkeletonWrapper kinectPrefab;

    public static HandInputManager GO_HAND_INPUT_MANAGER;
    public HandInputManager handInputManager;

    void Awake()
    {
        GO_KINECT_PREFAB = kinectPrefab;
        GO_HAND_INPUT_MANAGER = handInputManager;
    }
    // Use this for initialization
    void Start()
    {
        Debug.Log("GLOBLE");
    }
}

