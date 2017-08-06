using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace HitAndRun.Proto
{
    public class pDoorSlide : pDoor
    {
        public override void OpenCloseDoor()
        {
            if (dooropen)
            {
                doormesh.transform.localPosition = Vector3.Lerp(doormesh.transform.localPosition, new Vector3(-1.67f, 0.35f, 0.50f), speed * Time.deltaTime);
            }
            else
            {
                doormesh.transform.localPosition = Vector3.Lerp(doormesh.transform.localPosition, new Vector3(1.61f, 0.35f, 0.50f), speed * Time.deltaTime);
            }
        }
    }


}