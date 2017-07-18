using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class pPlayerMove : MonoBehaviour {

	public float speed;
	public float rotateSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float transl = (CrossPlatformInputManager.GetAxis ("Vertical") * speed) * Time.deltaTime;
		float rotation = (CrossPlatformInputManager.GetAxis ("Horizontal") * rotateSpeed) * Time.deltaTime;
		transform.Translate (0, 0, transl);
		transform.Rotate (0, rotation, 0);
	}
}
