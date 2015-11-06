using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {
	Vector2 mouse;
	int w = 64;
	int h = 64;
	public Texture2D cursor;
	
	void Start()
	{
		Cursor.visible = false;
	}
	
	void Update()
	{
		mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
	}
	
	void OnGUI()
	{
		GUI.DrawTexture(new Rect(mouse.x, mouse.y, w, h), cursor);
	}

}
