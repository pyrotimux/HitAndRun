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
            //GetComponentInChildren<TextMesh>().text = "Interact with this door.";
			Player p = o.gameObject.GetComponent<Player>();
            bool s = o.name.StartsWith("player");
            if (s && !p.infected)
            {
                dooropen = true;
                playDoorSound();

            }
            else if (s && p.infected)
            {
                GetComponentInChildren<TextMesh>().text = "Door opening in 2 sec.";
                StartCoroutine(openDoor(2.0f));

            }
        }

        private void OnTriggerExit(Collider o)
        {
            //GetComponentInChildren<TextMesh>().text = "";
            dooropen = false;
            playDoorSound();
        }

        public IEnumerator openDoor(float time)
        {
            yield return new WaitForSeconds(time);
            dooropen = true;
            GetComponentInChildren<TextMesh>().text = "";
            playDoorSound();
        }

        public void ChangeDoorStatus()
        {
            dooropen = !dooropen;
        }

        
    }
}