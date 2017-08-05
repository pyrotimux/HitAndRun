using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class pPlayer : NetworkBehaviour {

	[SyncVar]
	public string pname = "player"; // value set on run time by lobby hook

    [SyncVar]
    public Color pcolor = Color.red; // value sset on run time by lobby hook

    [SyncVar]
    public bool haskey = false; // tells if the player has key to unlock gate or not

    [SyncVar]
    public bool infected = false; // tells infected state

    [SyncVar]
    public bool gaterch = false; // tells if gate was reached or not

    public Material pmatblue, pmatred, pmatblack, pmatyellow, pmatgreen, pmat; // materials used to customize player
    
    private pPlayerMove playermoves; // define how player is moved 

    

    // Use this for initialization
    void Start () {
        
        if (isLocalPlayer) {
            // we will enable the local player movement but not network.
			playermoves = GetComponent<pPlayerMove>();
            playermoves.enabled = true;

            // this makes the camera offset and then follow the player by attaching to it.
            Camera.main.transform.position = this.transform.position - this.transform.forward * 5 + this.transform.up * 1;
			Camera.main.transform.LookAt (this.transform.position);
            Camera.main.transform.position = this.transform.position - this.transform.forward * 5 + this.transform.up * 3;
            Camera.main.transform.parent = this.transform;
		}

        // Set the name and color of the player depending on what was chosen in the lobby.
        
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

        GetComponentInChildren<TextMesh>().text = pname;
        GetComponentInChildren<TextMesh>().color = pcolor;

        transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = pmat;

        
        // we can use this to make player invisible by disabling the rendering engine.
        //Renderer[] rends = GetComponentsInChildren<Renderer>();
        //foreach (Renderer r in rends)
        //    r.material.color = pcolor;
    }

    // handles all player collision in this class
    void OnCollisionEnter(Collision col)
    {
        string s = col.gameObject.name; // name of the collider 


        if (s.StartsWith("player") && infected)
        { // we are infecting other players 
            pPlayer p = col.gameObject.GetComponent<pPlayer>();
            p.infected = true;
            p.CmdPlayerInfected();
        }
        else if (s.StartsWith("key") && !infected)
        { // we can pick up the key as long as we are not infected.
            NetworkServer.Destroy(col.gameObject);
            CmdPlayerGotKey();
            
        }
        else if (s.StartsWith("infections"))
        { // we got infected from world item
            NetworkServer.Destroy(col.gameObject);
            CmdPlayerInfected();
        }
        else if (s.StartsWith("gatebarrier") && haskey)
        { // we are destroying gatebarrier as long as we have the key
            NetworkServer.Destroy(col.gameObject);
        }
        else if (s.StartsWith("wingate") && !infected)
        { // we will enter win gate as long as we are not infected and win the game.
            if(isLocalPlayer){ // we disabled the local player movement and display win screen
                playermoves.go = true;
                //StartCoroutine(winscreen(1));
                winscreen();
            }
            
            // we then update the sync var.
            CmdPlayerReachGate();

            StartCoroutine(moveLvl(2));
            

        }

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
            Transform o = transform.GetChild(2);
            o.GetComponent<MeshRenderer>().enabled = true;
            o.GetComponent<Renderer>().material.color = Color.magenta;

            // turn to bad guy.
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(true);

        } else if (!infected && haskey) {
            Transform o = transform.GetChild(2);
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
        transform.rotation = t1.transform.rotation;
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
