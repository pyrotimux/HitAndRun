using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class pLootChest : NetworkBehaviour {
    private bool inzone = false;
    private int defaultvar = 11;
    private Light light;

    [SyncVar]
    public int countdown = 11;

    private void Start()
    {
        light = GetComponentInChildren<Light>();
        light.enabled = false;
    }

    void OpenChest() {
        if (countdown > 0)
        {
            countdown--;
            GetComponentInChildren<TextMesh>().text = "" + countdown;
        }
        else
        {
            GetComponentInChildren<TextMesh>().text = "";
            GetComponentInChildren<Light>().enabled = false;
            Destroy(gameObject.GetComponent<pLootChest>());

            gameObject.transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
        }

        
    }

    private void OnTriggerEnter(Collider o)
    {
        pPlayer p = o.gameObject.GetComponent<pPlayer>();
        if (o.name.StartsWith("player") && !p.infected){
            inzone = true;
            InvokeRepeating("OpenChest", 0.0f, 2.0f);
            countdown = defaultvar;
            light.enabled = true;
        }
        

    }

    private void OnTriggerExit(Collider o)
    {
        inzone = false;
        GetComponentInChildren<TextMesh>().text = "";
        countdown = defaultvar;
        CancelInvoke();
        light.enabled = false;
    }
}
