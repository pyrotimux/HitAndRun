﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace HitAndRun.Proto
{
    public class pPlayer : pPlayerIface
    {

        [SyncVar]
        public string pname = "player"; // value set on run time by lobby hook

        [SyncVar]
        public Color pcolor = Color.red; // value sset on run time by lobby hook

        [SyncVar]
        public bool haskey = false; // tells if the player has key to unlock gate or not

        [SyncVar]
        public bool infected = false; // tells infected state

        [SyncVar]
        public bool gaterch = false; // tells if gate was reached or not

        [SyncVar]
        public bool lighton = true;

        public Material pmatblue, pmatred, pmatblack, pmatyellow, pmatgreen, pmat; // materials used to customize player

        private pPlayerMove playermoves; // define how player is moved 
        public AudioClip evileffect;
        private AudioSource audsrc;
        private GameObject canvas;
        private Transform infoscr, goLight;
        private TextMesh goName;
        private Light lightcnt;
        private int neg = 1;
        private int powerup = 0, countdown = 10;
        private string powerstr = "";

        // Use this for initialization
        public override void Start()
        {

            if (isLocalPlayer)
            {
                // we will enable the local player movement but not network.
                playermoves = GetComponent<pPlayerMove>();
                playermoves.enabled = true;

                // this makes the camera offset and then follow the player by attaching to it.
                Camera.main.transform.position = this.transform.position - this.transform.forward * 5 + this.transform.up * 1;
                Camera.main.transform.LookAt(this.transform.position);
                Camera.main.transform.position = this.transform.position - this.transform.forward * 5 + this.transform.up * 3;
                Camera.main.transform.parent = this.transform;

            }

            // Set the name and color of the player depending on what was chosen in the lobby.

            if (pcolor == Color.blue)
                pmat = pmatblue;
            else if (pcolor == Color.green)
                pmat = pmatgreen;
            else if (pcolor == Color.yellow)
                pmat = pmatyellow;
            else if (pcolor == Color.red)
                pmat = pmatred;
            else if (pcolor == Color.black)
                pmat = pmatblack;
            else
                pmat = pmatred;

            GetComponentInChildren<TextMesh>().text = pname;
            GetComponentInChildren<TextMesh>().color = pcolor;

            transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = pmat;

            if (isLocalPlayer) {
                audsrc = transform.GetChild(2).gameObject.GetComponent<AudioSource>();
                audsrc.clip = evileffect;
            }
            

            canvas = GameObject.Find("Canvas");
            infoscr = canvas.transform.GetChild(1).gameObject.transform;

            goName = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMesh>();
            goLight = gameObject.transform.GetChild(3).gameObject.transform;
            lightcnt = goLight.GetComponent<Light>();

            // we can use this to make player invisible by disabling the rendering engine.
            //Renderer[] rends = GetComponentsInChildren<Renderer>();
            //foreach (Renderer r in rends)
            //    r.material.color = pcolor;
        }

        // handles all player collision in this class
        void OnCollisionEnter(Collision col)
        {
            string s = col.gameObject.name; // name of the collider 


            if (s.StartsWith("player") && infected)
            { // we are infecting other players 
                pPlayer p = col.gameObject.GetComponent<pPlayer>();
                p.infected = true;
                p.CmdPlayerInfected();
                if (isLocalPlayer) audsrc.Play();
            }
            else if (s.StartsWith("key") && !infected)
            { // we can pick up the key as long as we are not infected.
                NetworkServer.Destroy(col.gameObject);
                CmdPlayerGotKey();
            }
            else if (s.StartsWith("infections"))
            { // we got infected from world item
                NetworkServer.Destroy(col.gameObject);
                CmdPlayerInfected();
                if (isLocalPlayer) audsrc.Play();
            }
            else if (s.StartsWith("gatebarrier") && haskey)
            { // we are destroying gatebarrier as long as we have the key
                NetworkServer.Destroy(col.gameObject);
            }
            else if (s.StartsWith("wingate") && !infected)
            { // we will enter win gate as long as we are not infected and win the game.
                if (isLocalPlayer)
                { // we disabled the local player movement and display win screen
                    playermoves.go = true;
                    //StartCoroutine(winscreen(1));
                    winscreen();
                }

                // we then update the sync var.
                CmdPlayerReachGate();

                StartCoroutine(moveLvl(2));


            }
            else if (s.StartsWith("spwn") && infected)
            {
                NetworkServer.Destroy(col.gameObject);
            }
            else if (s.StartsWith("spwn"))
            {
                // we handle powerups here.
                if (isLocalPlayer) {
                    //if (s.StartsWith("spwnspeed"))
                    powerstr = "G 10  S";
                    powerup = 1;
                }
                    

                NetworkServer.Destroy(col.gameObject);
            }

        }

        private void TurnCamera() {
            Camera.main.transform.Rotate(new Vector3(20, 180, 0));
            Camera.main.transform.position = this.transform.position + (neg * this.transform.forward * 5) + this.transform.up * 3;
            goLight.Rotate(new Vector3(140, -180, 0));

            neg *= -1;
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            if (Input.GetButtonDown("Hide"))
            {
                CmdLightControl();
            }

            if (Input.GetButtonDown("Camera"))
            {
                TurnCamera();
            }

            if (Input.GetButtonDown("PowerUp1") && powerup == 1)
            {
                playermoves.speed = 13;
                countdown = 10;
                InvokeRepeating("PowerSpeed", 0.0f, 1.0f);
                
                
            }

           

        }

        private void PowerSpeed() {
            if (countdown > 0)
            {
                countdown--;
                powerstr = "G " + countdown + "  S";
            }
            else
            {
                playermoves.speed = 10;
                powerstr = "";
                powerup = 0;
                countdown = 10;
                CancelInvoke("PowerSpeed");

            }
            
        }

        private void LateUpdate()
        {
            checkStatus();

            if(isLocalPlayer)
                infoscr.GetChild(2).gameObject.GetComponent<Text>().text = powerstr;
        }



        public override void checkStatus()
        {

            if (infected)
            {
                if (haskey)
                {
                    haskey = false;
                    CmdPlayerDropKey();
                    pSpawner pspwn = GameObject.Find("SpawnManager").GetComponent<pSpawner>();
                    pspwn.SpawnKey(transform);
                }
                Transform o = transform.GetChild(2);
                o.GetComponent<MeshRenderer>().enabled = true;
                o.GetComponent<Renderer>().material.color = Color.magenta;

                // turn to bad guy.
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(4).gameObject.SetActive(true);

                if (isLocalPlayer) {
                    infoscr.GetChild(1).gameObject.GetComponent<Text>().text = "Infected";
                    infoscr.GetChild(0).gameObject.GetComponent<Text>().text = "";
                    infoscr.GetChild(2).gameObject.GetComponent<Text>().text = "G 10  S A";
                }
                

            }
            else if (!infected && haskey)
            {
                Transform o = transform.GetChild(2);
                o.GetComponent<MeshRenderer>().enabled = true;
                o.GetComponent<Renderer>().material.color = Color.green;
            }

            if (lighton)
            {
                goName.text = pname;
                lightcnt.enabled = lighton;
            }
            else
            {
                goName.text = "";
                lightcnt.enabled = lighton;
            }



        }


        [Server]
        void CmdPlayerGotKey()
        {
            haskey = true;
        }

        [Server]
        void CmdPlayerDropKey()
        {
            haskey = false;
        }

        [Server]
        void CmdPlayerInfected()
        {
            infected = true;
        }

        [Server]
        void CmdPlayerReachGate()
        {
            gaterch = true;
        }

        [Command]
        void CmdLightControl()
        {
            lighton = !lighton;
        }


        public IEnumerator moveLvl(float time)
        {
            yield return new WaitForSeconds(time);
            GameObject t1 = GameObject.Find("t1");
            transform.position = t1.transform.position;
            transform.rotation = t1.transform.rotation;
        }

        public void winscreen()
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject endscr = canvas.transform.GetChild(0).gameObject;

            endscr.SetActive(true);
            Transform endtxt = endscr.transform.GetChild(0);
            if (gaterch) endtxt.GetComponent<Text>().text = "You Win!";
        }

    }
}