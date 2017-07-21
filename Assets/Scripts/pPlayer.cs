using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class pPlayer : NetworkBehaviour {

	[SyncVar]
	public string pname = "player";

    [SyncVar]
    public Color pcolor = Color.red;

    [SyncVar]
    public bool haskey = false;

    [SyncVar]
    public bool infected = false;

    [SyncVar]
    public bool gaterch = false;

    public Material pmatblue, pmatred, pmatblack, pmatyellow, pmatgreen, pmat;
    
    private pPlayerMove playermoves;

    

    // Use this for initialization
    void Start () {
        
        if (isLocalPlayer) {
			playermoves = GetComponent<pPlayerMove>();
            playermoves.enabled = true;

            Camera.main.transform.position = this.transform.position - this.transform.forward * 5 + this.transform.up * 1;
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
            p.CmdPlayerInfected();
        }
        else if (s.StartsWith("key") && !infected)
        {
            NetworkServer.Destroy(col.gameObject);
            CmdPlayerGotKey();
            
        }
        else if (s.StartsWith("infections"))
        {
            NetworkServer.Destroy(col.gameObject);
            CmdPlayerInfected();
        }
        else if (s.StartsWith("gatebarrier") && haskey)
        {
            NetworkServer.Destroy(col.gameObject);
        }
        else if (s.StartsWith("wingate") && !infected)
        {
            if(isLocalPlayer){
                playermoves.go = true;
                //StartCoroutine(winscreen(1));
                winscreen();
            }

            

            CmdPlayerReachGate();

            StartCoroutine(moveLvl(2));

            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }

        //StartCoroutine(checkStatus(0.3f));

    }

    private void LateUpdate()
    {
        checkStatus();
    }
    


    public void checkStatus()
    {
        //yield return new WaitForSeconds(time);

        if (infected)
        {
            if (haskey) {
                haskey = false;
                CmdPlayerDropKey();
                pSpawner pspwn = GameObject.Find("SpawnManager").GetComponent<pSpawner>();
                pspwn.SpawnKey(transform);
            }
            Transform o = transform.GetChild(0).GetChild(6);
            o.GetComponent<MeshRenderer>().enabled = true;
            o.GetComponent<Renderer>().material.color = Color.magenta;

        } else if (!infected && haskey) {
            Transform o = transform.GetChild(0).GetChild(6);
            o.GetComponent<MeshRenderer>().enabled = true;
            o.GetComponent<Renderer>().material.color = Color.green;
        }
            
    }


    [Server]
    void CmdPlayerGotKey()
    {
       haskey = true;
    }

    [Server]
    void CmdPlayerDropKey()
    {
        haskey = false;
    }

    [Server]
    void CmdPlayerInfected() {
        infected = true;
    }

    [Server]
    void CmdPlayerReachGate()
    {
        gaterch = true;
    }

    public IEnumerator moveLvl(float time)
    {
        yield return new WaitForSeconds(time);
        GameObject t1 = GameObject.Find("t1");
        transform.position = t1.transform.position;
    }

    //public IEnumerator winscreen(float time)
    public void winscreen()
    {
        //yield return new WaitForSeconds(time);
        GameObject canvas = GameObject.Find("Canvas");
        GameObject endscr = canvas.transform.GetChild(0).gameObject;
        endscr.SetActive(true);
        Transform endtxt = endscr.transform.GetChild(0);
        if (gaterch) endtxt.GetComponent<Text>().text = "You Win!";
    }

}
