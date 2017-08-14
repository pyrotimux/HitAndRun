using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HitAndRun.Proto
{
    public class HeartBeat : MonoBehaviour {
        private AudioSource audheartbeat;
        public AudioClip heartclip;
        public bool canplay = true;

        // Use this for initialization
        void Start() {
            audheartbeat = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update() {

        }

        private void PlayHeartBeat() {
            if(canplay) audheartbeat.PlayOneShot(heartclip, 0.7f);
        }

        public void OnTriggerEnter(Collider other)
        {
            Player o = other.GetComponent<Player>();
            if (o != null && o.infected == true) InvokeRepeating("PlayHeartBeat", 0.0f, 1.5f); 
        }

        public void OnTriggerExit(Collider other)
        {
            Player o = other.GetComponent<Player>();
            if (o != null && o.infected == true) CancelInvoke("PlayHeartBeat");
        }


    }
}