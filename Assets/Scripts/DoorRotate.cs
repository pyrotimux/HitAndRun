using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace HitAndRun.Proto
{
    public class DoorRotate : IDoor
    {

        private float openAng = -90f;
        

        public override void OpenCloseDoor()
        {
            if (dooropen)
            {
                doorcont.transform.localRotation = Quaternion.Slerp(doorcont.transform.localRotation, Quaternion.Euler(0, openAng, 0), speed * Time.deltaTime);
            }
            else
            {
                doorcont.transform.localRotation = Quaternion.Slerp(doorcont.transform.localRotation, Quaternion.Euler(0, 0, 0), speed * Time.deltaTime);
            }
        }
    }
}