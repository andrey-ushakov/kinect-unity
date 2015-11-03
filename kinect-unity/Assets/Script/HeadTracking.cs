using UnityEngine;
using System;
using System.Collections;

public class HeadTracking : MonoBehaviour {
	
	private SkeletonWrapper sw;
    private HandInputManager him;
    private int PlayerId = 0;

    private Vector3 headPos;

    void Awake() {
        sw = SkeletonWrapper.Instance;
        him = HandInputManager.Instance;
    }


	void Start () {

	}
	
	void Update () {
        this.headPos = sw.bonePos[PlayerId, 3];
        Camera.main.transform.position = this.headPos + new Vector3(2, 1, -8);
    }
}
