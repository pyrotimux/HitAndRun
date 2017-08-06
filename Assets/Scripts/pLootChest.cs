using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace HitAndRun.Proto
{
    public class pLootChest : pLootContIface
    {
        private int defaultvar = 11;
        private Light plight;

        [SyncVar]
        public int countdown = 11;

        private void Start()
        {
            plight = GetComponentInChildren<Light>();
            plight.enabled = false;
        }

        public override void OpenChest()
        {
            if (countdown > 0)
            {
                countdown--;
                GetComponentInChildren<TextMesh>().text = "" + countdown;
            }
            else
            {
                GameObject.Find("SpawnManager").gameObject.GetComponent<pSpawner>().SpawnKey(gameObject.transform.GetChild(3).gameObject.transform);
                GetComponentInChildren<TextMesh>().text = "";
                GetComponentInChildren<Light>().enabled = false;
                Destroy(gameObject.GetComponent<pLootChest>());

                gameObject.transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);

            }


        }

        public override void OnChestOpening(Collider o) {
            pPlayer p = o.gameObject.GetComponent<pPlayer>();
            bool s = o.name.StartsWith("player");
            if (s && !p.infected)
            {
                InvokeRepeating("OpenChest", 0.0f, 2.0f);
                countdown = defaultvar;
                plight.enabled = true;
            }
        }

        public override void OnNoLongerOpening(Collider o) {
            GetComponentInChildren<TextMesh>().text = "";
            countdown = defaultvar;
            CancelInvoke();
            plight.enabled = false;
        }

        
    }
}