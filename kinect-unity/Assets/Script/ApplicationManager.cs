using UnityEngine;
using System.Collections;

public class ApplicationManager : MonoBehaviour {



	public void Quit () {
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}

	public void OnLimitedTimeClicked() {
		GlobalVariables.gameMode = GameMode.LimitedTime;
		RightHandTracking rht = gameObject.GetComponent<RightHandTracking> ();
		rht.enabled = false;
		Application.LoadLevel("Game");
	}

	public void OnLimitedLifeClicked() {
		GlobalVariables.gameMode = GameMode.LimitedLife;
		RightHandTracking rht = gameObject.GetComponent<RightHandTracking> ();
		rht.enabled = false;
		Application.LoadLevel("Game");
	}
}
