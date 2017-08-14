using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace HitAndRun.Proto
{
    public class Enemy : IPlayer
    {
        private Light glblght;

        public void PlayerGotInfected(string pname, Color pcolor) {
            this.pname = pname;
            this.pcolor = pcolor;

            infected = true;
            if (isLocalPlayer)
            {
                glblght = GameObject.Find("global_sun").GetComponent<Light>();
                glblght.enabled = true;
            }

        }

        public override void InitStart()
        {
            base.InitStart();
        }

        public override void PlayerCheckStatus()
        {
        }

        public override void PlayerCollisionEnter(Collision col, string s)
        {
            if (!infected) return;

            if (s.StartsWith("player") && infected)
            { // we are infecting other players 
                Survivor p = col.gameObject.GetComponent<Survivor>();
                p.infected = true;
                p.CmdPlayerInfected();
                audsrc.Play();
            }
            else if (s.StartsWith("spwn") && infected)
            {
                NetworkServer.Destroy(col.gameObject);
            }
        }

        public override void PlayerKeyDown()
        {
            if (!infected) return;

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


    }
}