using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class pDoor : NetworkBehaviour
{
    [SyncVar]
    public bool dooropen = true;

    private float openAng = -90f;
    public float speed = 10f;
    public bool rotatingDoor;
    private GameObject doorcont, doormesh;



    // Use this for initialization
    void Start()
    {
        doorcont = transform.GetChild(0).gameObject;
        doormesh = transform.GetChild(0).GetChild(0).gameObject;

    }



    private void OpenerRotate(bool open)
    {
        if (open)
        {
            doorcont.transform.localRotation = Quaternion.Slerp(doorcont.transform.localRotation, Quaternion.Euler(0, openAng, 0), speed * Time.deltaTime);
        }
        else
        {
            doorcont.transform.localRotation = Quaternion.Slerp(doorcont.transform.localRotation, Quaternion.Euler(0, 0, 0), speed * Time.deltaTime);
        }
    }

    private void OpenerSideWay(bool open)
    {
        if (open)
        {
            doormesh.transform.localPosition = Vector3.Lerp(doormesh.transform.localPosition, new Vector3(-1.67f, 0.35f, 0.50f), speed * Time.deltaTime);
        }
        else
        {
            doormesh.transform.localPosition = Vector3.Lerp(doormesh.transform.localPosition, new Vector3(1.61f, 0.35f, 0.50f), speed * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (inzone && Input.GetButtonDown("Interact"))
        //    CmdChangeDoorStatus();


        if (rotatingDoor)
        {
            OpenerRotate(dooropen);
        }
        else
        {
            OpenerSideWay(dooropen);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //GetComponentInChildren<TextMesh>().text = "Interact with this door.";
        dooropen = true;
    }

    private void OnTriggerExit(Collider o)
    {
        //GetComponentInChildren<TextMesh>().text = "";
        dooropen = false;
    }

    public IEnumerator closeDoor(float time)
    {
        yield return new WaitForSeconds(time);
        dooropen = false;
    }
    
    public void ChangeDoorStatus()
    {
        dooropen = !dooropen;
    }
    

}
