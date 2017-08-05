using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class pInfectionBehaviour : NetworkBehaviour
{

    private GameObject[] players;
    private GameObject target;
    private bool ready = false;
    public float speed = 16f;


	// Use this for initialization
	void Start () {
        StartCoroutine( initChaser(2f) );
        
    }

    public IEnumerator initChaser(float time)
    {
        yield return new WaitForSeconds(time);
        players = GameObject.FindGameObjectsWithTag("Player");
        target = players[Random.Range(0, players.Length )];
        ready = true;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if(ready)
            transform.localPosition = Vector3.Lerp(transform.localPosition, target.transform.position,  speed);
	}

   
}
