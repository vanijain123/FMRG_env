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

    private void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;
    }

    private void PointerClick(object sender, PointerEventArgs e)
    {
        Animator a = e.target.gameObject.GetComponent<Animator>();
        if (e.target.tag == "redPlaneButton")
        {
            Debug.Log("Red button clicked");
            StartCoroutine(ButtonPressed(a, redLocation));
        }
        else if (e.target.tag == "yellowPlaneButton")
        {
            Debug.Log("Yellow Button Clicked");
            StartCoroutine(ButtonPressed(a, yellowLocation));
        }
        else if (e.target.tag == "greenPlaneButton")
        {
            Debug.Log("Green Button Clicked");
            StartCoroutine(ButtonPressed(a, greenLocation));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject c = collision.gameObject;
        if (c.tag == "redPlaneButton")
        {
            Debug.Log("Red button touched");
            ChangePosition(redLocation);
        }
    }

    private void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.tag == "redPlaneButton")
        {
            Debug.Log("Red button entered");
            ProjectOnPlane(e.target.gameObject);
        }
        else if (e.target.tag == "yellowPlaneButton")
        {
            Debug.Log("Yellow Button entered");
            ProjectOnPlane(e.target.gameObject);
        }
        else if (e.target.tag == "greenPlaneButton")
        {
            Debug.Log("Green Button entered");
            ProjectOnPlane(e.target.gameObject);
        }
    }

    private void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.tag == "redPlaneButton")
        {
            Debug.Log("Red button exited");
            projectionPlane.SetActive(false);
            lr.enabled = false;
        }
        else if (e.target.tag == "yellowPlaneButton")
        {
            Debug.Log("Yellow Button exited");
            projectionPlane.SetActive(false);
        }
        else if (e.target.tag == "greenPlaneButton")
        {
            Debug.Log("Green Button exited");
            projectionPlane.SetActive(false);
        }
    }

    private void ChangePosition(Transform pos)
    {
        transform.position = pos.position;
        ft.FadeMethod();
    }

    IEnumerator ButtonPressed(Animator a, Transform pos)
    {
        a.SetTrigger("ButtonPressed");
        yield return new WaitForSeconds(0.8f);
        ChangePosition(pos);
    }

    private void ProjectOnPlane(GameObject button)
    {
        // Get projection plane gameobject from the button pressed
        projectionPlane = button.transform.parent.GetChild(3).gameObject;
        Debug.Log(projectionPlane.name);
        projectionPlane.SetActive(true);

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
}
