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

    public Transform attachmentPoint;

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

        clicked = false;
    }

    private void PointerClick(object sender, PointerEventArgs e)
    {
        Animator a = e.target.gameObject.GetComponent<Animator>();
        if (e.target.tag == "redPlaneButton" && clicked == false)
        {
            Debug.Log("Red button clicked");
            ButtonPressed(a);
            clicked = true;
            clickedGameObject = e.target.gameObject;
            DestroyReplicas();
            ProjectOnPlane(e.target.gameObject);
        }
        else if (e.target.tag == "yellowPlaneButton" && clicked == false)
        {
            Debug.Log("Yellow Button Clicked");
            ButtonPressed(a);
            clicked = true;
            clickedGameObject = e.target.gameObject;
            DestroyReplicas();
            ProjectOnPlane(e.target.gameObject);
        }
        else if (e.target.tag == "greenPlaneButton" && clicked == false)
        {
            Debug.Log("Green Button Clicked");
            ButtonPressed(a);
            clicked = true;
            clickedGameObject = e.target.gameObject;
            DestroyReplicas();
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
        else if(e.target.name == "ProjectionPlane" || e.target.name == "InformationProjectionPlane")
        {
            FlipPlanes(e.target.gameObject);
        }
        //else if (e.target.name == "InformationProjectionPlane")
        //{
        //    pp.SetTrigger("FlipBackPlane");
        //    ip.SetTrigger("FlipBackPlane");
        //}
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    // temp
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    // temp
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    GameObject c = collision.gameObject;
    //    Debug.Log("OnTriggerStay");
    //    if (c.layer.ToString() == "Grabbable")
    //    {
    //        Debug.Log("Entered " + c.name);
    //        Grab(c);
    //    }
    //}

    private void Grab(GameObject g)
    {
        g.transform.position = attachmentPoint.position;
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

            //if (tr.tag == "projectionPlane")
            //{
            //    projectionPlane = tr.gameObject;
            //}

            if (tr.name == "ProjectionPlane")
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

        //if (clicked == true)
        //{
            CreateMiniWorld(projectionPlane, button);
        //}
        

        // Change material of projection plane
        Debug.Log("Inside ProjectOnPlane");

        // Animator pp = projectionPlane.GetComponent<Animator>();
        // pp.enabled = true;

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
        Vector3 start_pos = new Vector3(button.transform.position.x, button.transform.position.y + 5, button.transform.position.z);
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

        // Destroy gameobjects and empty replicas collection
        DestroyReplicas();

        projectedComponents.SetActive(false);
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

        GameObject replicatePlane = surface;

        if (button.tag == "redPlaneButton")
        {
            replicatePlane = redPlane;
        }
        else if (button.tag == "greenPlaneButton")
        {
            replicatePlane = greenPlane;
        }
        else if (button.tag == "yellowPlaneButton")
        {
            replicatePlane = yellowPlane;
        }

        //GameObject[] replicas;

        foreach (Transform tr in replicatePlane.transform)
        {
            if (tr.tag == "replicable")
            {
                //replicas.Add(Instantiate(tr.gameObject, surface.transform, true) as GameObject);
                //replicas.Add(Instantiate(tr.gameObject) as GameObject);
                replicas.Add(Instantiate(tr.gameObject, surface.transform) as GameObject);

                // scale = tr.localScale;
            }
        }
        
        foreach (GameObject x in replicas)
        {
            //x.transform.SetParent(surface.transform, true);
            //    //x.transform.localScale = x.transform.localScale / surface.transform.localScale;
            //x.transform.localPosition = surface.transform.position;

            x.transform.SetParent(null);
            x.transform.localScale = scale;
            //x.transform.localScale = scale;

            x.transform.SetParent(surface.transform);
            x.transform.localScale /= 10;
            //x.transform.localScale = newLocalScale;
            x.layer = 8;

            x.AddComponent<Interactable>();
            x.AddComponent<InteractableHoverEvents>();
            x.AddComponent<SimpleAttach>();
        }
    }
}
