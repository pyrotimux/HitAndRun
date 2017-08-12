using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace HitAndRun.Proto
{
	public abstract class IDoor : NetworkBehaviour
    {
        [SyncVar]
        public bool dooropen = true;

        public float speed = 10f;
        public GameObject doorcont, doormesh;
        public AudioClip dooreffect;

        public abstract void OpenCloseDoor();
        private int countdown = 3;
        private bool inzone = false;

        // Use this for initialization
        void Start()
        {
            doorcont = transform.GetChild(0).gameObject;
            doormesh = transform.GetChild(0).GetChild(0).gameObject;

        }

		public void FixedUpdate()
		{
			OpenCloseDoor();
		}

        private void playDoorSound()
        {
            AudioSource audio = gameObject.transform.GetChild(2).GetComponent<AudioSource>();
            audio.clip = dooreffect;
            audio.Play();
        }

        private void OnTriggerEnter(Collider o)
        {
            inzone = true;
            //GetComponentInChildren<TextMesh>().text = "Interact with this door.";
            IPlayer p = o.gameObject.GetComponent<Survivor>();
            if (p == null) p = o.gameObject.GetComponent<Enemy>();

            bool s = o.name.StartsWith("player");
            if (s && !p.infected)
            {
                ChangeDoorStatus(true);
                playDoorSound();

            }
            else if (s && p.infected)
            {
                
                InvokeRepeating("OpenDoorAfterCount", 0.0f, 1.0f);

            }
            
        }

        private void OnTriggerExit(Collider o)
        {
            inzone = false;
            //GetComponentInChildren<TextMesh>().text = "";
            ChangeDoorStatus(false);
            playDoorSound();
        }

        public IEnumerator openDoor(float time)
        {
            yield return new WaitForSeconds(time);
            ChangeDoorStatus(true);
            GetComponentInChildren<TextMesh>().text = "";
            playDoorSound();
        }

        private void OpenDoorAfterCount()
        {
            if (!inzone) {
                CancelDoorOpen();
                return;
            } 

            if (countdown > 0)
            {
                GetComponentInChildren<TextMesh>().text = "Door opening in: " + countdown;
                countdown--;
            }
            else
            {
                ChangeDoorStatus(true);
                playDoorSound();
                CancelDoorOpen();

            }

        }

        private void CancelDoorOpen() {
            GetComponentInChildren<TextMesh>().text = "";
            CancelInvoke("OpenDoorAfterCount");
            countdown = 3;
        }

        public void ChangeDoorStatus(bool status)
        {
            dooropen = status;
        }

        
    }
}