using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFunction : MonoBehaviour {

    GameObject gomain, godirect, gomatch, goback;

    public void InitButton()
    {
        gomain = GameObject.Find("MainPanel");
        godirect = gomain.transform.GetChild(2).gameObject;
        gomatch = gomain.transform.GetChild(1).gameObject;
        goback = gomain.transform.GetChild(0).GetChild(3).gameObject;
    }

    public void Button_Quit()
    {
        Application.Quit();
    }

    public void Button_Show_DirectPlay() {
        InitButton();
        godirect.SetActive(true);
        goback.SetActive(true);
    }

    public void Button_Show_MatchMake()
    {
        InitButton();
        gomatch.SetActive(true);
        goback.SetActive(true);
    }

    public void Button_Show_Back()
    {
        InitButton();
        godirect.SetActive(false);
        gomatch.SetActive(false);
        goback.SetActive(false);
    }
}
