using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace HitAndRun.Proto
{
    public abstract class pLootContIface : NetworkBehaviour
    {

        public abstract void OpenChest();
        public abstract void OnChestOpening(Collider o);
        public abstract void OnNoLongerOpening(Collider o);

        public void OnTriggerEnter(Collider o)
        {
            OnChestOpening(o);
        }

        public void OnTriggerExit(Collider o)
        {
            OnNoLongerOpening(o);
        }
    }
}
