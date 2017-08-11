using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFunction : MonoBehaviour {

    GameObject godirect;

    public void Start()
    {
        godirect = GameObject.Find("MainPanel").transform.GetChild(2).gameObject;
    }

    public void Button_Quit()
    {
        Application.Quit();
    }

    public void Button_Show_DirectPlay() {
        
        godirect.SetActive(true);
    }

    public void Button_Show_Back()
    {
       
    }
}
