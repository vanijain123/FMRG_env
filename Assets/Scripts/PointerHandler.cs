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

    public SteamVR_Fade fade;
    public FadeTest ft;
    public SteamVR_LaserPointer laserPointer;
    public Transform redLocation;
    public Transform yellowLocation;
    public Transform greenLocation;

    // private SteamVR_Fade fade;

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
        }
        else if (e.target.tag == "yellowPlaneButton")
        {
            Debug.Log("Yellow Button entered");
        }
        else if (e.target.tag == "greenPlaneButton")
        {
            Debug.Log("Green Button entered");
        }
    }

    private void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.tag == "redPlaneButton")
        {
            Debug.Log("Red button exited");
        }
        else if (e.target.tag == "yellowPlaneButton")
        {
            Debug.Log("Yellow Button exited");
        }
        else if (e.target.tag == "greenPlaneButton")
        {
            Debug.Log("Green Button exited");
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
}
