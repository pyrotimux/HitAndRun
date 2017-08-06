using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace HitAndRun.Proto
{
    public class pPlayerMove : pEntityIface
    {
        public float speed;
        public float rotateSpeed;
        public bool go = false;


        public override void MoveEntity()
        {
            float transl = (CrossPlatformInputManager.GetAxis("Vertical") * speed) * Time.deltaTime;
            float rotation = (CrossPlatformInputManager.GetAxis("Horizontal") * rotateSpeed) * Time.deltaTime;
            if (!go)
            {
                transform.Translate(0, 0, transl);
                transform.Rotate(0, rotation, 0);
            }
        }

    }

}
