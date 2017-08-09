using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HitAndRun.Proto
{
	public class TextMod : MonoBehaviour {

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void LateUpdate () {
			this.transform.LookAt (Camera.main.transform.position);
			this.transform.Rotate (new Vector3 (0, 180, 0));
		}
	}
}