using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pPowerUps : MonoBehaviour {
    public float direction = 1f;
    public float speed = 5;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.Rotate(Vector3.up, (speed * direction) * Time.deltaTime);
    }
}
