using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace HitAndRun.Proto {
    public abstract class pEntityIface : NetworkBehaviour
    {

        public abstract void MoveEntity();

        public void FixedUpdate()
        {
            MoveEntity();
        }
    }
}

