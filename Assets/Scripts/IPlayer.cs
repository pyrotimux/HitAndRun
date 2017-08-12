using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace HitAndRun.Proto
{
    public abstract class IPlayer : NetworkBehaviour
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

        [SyncVar]
        public bool meshon = true;

        

        protected PlayerMovements playermoves; // define how player is moved 
        public AudioClip evileffect;
        protected AudioSource audsrc;
        protected GameObject canvas, globe_light;
        protected Transform infoscr, goLight;
        protected TextMesh goName;
        protected Light lightcnt;
        protected int neg = 1, powerup = 0, countdown = 10;
        protected string powerstr = "";

        public abstract void PlayerCollisionEnter(Collision col, string colname);
        public abstract void PlayerKeyDown();
        public abstract void PlayerCheckStatus();

        // Use this for initialization
        public void Start()
        {
            InitStart();
        }

        public virtual void InitStart()
        {
            if (isLocalPlayer)
            {
                // we will enable the local player movement but not network.
                playermoves = GetComponent<PlayerMovements>();
                playermoves.enabled = true;
            }
            audsrc = transform.GetChild(2).gameObject.GetComponent<AudioSource>();
            audsrc.clip = evileffect;

            canvas = GameObject.Find("Canvas");
            infoscr = canvas.transform.GetChild(1).gameObject.transform;

            goName = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMesh>();
            goLight = gameObject.transform.GetChild(3).gameObject.transform;
            lightcnt = goLight.GetComponent<Light>();

            globe_light = GameObject.Find("global_light");
        }



        // handles all player collision in this class
        void OnCollisionEnter(Collision col)
        {
            string s = col.gameObject.name; // name of the collider 
            PlayerCollisionEnter(col,s);

        }

        protected void TurnCamera()
        {
            Camera.main.transform.Rotate(new Vector3(20, 180, 0));
            Camera.main.transform.position = this.transform.position + (neg * this.transform.forward * 5) + this.transform.up * 3;
            goLight.Rotate(new Vector3(140, -180, 0));

            neg *= -1;
        }

        protected void Update()
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

            PlayerKeyDown();



        }

        

        public void LateUpdate()
        {
            PlayerCheckStatus();

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

            if (meshon && !infected)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }

            if (isLocalPlayer)
                infoscr.GetChild(2).gameObject.GetComponent<Text>().text = powerstr;
        }


        [Server]
        public void CmdPlayerGotKey()
        {
            haskey = true;
        }

        [Server]
        public void CmdPlayerDropKey()
        {
            haskey = false;
        }

        [Server]
        public void CmdPlayerInfected()
        {
            infected = true;
        }

        [Server]
        public void CmdPlayerReachGate()
        {
            gaterch = true;
        }

        [Command]
        public void CmdLightControl()
        {
            lighton = !lighton;
        }

        [Command]
        public void CmdMeshControl()
        {
            meshon = !meshon;
        }
        

    }
}