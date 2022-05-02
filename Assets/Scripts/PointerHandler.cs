using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.Extras;
using Valve.VR.InteractionSystem;

public class PointerHandler : MonoBehaviour
{
    public GameObject rightHandGameobject;
    public SteamVR_LaserPointer laserPointer;
    public Color originalLaserColor;
    public GameObject activatedWorld;

    private LineRenderer l;

    private void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;

        activatedWorld = null;

        originalLaserColor = laserPointer.color;
        Material lineRenderedMaterial = new Material(Shader.Find("Unlit/Color"));
        lineRenderedMaterial.SetColor("_Color", originalLaserColor);

        l = gameObject.AddComponent<LineRenderer>();
        l.material = lineRenderedMaterial;
        l.startColor = Color.clear;
        l.endColor = Color.clear;
        l.startWidth = 0;
        l.endWidth = 0;

        laserPointer.color = Color.clear;
        laserPointer.thickness = 0;
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        }
    }

    private void PointerClick(object sender, PointerEventArgs e)
    {
        Animator a = e.target.gameObject.GetComponent<Animator>();
        if (e.target.GetComponent<Button>() != null)
        {
            e.target.GetComponent<Button>().onClick.Invoke();
        }
    }

    private void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.tag != "Plane")
        {
            laserPointer.color = originalLaserColor;
            laserPointer.thickness = 0.002f;
        }
        else
        {
            laserPointer.color = Color.clear;
            laserPointer.thickness = 0;
        }
    }

    private void PointerOutside(object sender, PointerEventArgs e)
    {

        laserPointer.color = Color.clear;
        laserPointer.thickness = 0;
    }


    private void SnapPointerToButton(GameObject g)
    {
        laserPointer.color = Color.clear;
        laserPointer.thickness = 0;

        List<Vector3> pos = new List<Vector3>();
        pos.Add(rightHandGameobject.transform.position);
        pos.Add(g.transform.position);
        l.startWidth = 0.004f;
        l.endWidth = 0.004f;
        l.startColor = originalLaserColor;
        l.endColor = originalLaserColor;
        l.SetPositions(pos.ToArray());
    }

    private void UnsnapPointerToButton()
    {
        laserPointer.color = originalLaserColor;
        laserPointer.thickness = 0.002f;

        l.startColor = Color.clear;
        l.endColor = Color.clear;
        l.startWidth = 0;
        l.endWidth = 0;
    }
}
