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
		Debug.Log ("Act1");
		Application.LoadLevel("action1");
	}

	public void OnGameClicked() {
		Debug.Log ("Act2");
	}
}
