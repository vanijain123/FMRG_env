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
    public GameObject rightHandGameobject;

    public SteamVR_Fade fade;
    public FadeTest ft;
    public SteamVR_LaserPointer laserPointer;
    public Color laserColor;
    public Color originalLaserColor;

    public Transform attachmentPoint;

    public GameObject VRCamera;

    private int insideMenuGrab;
    private Transform grabbingObject;
    public GameObject activatedWorld;
    private Vector3 leftHandPosition;
    private float handPosition;
    private float diff;

    private LineRenderer l;

    public GameObject cube;
    public GameObject cylinder;
    public GameObject sphere;

    private void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;

        insideMenuGrab = 0;
        activatedWorld = null;
        leftHandPosition = leftHand.transform.position;

        originalLaserColor = laserPointer.color;
        Material lineRenderedMaterial = new Material(Shader.Find("Unlit/Color"));
        lineRenderedMaterial.SetColor("_Color", originalLaserColor);

        l = gameObject.AddComponent<LineRenderer>();
        l.material = lineRenderedMaterial;
        l.startColor = Color.clear;
        l.endColor = Color.clear;
        l.startWidth = 0;
        l.endWidth = 0;
    }

    private void Update()
    {
        //Grabbing WIMs
        //if (insideMenuGrab > 0)
        //{
        //    if (SteamVR_Actions.default_GrabGrip[rightHand.handType].state)
        //    {
        //        grabbingObject.transform.position = attachmentPoint.position;
        //        grabbingObject.transform.rotation = attachmentPoint.rotation;
        //    }
        //}

        //DISABLED SCALING:
        //if (activatedWorld && activatedWorld.tag!="scaling")
        //{
        //    if (SteamVR_Actions.default_GrabGrip[rightHand.handType].state)
        //    {
        //        activatedWorld.transform.position = attachmentPoint.position;
        //        activatedWorld.transform.rotation = attachmentPoint.rotation;
        //    }
        //    if (SteamVR_Actions.default_GrabGrip[leftHand.handType].state)
        //    {
        //        float x = leftHandPosition.x - leftHand.transform.position.x;
        //        activatedWorld.transform.localScale += new Vector3(x, x, x);
        //    }
        //}
        //leftHandPosition = leftHand.transform.position;
    }

    private void FixedUpdate()
    {
        //laserPointer.color = testColor;
    }

    private void PointerClick(object sender, PointerEventArgs e)
    {
        Animator a = e.target.gameObject.GetComponent<Animator>();
        if (e.target.GetComponent<Button>() != null)
        { 
            e.target.GetComponent<Button>().onClick.Invoke(); 
        }


        //if (e.target.name == "AddButton")
        //{
        //    //e.target.GetComponent<AddingWorld>().AddWorld(cube, cylinder, sphere);
        //    e.target.GetComponent<AddingWorld>().AddWorld();
        //}

        //if (e.target.name == "DeleteButton")
        //{
        //    e.target.GetComponent<DeletingWorld>().DeleteWorld();
        //}

        //else if(e.target.name == "ProjectedSiteDeleteButton")
        //{
        //    e.target.GetComponent<ProjectedSiteDeleteButton>().DeleteProjectedSite();
        //}

        //else if(e.target.name == "ActivateTask" && e.target.tag == "deactivated")
        //{
        //    e.target.GetComponent<ActivateTaskButton>().ActivateTask(ref activatedWorld);
        //    //activatedObject = e.target.transform.parent;
        //}

        //else if (e.target.name == "ActivateTask" && e.target.tag == "activated")
        //{
        //    //e.target.GetComponent<ActivateTaskButton>().DeactivateTask(ref activatedWorld);
        //    e.target.parent.GetComponent<SiteControls>().DeactivateControls(ref activatedWorld);
        //    //activatedObject = null;
        //}
        
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

        if (e.target.name == "AddButton")
        {
            SnapPointerToButton(e.target.gameObject);
        }

        if (e.target.name == "DeleteButton")
        {
            SnapPointerToButton(e.target.gameObject);
        }

        if (e.target.name == "ButtonOuter")
        {
            SnapPointerToButton(e.target.gameObject);
        }
    }

    private void PointerOutside(object sender, PointerEventArgs e)
    {
        if (e.target.name == "MenuBackPlane" && e.target.tag == "deactivated")
        {
            insideMenuGrab -= 1;
        }
        if (e.target.name == "AddButton")
        {
            UnsnapPointerToButton();
        }
        if (e.target.name == "DeleteButton")
        {
            UnsnapPointerToButton();
        }
        if (e.target.name == "ButtonOuter")
        {
            UnsnapPointerToButton();
        }
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
