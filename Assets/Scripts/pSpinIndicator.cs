using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pSpinIndicator : MonoBehaviour {

    private Vector3 rotateOn = new Vector3(0,1,0);
    public float speed = 5;
    public Transform around;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.RotateAround(around.transform.position, rotateOn, speed * Time.deltaTime);
    }
}
