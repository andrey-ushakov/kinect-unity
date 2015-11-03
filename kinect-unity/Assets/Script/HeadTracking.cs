using UnityEngine;
using System;
using System.Collections;

public class HeadTracking : MonoBehaviour {
	
	private SkeletonWrapper sw;
    private HandInputManager him;
    private int PlayerId = 0;
    public Vector3 Origin = new Vector3(-4,3,12);

    private Vector3 headPos;

    void OnEnable() {
        HandInputManager.handMotionDetected += GoBackToMenu;
    }

    void OnDisable() {
        HandInputManager.handMotionDetected -= GoBackToMenu;
    }


    void Awake() {
        sw = SkeletonWrapper.Instance;
        him = HandInputManager.Instance;
    }


	void Start () {

	}
	
	void Update () {
        this.headPos = sw.bonePos[PlayerId, 3];
        Camera.main.transform.position = this.headPos + Origin;
    }

    void GoBackToMenu(object sender, HandMotionDetectedEventArgs args) {
        if (args.motion == HandMotion.TWO_HAND_RISE) {
            Debug.Log("GO BACK TO MENU");
        }
    }

}
