using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace HitAndRun.Proto
{
    public class Survivor : IPlayer
    {

        //private Enemy enemyscript;
        public Material pmatblue, pmatred, pmatblack, pmatyellow, pmatgreen, pmat; // materials used to customize player

        public override void InitStart()
        {
            base.InitStart();
            if (isLocalPlayer)
            {
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
            
        }

        public override void PlayerCheckStatus()
        {
            if (infected)
            {
                if (haskey)
                {
                    haskey = false;
                    CmdPlayerDropKey();
                    SpawnMgr pspwn = GameObject.Find("SpawnManager").GetComponent<SpawnMgr>();
                    pspwn.SpawnKey(transform);
                }
                Transform o = transform.GetChild(2);
                o.GetComponent<MeshRenderer>().enabled = true;
                o.GetComponent<Renderer>().material.color = Color.magenta;

                // turn to bad guy.
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(4).gameObject.SetActive(true);

                //enemyscript = GetComponent<Enemy>();
                //enemyscript.enabled = true;

                Enemy enm = gameObject.AddComponent<Enemy>();
                enm.pname = pname;
                enm.pcolor = pcolor;
                enm.evileffect = evileffect;

                this.enabled = false;
                Destroy(this);

                if (isLocalPlayer)
                {
                    infoscr.GetChild(1).gameObject.GetComponent<Text>().text = "Infected";
                    infoscr.GetChild(0).gameObject.GetComponent<Text>().text = "";
                    infoscr.GetChild(2).gameObject.GetComponent<Text>().text = "G 10  S A";
                    playermoves.speed = 9;
                }


            }
            else if (!infected && haskey)
            {
                Transform o = transform.GetChild(2);
                o.GetComponent<MeshRenderer>().enabled = true;
                o.GetComponent<Renderer>().material.color = Color.green;
            }


        }

        public override void PlayerCollisionEnter(Collision col, string s)
        {
            if (infected) return;

            if (s.StartsWith("key") && !infected)
            { // we can pick up the key as long as we are not infected.
                NetworkServer.Destroy(col.gameObject);
                CmdPlayerGotKey();
            }
            else if (s.StartsWith("infections"))
            { // we got infected from world item
                NetworkServer.Destroy(col.gameObject);
                CmdPlayerInfected();
                audsrc.Play();
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
                    WinScreen();
                }

                // we then update the sync var.
                CmdPlayerReachGate();

                StartCoroutine(MoveLvl(2));


            }
            else if (s.StartsWith("spwn"))
            {
                // we handle powerups here.
                if (isLocalPlayer)
                {
                    if (s.StartsWith("spwnelec"))
                    {
                        powerstr = "G 10  S";
                        powerup = 1;
                    }
                    else if (s.StartsWith("spwnfire"))
                    {
                        powerstr = "G 10  A";
                        powerup = 2;
                    }

                }


                NetworkServer.Destroy(col.gameObject);
            }
        }

        public override void PlayerKeyDown()
        {
            if (infected) return;

            if (Input.GetButtonDown("PowerUp1"))
            {

                if (powerup == 0) return;

                if (powerup == 1)
                {
                    playermoves.speed = 13;
                    countdown = 10;
                    InvokeRepeating("PowerSpeed", 0.0f, 1.0f);
                    powerup = 0;
                }
                else if (powerup == 2)
                {
                    CmdLightControl();
                    CmdMeshControl();
                    countdown = 10;
                    InvokeRepeating("PowerInvisb", 0.0f, 1.0f);
                    powerup = 0;
                    globe_light.GetComponent<Light>().enabled = true;
                }


            }
        }

        public IEnumerator MoveLvl(float time)
        {
            yield return new WaitForSeconds(time);
            GameObject t1 = GameObject.Find("t1");
            transform.position = t1.transform.position;
            transform.rotation = t1.transform.rotation;
        }

        protected void PowerSpeed()
        {
            if (countdown > 0)
            {
                countdown--;
                powerstr = "G " + countdown + "  S";
            }
            else
            {
                playermoves.speed = 7;
                powerstr = "";
                countdown = 10;
                CancelInvoke("PowerSpeed");

            }

        }

        protected void PowerInvisb()
        {
            if (countdown > 0)
            {
                countdown--;
                powerstr = "G " + countdown + "  A";
            }
            else
            {
                CmdMeshControl();
                powerstr = "";
                countdown = 10;
                CmdLightControl();
                globe_light.GetComponent<Light>().enabled = false;
                CancelInvoke("PowerInvisb");

            }

        }

        public void WinScreen()
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject endscr = canvas.transform.GetChild(0).gameObject;

            endscr.SetActive(true);
            Transform endtxt = endscr.transform.GetChild(0);
            if (gaterch) endtxt.GetComponent<Text>().text = "You Win!";
        }

    }
}