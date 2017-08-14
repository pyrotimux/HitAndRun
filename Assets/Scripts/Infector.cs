using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HitAndRun.Proto
{
	public class Infector : IEntity
    {

        private GameObject[] players;
        private GameObject target;
        private bool ready = false;
        public float speed = 2f;

        // Use this for initialization
        void Start()
        {
            StartCoroutine(initChaser(2f));

        }

        public IEnumerator initChaser(float time)
        {
            yield return new WaitForSeconds(time);
            players = GameObject.FindGameObjectsWithTag("Player");
            //UnityEngine.Random.InitState(System.Environment.TickCount);
            target = players[UnityEngine.Random.Range(0, players.Length)];
            ready = true;
        }


        public override void MoveEntity()
        {
            if (ready) {
                transform.localPosition = Vector3.Lerp(transform.localPosition, target.transform.position, speed  * Time.deltaTime);
            }
                
        }
    }

}
