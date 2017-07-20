using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class pPlayer : NetworkBehaviour {

	[SyncVar]
	public string pname = "player";

    [SyncVar]
    public Color pcolor = Color.red;

    [SyncVar]
    public bool haskey = false, infected = false, gatereached = false;

    public Material pmatblue, pmatred, pmatblack, pmatyellow, pmatgreen, pmat;

    public GameObject spawner;

    private pPlayerMove playermoves;

    // Use this for initialization
    void Start () {
		if (isLocalPlayer) {
			playermoves = GetComponent<pPlayerMove>();
            playermoves.enabled = true;

			Camera.main.transform.position = this.transform.position - this.transform.forward * 5 + this.transform.up * 2;
			Camera.main.transform.LookAt (this.transform.position);
			Camera.main.transform.parent = this.transform;
		}

        GetComponentInChildren<TextMesh>().text = pname;


        if (pcolor == Color.blue)
            pmat = pmatblue;
        else if (pcolor == Color.green)
            pmat = pmatgreen;
        else if (pcolor == Color.yellow)
            pmat = pmatyellow;
        else if (pcolor == Color.red)
            pmat = pmatred;
        else if (pcolor == Color.black)
            pmat = pmatblack;
        else
            pmat = pmatred;

        
        transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = pmat;

        

        //Renderer[] rends = GetComponentsInChildren<Renderer>();
        //foreach (Renderer r in rends)
        //    r.material.color = pcolor;
    }
    
    
    void OnCollisionEnter(Collision col)
    {
        string s = col.gameObject.name;

        if (s.StartsWith("player") && infected) {
            pPlayer p = col.gameObject.GetComponent<pPlayer>();
            p.infected = true;
        }
        else if (s.StartsWith("key") && !infected)
        {
            NetworkServer.Destroy(col.gameObject);
            haskey = true;
        }
        else if (s.StartsWith("infections"))
        {
            NetworkServer.Destroy(col.gameObject);
            infected = true;
        }
        else if (s.StartsWith("gatebarrier") && haskey)
        {
            NetworkServer.Destroy(col.gameObject);
        }
        else if (s.StartsWith("wingate") && haskey)
        {
            gatereached = true;
            pGameMgr.win = true;

            if(isLocalPlayer){ playermoves.enabled = false; }
            transform.GetComponent<Renderer>().enabled = false;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }

        //StartCoroutine(checkStatus(0.3f));

    }

    private void LateUpdate()
    {
        checkStatus();
    }

    //private IEnumerator checkStatus(float  time)
    public void checkStatus()
    {
        //yield return new WaitForSeconds(time);

        if (infected)
        {
            if (haskey) {
                haskey = false;
                pSpawner pspwn = GameObject.Find("SpawnManager").GetComponent<pSpawner>();
                pspwn.SpawnKey(transform);
            }
            Transform o = transform.GetChild(0).GetChild(6);
            o.GetComponent<MeshRenderer>().enabled = true;
            o.GetComponent<Renderer>().material.color = Color.magenta;
            playermoves.speed = 20;
            playermoves.rotateSpeed = 70;

        } else if (!infected && haskey) {
            Transform o = transform.GetChild(0).GetChild(6);
            o.GetComponent<MeshRenderer>().enabled = true;
            o.GetComponent<Renderer>().material.color = Color.green;
        }
            
    }


}
