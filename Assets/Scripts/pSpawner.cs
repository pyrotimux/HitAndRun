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
    void CmdInfectSpwn()
    {
        GameObject infection = (GameObject)Instantiate(infectprefab, new Vector3(infspwn.position.x, 10, infspwn.position.z), Quaternion.identity);
        NetworkServer.Spawn(infection);
    }

    [Command]
    void CmdSpawn()
    {
        lkeypos = spawn1;
        CmdKeySpwn();
        CmdInfectSpwn();
        
    }
}
