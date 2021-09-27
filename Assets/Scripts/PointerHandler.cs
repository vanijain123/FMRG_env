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
    public Hand rightHand;
    public Hand leftHand;
    public SteamVR_Input_Sources rightController;
    //public GameObject rightHand;

    public SteamVR_Fade fade;
    public FadeTest ft;
    public SteamVR_LaserPointer laserPointer;

    public Transform attachmentPoint;

    public GameObject VRCamera;

    private int insideMenuGrab;
    private Transform grabbingObject;
    private Transform activatedObject;
    private Vector3 leftHandPosition;
    private float handPosition;
    private float diff;

    private void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;

        insideMenuGrab = 0;
        activatedObject = null;
        leftHandPosition = leftHand.transform.position;
    }

    private void Update()
    {
        if (insideMenuGrab > 0)
        {
            if (SteamVR_Actions.default_GrabGrip[rightHand.handType].state)
            {
                grabbingObject.transform.position = attachmentPoint.position;
                grabbingObject.transform.rotation = attachmentPoint.rotation;
            }
        }

        if (activatedObject)
        {
            if (SteamVR_Actions.default_GrabGrip[rightHand.handType].state)
            {
                activatedObject.transform.position = attachmentPoint.position;
                activatedObject.transform.rotation = attachmentPoint.rotation;
            }
            if (SteamVR_Actions.default_GrabGrip[leftHand.handType].state)
            {
                float x = leftHandPosition.x - leftHand.transform.position.x;
                activatedObject.transform.localScale += new Vector3(x, x, x);
            }
        }
        leftHandPosition = leftHand.transform.position;
    }

    private void PointerClick(object sender, PointerEventArgs e)
    {
        Animator a = e.target.gameObject.GetComponent<Animator>();

        //if (e.target.tag == "site")
        //{
        //    e.target.Find("AddButton").GetComponent<AddingWorld>().AddWorld();
        //}

        if (e.target.name == "AddButton")
        {
            e.target.GetComponent<AddingWorld>().AddWorld();
        }

        else if (e.target.name == "DeleteButton")
        {
            e.target.GetComponent<DeletingWorld>().DeleteWorld();
        }

        else if(e.target.name == "ProjectedSiteDeleteButton")
        {
            e.target.GetComponent<ProjectedSiteDeleteButton>().DeleteProjectedSite();
        }

        else if(e.target.name == "ActivateTask" && e.target.tag == "deactivated")
        {
            e.target.GetComponent<ActivateTaskButton>().ActivateTask();
            activatedObject = e.target.transform.parent;
        }

        else if (e.target.name == "ActivateTask" && e.target.tag == "activated")
        {
            e.target.GetComponent<ActivateTaskButton>().DeactivateTask();
            activatedObject = null;
        }
        //if (e.target.tag == "siteTask")
        //{
        //    if (e.target.transform.childCount > 1)
        //    {   
        //        Destroy(e.target.transform.GetChild(1).gameObject);
        //    }
        //    e.target.transform.parent.transform.parent.GetChild(0).GetComponent<SiteManager>().SelectSite(e.target.GetSiblingIndex());
        //}

        //if (e.target.tag == "task")
        //{
        //    e.target.transform.parent.transform.parent.transform.parent.Find("TaskProjectionPlane").GetComponent<TaskProjectionPlane>().SetTask(e.target.gameObject);
        //}
    }

    private void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.name == "MenuBackPlane" && e.target.tag == "deactivated")
        {
            insideMenuGrab += 1;
            grabbingObject = e.target.transform.parent;
        }
    }

    private void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.name == "MenuBackPlane" && e.target.tag == "deactivated")
        {
            insideMenuGrab -= 1;
        }
    }
    
    // Not being used
    //private void CreateMiniWorld(GameObject surface, GameObject button)
    //{
        
    //    Vector3 scale = new Vector3 (1,1,1);
    //    Vector3 newLocalScale = new Vector3(0.1f, 0.1f, 0.1f);

    //    GameObject replicatePlane = button.GetComponent<AttachedPlane>().plane;

    //    foreach (Transform tr in replicatePlane.transform)
    //    {
    //        if (tr.tag == "replicable")
    //        {
    //            replicas.Add(Instantiate(tr.gameObject, surface.transform) as GameObject);
    //        }
    //    }
        
    //    foreach (GameObject x in replicas)
    //    {
    //        Debug.Log(x.name);
    //        //x.transform.SetParent(null);
    //        //x.transform.localScale = scale;

    //        x.transform.SetParent(surface.transform);
    //        //x.transform.localScale /= 10;
    //        //x.transform.localPosition /= 10;
    //        x.layer = 8;

    //        x.AddComponent<Interactable>();
    //        x.AddComponent<InteractableHoverEvents>();
    //        x.AddComponent<SimpleAttach>();
    //    }
    //}
}
