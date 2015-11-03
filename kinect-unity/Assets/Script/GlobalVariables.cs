using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour
{

    public static SkeletonWrapper GO_KINECT_PREFAB;
    public SkeletonWrapper go_KinectPrefab;


    void Awake()
    {
        GO_KINECT_PREFAB = go_KinectPrefab;

    }
    // Use this for initialization
    void Start()
    {
    }
}

