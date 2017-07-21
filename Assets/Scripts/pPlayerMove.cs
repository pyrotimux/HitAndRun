using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class pPlayerMove : MonoBehaviour {
	public float speed;
	public float rotateSpeed;
    public bool go = false;
    
	void FixedUpdate () {
		float transl = (CrossPlatformInputManager.GetAxis ("Vertical") * speed) * Time.deltaTime;
		float rotation = (CrossPlatformInputManager.GetAxis ("Horizontal") * rotateSpeed) * Time.deltaTime;
        if (!go) {
            transform.Translate(0, 0, transl);
            transform.Rotate(0, rotation, 0);
        }
        
	}
}
