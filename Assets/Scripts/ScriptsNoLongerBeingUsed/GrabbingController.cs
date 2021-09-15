using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GrabbingController : MonoBehaviour
{
    public Transform attachmentPoint;

    private SteamVR_Behaviour_Pose bp;

    private bool grabbing;
    private GameObject grabbedObject;


    private void Start()
    {
        grabbing = false;
        
    }

    private void Update()
    {
        if (grabbing == true)
        {
            Debug.Log("Grab update");
            Grab(grabbedObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        grabbedObject = collider.gameObject;
        
        Debug.Log("OnTriggerEnter || " + grabbedObject.layer.ToString());
        if (grabbedObject.layer == 8)
        {
            Debug.Log("Entered " + grabbedObject.name);
            //Grab(c);
            grabbing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("On Trigger Exit");
        grabbing = false;
    }

    private void Grab(GameObject g)
    {

        Debug.Log("Grabbing");
        g.transform.position = attachmentPoint.position;
    }

    

}
