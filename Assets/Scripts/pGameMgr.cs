using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class pGameMgr : NetworkBehaviour
{
    public static bool win = false;
    public bool gameover = false;
    private int totinfected = 0, numplayers = 0;
	
	void LateUpdate () {
        totinfected = 0;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        numplayers = players.Length;
        foreach (GameObject p in players) {
            pPlayer scr = p.GetComponent<pPlayer>();

            if (!scr.infected && !scr.gatereached)
            {
                gameover = false;
                break;
            }
            totinfected++;
            gameover = true; 

        }

        if (gameover == true) {
            if (numplayers / 2 < totinfected) { win = true; }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
		
	}
}
