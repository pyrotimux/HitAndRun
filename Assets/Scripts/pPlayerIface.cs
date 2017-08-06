using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace HitAndRun.Proto
{
    public abstract class pPlayerIface : NetworkBehaviour
    {
        public abstract void checkStatus();
        public abstract void Start();
    }
}