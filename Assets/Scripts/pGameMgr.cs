using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class pGameMgr : NetworkBehaviour
{
    public bool gameover = false;
    private int totinfected = 0, numplayers = 0;
    public GameObject endscr;
    private bool hello = true;

    private GameObject pl;

    public bool revenge = false;

    public IEnumerator delayStart(float time)
    {
        yield return new WaitForSeconds(time);
        hello = false;
    }

    void LateUpdate () {
        if (hello) {
            StartCoroutine(delayStart(2)); return;
        }
        totinfected = 0;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        numplayers = players.Length;
        foreach (GameObject p in players) {
            pPlayer scr = p.GetComponent<pPlayer>();
            
            if (!scr.infected && !scr.gaterch)
            {
                gameover = false;
                break;
            }

            

            if(scr.infected) totinfected++;
            gameover = true; 

        }
        
        if (gameover == true) {
            if (numplayers == totinfected) { revenge = true; }
            foreach (GameObject p in players)
            {
                p.GetComponent<pPlayerMove>().go = true;
                
            }

            endscr.SetActive(true);
            Transform endtxt = endscr.transform.GetChild(0);
            if (revenge) endtxt.GetComponent<Text>().text = "Revenge Success!";
            else endtxt.GetComponent<Text>().text = "Revenge Failed!";
            
            
            
            
        }
		
	}
}
