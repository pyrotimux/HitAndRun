using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class pSpawner : NetworkBehaviour
{
    public Transform spawn1, spawn2, spawn3, spawn4, spawn5, spawn6;
    public GameObject keyprefab;

    // Use this for initialization
    void Start () {
        CmdSpawn();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    [Command]
    void CmdSpawn()
    {
        GameObject key = (GameObject)Instantiate(keyprefab, new Vector3(110.5f, 0.4f, 49.2f), Quaternion.identity);
        NetworkServer.Spawn(key);
    }
}
