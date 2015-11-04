using UnityEngine;
using System.Collections;

public class wallTexture : MonoBehaviour {

	void Start () {
        transform.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, 100);
    }
	
	void Update () {

    }
}
