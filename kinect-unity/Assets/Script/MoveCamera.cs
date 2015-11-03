using UnityEngine;
using System.Collections;


public class MoveCamera : MonoBehaviour {
    public float speed = 10.0F;

	void Start () {
	
	}
	
	void Update () {

        //float translX = Input.GetAxis("Horizontal") * speed;
        //translX *= Time.deltaTime;

        //float translY = Input.GetAxis("Vertical") * speed;
        //translY *= Time.deltaTime;

        //float translZ = Input.GetAxis("Zoom") * speed;
        //translZ *= Time.deltaTime;

        //transform.Translate(translX, translY, translZ);

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            Camera.main.transform.Translate(0f, speed, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Camera.main.transform.Translate(0f, -speed, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            Camera.main.transform.Translate(-speed, 0f, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Camera.main.transform.Translate(speed, 0f, 0f);
        }

    }
}
