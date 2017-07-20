using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class pSpawner : NetworkBehaviour
{
    public Transform infspwn, spawn1, spawn2, spawn3, spawn4, lkeypos;
    public GameObject keyprefab, infectprefab;

    // Use this for initialization
    void Start () {
        CmdSpawn();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnKey(Transform p)
    {
        lkeypos = p;
        CmdKeySpwn();
    }


    [Command]
    void CmdKeySpwn()
    {
        GameObject key = (GameObject)Instantiate(keyprefab, new Vector3(lkeypos.position.x, 10, lkeypos.position.z), Quaternion.identity);
        NetworkServer.Spawn(key);
    }

    [Command]
    void CmdSpawn()
    {
        GameObject key = (GameObject)Instantiate(keyprefab, spawn1.position, Quaternion.identity);
        NetworkServer.Spawn(key);

//        GameObject infections = (GameObject)Instantiate(infectprefab, infspwn.position, Quaternion.identity);
//        NetworkServer.Spawn(infections);

        //GameObject key2 = (GameObject)Instantiate(keyprefab, spawn2.position, Quaternion.identity);
        //NetworkServer.Spawn(key2);
    }
}
