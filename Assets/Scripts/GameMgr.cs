﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace HitAndRun.Proto
{
    public class GameMgr : NetworkBehaviour
    {
        public bool gameover = false;
        private int totinfected = 0, numplayers = 0;
        public GameObject endscr;
        private bool delayed = true;

        private GameObject pl;

        public bool revenge = false;

        public IEnumerator delayStart(float time)
        {
            yield return new WaitForSeconds(time);
            delayed = false;
            
        }

        public void Start()
        {
            GameObject[] chests = GameObject.FindGameObjectsWithTag("chests");
            UnityEngine.Random.InitState(System.Environment.TickCount);
            chests[UnityEngine.Random.Range(0, chests.Length)].GetComponent<LootChest>().canspawnkey = true;
            StartCoroutine(delayStart(2));
        }


        void CheckGameStatus() {
            totinfected = 0;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            numplayers = players.Length;
            foreach (GameObject p in players)
            {
                IPlayer scr = p.GetComponent<Survivor>();

                if(scr == null) scr = p.GetComponent<Enemy>();

                if (!scr.infected && !scr.gaterch)
                {
                    gameover = false;
                    break;
                }



                if (scr.infected) totinfected++;
                gameover = true;

            }

            if (gameover == true)
            {
                if (numplayers == totinfected) { revenge = true; }
                foreach (GameObject p in players)
                {
                    p.GetComponent<PlayerMovements>().go = true;

                }

                endscr.SetActive(true);
                Transform endtxt = endscr.transform.GetChild(0);
                if (revenge) endtxt.GetComponent<Text>().text = "Revenge Success!";
                else endtxt.GetComponent<Text>().text = "Revenge Failed!";




            }

        }
    

        void LateUpdate()
        {
            if (delayed)
            {
                return;
            }

            CheckGameStatus();
        }


    }
}