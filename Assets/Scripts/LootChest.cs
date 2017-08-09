using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace HitAndRun.Proto
{
    public class LootChest : ILootContainer
    {
        private int defaultvar = 11;
        private Light plight;

        [SyncVar]
        public int countdown = 11;

        private void Start()
        {
            plight = GetComponentInChildren<Light>();
            plight.enabled = false;
            StartCoroutine(delayStart(2));
        }

        public IEnumerator delayStart(float time)
        {
            yield return new WaitForSeconds(time);
			spawner = GameObject.Find("SpawnManager").gameObject.GetComponent<SpawnMgr>();
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
                if (canspawnkey)
                    spawner.SpawnKey(gameObject.transform.GetChild(3).gameObject.transform);
                else
                    spawner.Spawn(gameObject.transform.GetChild(3).gameObject.transform);
                // disable loot chest
                GetComponentInChildren<TextMesh>().text = "";
                GetComponentInChildren<Light>().enabled = false;
                Destroy(gameObject.GetComponent<LootChest>());

                gameObject.transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);

            }


        }

        public override void OnChestOpening(Collider o) {
            Player p = o.gameObject.GetComponent<Player>();
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