using UnityEngine;
using System.Collections;

public class FinalDialog : MonoBehaviour {

	int h = 30;
	int dlgWidth = 600;
	int dlgHeight = 250;

	public Rect windowRect;


	void OnGUI() {
		windowRect = GUI.Window(0, new Rect( (Screen.width-dlgWidth)/2, (Screen.height-dlgHeight)/2, dlgWidth, dlgHeight), ShowFinalDialog, "Game Over");
	}


	void ShowFinalDialog(int windowID) {

		string text = "";
		switch(GameManager.Instance.GetGameMode) {
		case GameMode.LimitedLife :
			text = "Time passed : " + GameManager.Instance.TimePassed.ToString("F1") + " s";
			break;
		case GameMode.LimitedTime:
			text = "Score : " + GameManager.Instance.Score;
			break;
		}

		// rect : left margin, top margin, width, height
		GUI.Label (new Rect(150, dlgHeight/2-h, dlgWidth, h) , "<size=24>" + text + "</size>");

		GUI.DrawTexture(new Rect(10, dlgHeight-64-10, 64, 64), GameManager.Instance.textureRightHandUp, ScaleMode.ScaleToFit, true, 0);
		GUI.Label (new Rect(80, dlgHeight-h-32+10, dlgWidth, h) , "<size=20>" + "Put your right hand up for return to main menu" + "</size>");
		
	}
}
