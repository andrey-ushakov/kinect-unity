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

	public void OnHeadTrackingClicked() {
		Application.LoadLevel("HeadTracking");
	}

	public void OnGameClicked() {
		Application.LoadLevel("Game");
	}
}
