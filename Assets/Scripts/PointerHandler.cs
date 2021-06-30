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

    public GameObject redPlane;
    public GameObject yellowPlane;
    public GameObject greenPlane;

    public GameObject currentPlane;
    public GameObject nextPlane;

    public Transform attachmentPoint;

    public GameObject VRCamera;

    private List<GameObject> replicas = new List<GameObject>();

    // private SteamVR_Fade fade;
    private GameObject projectionPlane;
    private GameObject projectedComponents;
    private bool clicked;
    private GameObject clickedGameObject;

    public Animator pp;
    public Animator ip;

    private void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;

        currentPlane = greenPlane;

        clicked = false;
    }

    private void PointerClick(object sender, PointerEventArgs e)
    {
        Animator a = e.target.gameObject.GetComponent<Animator>();

        //if (e.target.tag == "planeButton" && clicked == false)
        if (e.target.tag == "planeButton")
        {
            nextPlane = e.target.GetComponent<AttachedPlane>().plane;
            Debug.Log(currentPlane.name + " clicked");

            ButtonPressed(a);
            //clicked = true;
            clickedGameObject = e.target.gameObject;
            DestroyReplicas();
            ProjectOnPlane(e.target.gameObject);
        }

        //else if (e.target.name == "CloseMenuButton_Button")
        //{
        //    HideProjectedComponents();
        //}
        if (e.target.name == "TeleportButton_Button")
        {
            //currentPlane.SetActive(false);
            ChangePosition();
        }
        else if (e.target.name == "DuplicatetButton_Button")
        {
            //currentPlane.SetActive(false);
            Debug.Log("Duplicate button clicked" + e.target.name);
            DuplicateMenu(e.target.transform.parent.transform.parent.transform.parent.gameObject);
        }
        //else if(e.target.name == "ProjectionPlane" || e.target.name == "InformationProjectionPlane")
        //{
        //    FlipPlanes(e.target.gameObject);
        //}
    }

    private void DuplicateMenu(GameObject menu)
    {
        Debug.Log("Inside Duplicate Menu");
        Quaternion q = menu.transform.rotation;
        Vector3 pos = new Vector3(menu.transform.position.x - 0.8f, menu.transform.position.y, menu.transform.position.z);
        Instantiate(menu, pos, q, menu.transform.parent.transform);
    }

    private void Grab(GameObject g)
    {
        g.transform.position = attachmentPoint.position;
    }

    private void PointerInside(object sender, PointerEventArgs e)
    {

        if (e.target.tag == "planeButton")
        {

            Debug.Log(nextPlane + " button entered");
            //HideProjectedComponents();
            ////if (clicked == false)
            ////{
            //    ProjectOnPlane(e.target.gameObject);
            ////}
        }
    }

    private void PointerOutside(object sender, PointerEventArgs e)
    {

        if (e.target.tag == "planeButton")
        {
            Debug.Log(nextPlane + " button exited");
            //if (clicked == false)
            //{
            //    HideProjectedComponents();
            //}
        }

    }

    private void ChangePosition()
    {
        Vector3 dest = new Vector3(0,0,0);

        Debug.Log("Before teleport: Current plane - " + currentPlane + " Next plane - " + nextPlane);

        //currentPlane.SetActive(false);

        //nextPlane.SetActive(true);
        transform.position = nextPlane.transform.position;
        //currentPlane = nextPlane;

        //currentPlane.layer = 9;
        //SetLayerRecursively(currentPlane, 9);

        //nextPlane.layer = 0;
        //SetLayerRecursively(nextPlane, 0);

        currentPlane = nextPlane.gameObject;

        Debug.Log("After teleport: Current plane - " + currentPlane + " Next plane - " + nextPlane);

        ft.FadeMethod();
        HideProjectedComponents();
    }

    private void SetLayerRecursively(GameObject g, int layer)
    {
        g.layer = layer;
        foreach (Transform child in g.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    private void ButtonPressed(Animator a)
    {
        a.SetTrigger("ButtonPressed");
    }

    private void ProjectOnPlane(GameObject button)
    {
        // Get projection plane gameobject from the button pressed
        Transform t = button.transform.parent.transform.parent.transform;
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
            
            if (tr.name == "ProjectionPlane")
            {
                projectionPlane = tr.gameObject;
            }

            //if (clicked == true)
            //{
            //    if (tr.name == "TeleportButton" || tr.name == "CloseMenuButton")
            //    {
            //        tr.gameObject.SetActive(true);
            //    }
                
            //}
        }

        //if (clicked == true)
        //{
            CreateMiniWorld(projectionPlane, button);
        //}
        

        // Change material of projection plane
        Debug.Log("Inside ProjectOnPlane");

        // Animator pp = projectionPlane.GetComponent<Animator>();
        // pp.enabled = true;

        Material projectionTexture = button.GetComponent<AttachedPlane>().projectionTexture;
        projectionPlane.GetComponent<MeshRenderer>().material = projectionTexture ;
        
        Debug.Log(projectionPlane.GetComponent<MeshRenderer>().materials[0]);

        // Draw line from button to preview
        lr.enabled = true;
        Vector3 start_pos = new Vector3(button.transform.position.x, button.transform.position.y + 5, button.transform.position.z);
        lr.SetPosition(0, button.transform.position);
        lr.startWidth = 0.005f;
        lr.endWidth = 0.005f;
        Vector3 dest = new Vector3(button.transform.position.x + 1, button.transform.position.y + 0.4f, button.transform.position.z);
        lr.SetPosition(1, dest);
    }

    private void HideProjectedComponents()
    {

        //if (clicked == true)
        //{
        //    foreach (Transform tr in projectedComponents.transform)
        //    {
        //        if (tr.name == "TeleportButton" || tr.name == "CloseMenuButton")
        //        {
        //            tr.gameObject.SetActive(false);
        //        }
        //    }
        //}

        // Destroy gameobjects and empty replicas collection
        if (replicas.Count != 0)
        {
            DestroyReplicas();
        }
        

        //projectedComponents.SetActive(false);
        lr.enabled = false;
        clicked = false;
    }

    private void DestroyReplicas()
    {
        List<GameObject> temp = new List<GameObject>(replicas);
        int count = 0;
        foreach (GameObject x in temp)
        {
            Debug.Log(replicas[count]);
            Destroy(replicas[count]);
            replicas.Remove(replicas[count]);
        }
    }

    private void FlipPlanes(GameObject a)
    {
        //GameObject x, y;

        //Transform parent = a.transform.parent.transform;
        
        //foreach (Transform tr in parent)
        //{
        //    if (tr.name == "ProjectionPlane")
        //    {
        //        x = tr.gameObject;
        //        pp = x.GetComponent<Animator>();
        //    }
        //    if (tr.name == "InformationProjectionPlane")
        //    {
        //        y = tr.gameObject;
        //        ip = y.GetComponent<Animator>();
        //    }
        //}
        

        //if (a.name == "ProjectionPlane")
        //{
        //    pp.SetTrigger("FlipPlane");
        //    ip.SetTrigger("FlipPlane");
        //}
        //else
        //{
        //    pp.SetTrigger("FlipPlaneBack");
        //    ip.SetTrigger("FlipPlaneBack");
        //}
    }

    private void CreateMiniWorld(GameObject surface, GameObject button)
    {
        
        Vector3 scale = new Vector3 (1,1,1);
        Vector3 newLocalScale = new Vector3(0.1f, 0.1f, 0.1f);

        GameObject replicatePlane = button.GetComponent<AttachedPlane>().plane;

        foreach (Transform tr in replicatePlane.transform)
        {
            if (tr.tag == "replicable")
            {
                replicas.Add(Instantiate(tr.gameObject, surface.transform) as GameObject);
            }
        }
        
        foreach (GameObject x in replicas)
        {
            Debug.Log(x.name);
            //x.transform.SetParent(null);
            //x.transform.localScale = scale;

            x.transform.SetParent(surface.transform);
            //x.transform.localScale /= 10;
            //x.transform.localPosition /= 10;
            x.layer = 8;

            x.AddComponent<Interactable>();
            x.AddComponent<InteractableHoverEvents>();
            x.AddComponent<SimpleAttach>();
        }
    }
}
