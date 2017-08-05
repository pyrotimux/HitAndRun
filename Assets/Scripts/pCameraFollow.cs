using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pCameraFollow : MonoBehaviour {
	static public Transform player;
	public float smooth;

	private Vector3 offset;

	 
	void Start () {
		offset = new Vector3(-0.12f,1.88f,-3.34f);
	}
	
	
	void Update () {
		transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * smooth);
		transform.rotation = player.rotation;
		transform.LookAt (player.position);
	}
}
