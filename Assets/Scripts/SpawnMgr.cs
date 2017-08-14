using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace HitAndRun.Proto
{
    public class SpawnMgr : NetworkBehaviour
    {
        public Transform infspwn;
        public Transform lkeypos, lspawnpos;
        public GameObject keyprefab, infectprefab;

        public GameObject[] pSpawnables;
        private int spawncount;

        // Use this for initialization
        void Start()
        {
            CmdInfectSpwn();
            spawncount = UnityEngine.Random.Range(0, pSpawnables.Length);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Spawn(Transform p) {
            lspawnpos = p;
            CmdSpawn();
        }

        public void SpawnKey(Transform p)
        {
            lkeypos = p;
            CmdKeySpwn();
        }

        [Command]
        void CmdSpawn()
        {
            GameObject spwnitem = (GameObject)Instantiate(pSpawnables[spawncount], new Vector3(lspawnpos.position.x, 10, lspawnpos.position.z), Quaternion.identity);
            NetworkServer.Spawn(spwnitem);

            if (spawncount == 2) spawncount = 0;
            else spawncount++;
        }


        [Command]
        void CmdKeySpwn()
        {
            GameObject key = (GameObject)Instantiate(keyprefab, new Vector3(lkeypos.position.x, 10, lkeypos.position.z), Quaternion.identity);
            NetworkServer.Spawn(key);
        }

        [Command]
        void CmdInfectSpwn()
        {
            GameObject infection = (GameObject)Instantiate(infectprefab, new Vector3(infspwn.position.x, 10, infspwn.position.z), Quaternion.identity);
            NetworkServer.Spawn(infection);
        }
    }
}