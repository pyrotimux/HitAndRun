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
        public override void InitStart()
        {
            base.InitStart();
            infected = true;
        }

        public override void PlayerCheckStatus()
        {
        }

        public override void PlayerCollisionEnter(Collision col, string s)
        {
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