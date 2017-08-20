using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitAndRun.Proto
{
    public class CameraFollow : MonoBehaviour {
        public bool camActive = false;
        public int curplayer = 0;
        GameObject[] players;

        // Use this for initialization
        void Start() {
            StartCoroutine(DelayStart(2));
        }

        public IEnumerator DelayStart(float time)
        {
            yield return new WaitForSeconds(time);
            players = GameObject.FindGameObjectsWithTag("Player");


        }

        // Update is called once per frame
        void Update() {

        }

        public void AttachOnPlayer() {
            if (curplayer + 1 < players.Length)
            {
                curplayer += 1;
            }
            else
            {
                curplayer = 0;
            }
            Debug.Log(curplayer);
            Player p = players[curplayer].GetComponent<Player>();
            if (p.infected || p.gaterch)
                return;

            GameObject curp = players[curplayer];

            Camera.main.transform.position = curp.transform.position - curp.transform.forward * 5 + curp.transform.up * 1;
            Camera.main.transform.LookAt(curp.transform.position);
            Camera.main.transform.position = curp.transform.position - curp.transform.forward * 5 + curp.transform.up * 3;
            Camera.main.transform.parent = curp.transform;
        }

        public void StartFollowing() {
            InvokeRepeating("AttachOnPlayer", 2.0f, 10.0f);
        }
    }
}