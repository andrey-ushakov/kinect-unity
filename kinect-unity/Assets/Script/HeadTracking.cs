using UnityEngine;
using System;
using System.Collections;

public class HeadTracking : MonoBehaviour {
	
	private SkeletonWrapper sw;
    private HandInputManager him;
    private int PlayerId = 0;

    private Vector3 headPos;
    private Vector3 preHeadPos;

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
        this.headPos = sw.bonePos[PlayerId, 3];
        this.preHeadPos = this.headPos;
	}
	
	void Update () {
        this.headPos = sw.bonePos[PlayerId, 3];
        Vector3 headVelo = this.headPos - this.preHeadPos;
        Camera.main.transform.position += headVelo;
        this.preHeadPos = this.headPos;
    }

    void GoBackToMenu(object sender, HandMotionDetectedEventArgs args) {
        if (args.motion == HandMotion.TWO_HAND_RISE) {
            //TODO: Change the code here, implement going back to menu function
            Debug.Log("GO BACK TO MENU");
        }
    }

}
