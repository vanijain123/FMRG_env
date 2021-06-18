using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.Extras;

public class PointerHandler : MonoBehaviour
{
    public Animator redButton;
    public LineRenderer lr;

    public Material redRenderTexture;
    public Material greenRenderTexture;
    public Material yellowRenderTexture;

    public SteamVR_Fade fade;
    public FadeTest ft;
    public SteamVR_LaserPointer laserPointer;
    public Transform redLocation;
    public Transform yellowLocation;
    public Transform greenLocation;

    // private SteamVR_Fade fade;
    private GameObject projectionPlane;
    private GameObject projectedComponents;
    private bool clicked;
    private GameObject clickedGameObject;

    private void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;

        clicked = false;
    }

    private void PointerClick(object sender, PointerEventArgs e)
    {
        Animator a = e.target.gameObject.GetComponent<Animator>();
        if (e.target.tag == "redPlaneButton")
        {
            Debug.Log("Red button clicked");
            ButtonPressed(a);
            clicked = true;
            clickedGameObject = e.target.gameObject;
            ProjectOnPlane(e.target.gameObject);
        }
        else if (e.target.tag == "yellowPlaneButton")
        {
            Debug.Log("Yellow Button Clicked");
            ButtonPressed(a);
            clicked = true;
            clickedGameObject = e.target.gameObject;
            ProjectOnPlane(e.target.gameObject);
        }
        else if (e.target.tag == "greenPlaneButton")
        {
            Debug.Log("Green Button Clicked");
            ButtonPressed(a);
            clicked = true;
            clickedGameObject = e.target.gameObject;
            ProjectOnPlane(e.target.gameObject);
        }
        else if (e.target.name == "CloseMenuButton_Button")
        {
            HideProjectedComponents();
        }
        else if (e.target.name == "TeleportButton_Button")
        {
            ChangePosition();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject c = collision.gameObject;
        if (c.tag == "redPlaneButton")
        {
            Debug.Log("Red button touched");
            ChangePosition();
        }
    }

    private void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.tag == "redPlaneButton")
        {
            Debug.Log("Red button entered");
            if (clicked == false)
            {
                ProjectOnPlane(e.target.gameObject);
            }
           
        }
        else if (e.target.tag == "yellowPlaneButton")
        {
            Debug.Log("Yellow Button entered");
            if (clicked == false)
            {
                ProjectOnPlane(e.target.gameObject);
            }
        }
        else if (e.target.tag == "greenPlaneButton")
        {
            Debug.Log("Green Button entered");
            if (clicked == false)
            {
                ProjectOnPlane(e.target.gameObject);
            }
        }
    }

    private void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.tag == "redPlaneButton")
        {
            Debug.Log("Red button exited");
            if (clicked == false)
            {
                HideProjectedComponents();
            }
        }
        else if (e.target.tag == "yellowPlaneButton")
        {
            Debug.Log("Yellow Button exited");
            if (clicked == false)
            {
                HideProjectedComponents();
            }
        }
        else if (e.target.tag == "greenPlaneButton")
        {
            Debug.Log("Green Button exited");
            if (clicked == false)
            {
                HideProjectedComponents();
            }
        }
        
    }

    private void ChangePosition()
    {
        Vector3 dest = new Vector3(0,0,0);
        if (clickedGameObject.tag == "redPlaneButton")
        {
            dest = redLocation.position;
        }
        else if (clickedGameObject.tag == "yellowPlaneButton")
        {
            dest = yellowLocation.position;
        }
        else if (clickedGameObject.tag == "greenPlaneButton")
        {
            dest = greenLocation.position;
        }
        transform.position = dest;
        ft.FadeMethod();
        HideProjectedComponents();
    }

    private void ButtonPressed(Animator a)
    {
        a.SetTrigger("ButtonPressed");
        // yield return new WaitForSeconds(0.8f);
        //ChangePosition(pos);
    }

    private void ProjectOnPlane(GameObject button)
    {
        // Get projection plane gameobject from the button pressed
        Transform t = button.transform.parent.transform;
        foreach(Transform tr in t)
        {
            if (tr.name == "ProjectedComponents")
            {
                projectedComponents = tr.gameObject;
            }
        }
        projectedComponents.SetActive(true);
        

        foreach (Transform tr in projectedComponents.transform)
        {

            if (tr.tag == "projectionPlane")
            {
                projectionPlane = tr.gameObject;
            }

            if (clicked == true)
            {
                if (tr.name == "TeleportButton" || tr.name == "CloseMenuButton")
                {
                    tr.gameObject.SetActive(true);
                }
            }
        }
        
        // Change material of projection plane
        Debug.Log("Inside ProjectOnPlane");;

        if (button.tag == "redPlaneButton")
        {
            projectionPlane.GetComponent<MeshRenderer>().material = redRenderTexture;
        }
        else if (button.tag == "greenPlaneButton")
        {
            projectionPlane.GetComponent<MeshRenderer>().material = greenRenderTexture;
        }
        else if (button.tag == "yellowPlaneButton")
        {
            projectionPlane.GetComponent<MeshRenderer>().material = yellowRenderTexture;
        }
        Debug.Log(projectionPlane.GetComponent<MeshRenderer>().materials[0]);

        // Draw line from button to preview
        lr.enabled = true;
        lr.SetPosition(0, button.transform.position);
        lr.startWidth = 0.005f;
        lr.endWidth = 0.005f;
        Vector3 dest = new Vector3(button.transform.position.x + 1, button.transform.position.y + 0.4f, button.transform.position.z);
        lr.SetPosition(1, dest);
    }

    private void HideProjectedComponents()
    {
        if (clicked == true)
        {
            foreach (Transform tr in projectedComponents.transform)
            {
                if (tr.name == "TeleportButton" || tr.name == "CloseMenuButton")
                {
                    tr.gameObject.SetActive(false);
                }
            }
        }
        projectedComponents.SetActive(false);
        lr.enabled = false;
        clicked = false;
    }
}
