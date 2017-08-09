using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HitAndRun.Proto
{
	public class SpinEarth : MonoBehaviour
	{
	    public bool spinParent, spin;
	    public float speed = 10f;
	    public bool clockwise = true;
	    public float direction = 1f;
	    public float directionChangeSpeed = 2f;

	    void Update()
	    {
	        
	        if (direction < 1f)
	        {
	            direction += Time.deltaTime / (directionChangeSpeed / 2);
	        }

	        if (spin)
	        {
	            if (clockwise)
	            {
	                if (spinParent)
	                    transform.parent.transform.Rotate(Vector3.up, (speed * direction) * Time.deltaTime);
	                else
	                    transform.Rotate(Vector3.up, (speed * direction) * Time.deltaTime);
	            }
	            else
	            {
	                if (spinParent)
	                    transform.parent.transform.Rotate(-Vector3.up, (speed * direction) * Time.deltaTime);
	                else
	                    transform.Rotate(-Vector3.up, (speed * direction) * Time.deltaTime);
	            }
	        }
	    }
	}
}