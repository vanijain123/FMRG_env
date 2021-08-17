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
    public SteamVR_Action_Boolean menuManipulation;
    public Hand hand;
    public bool menuHeld;

    public SteamVR_Action_Vector2 joystick;

    public Animator redButton;
    public LineRenderer lr;

    public SteamVR_Input_Sources rightController;
    public GameObject rightHand;

    public GameObject menusParent;
    public GameObject siteMenusParent;
    public GameObject menuGameobject;

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

    private Vector3 pos;
    private float dis;

    public GameObject selectedMenu;
    private Transform ogPos;

    private GameObject activeTimestamp;

    

    private void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        laserPointer.PointerOut += PointerOutside;
        laserPointer.PointerClick += PointerClick;

        pos = new Vector3(0, 0, 0);

        currentPlane = greenPlane;

        clicked = false;
        dis = 0f;

        menuHeld = false;

        FindAllPlanes();
    }



    private void FindAllPlanes()
    {
        redPlane = GameObject.Find("RedPlane");
        greenPlane = GameObject.Find("GreenPlane");
        yellowPlane = GameObject.Find("YellowPlane");

        redPlane.SetActive(false);
        yellowPlane.SetActive(false);
    }

    private void Update()
    {
        if (menuHeld)
        {
            Vector3 pointerPos = laserPointer.transform.position;

            Vector3 ax = SteamVR_Actions.default_MoveMenu[hand.handType].axis / 100;

            if (ax.x != 0 || ax.y != 0)
            {
                selectedMenu.transform.position += new Vector3(ax.x + Mathf.Pow(ax.x, 2), 0, ax.y + Mathf.Pow(ax.y, 2));
            }
            else if (SteamVR_Actions.default_GrabGrip[hand.handType].state)
            {
                selectedMenu.transform.position = attachmentPoint.transform.position;
            }
            else if (SteamVR_Actions.default_GrabPinch[hand.handType].state)
            {
                selectedMenu.transform.position = ogPos.position;
            }
        }
    }

    private void PointerClick(object sender, PointerEventArgs e)
    {

        Animator a = e.target.gameObject.GetComponent<Animator>();

        if(e.target.tag == "site")
        {

            e.target.GetComponent<SiteManager>().SelectSite();
        }

        if (e.target.tag == "timestamp")
        {
            TimestampClicked(e.target.gameObject);
        }

        if (e.target.tag == "siteTask")
        {
            if (e.target.transform.childCount > 1)
            {   
                Destroy(e.target.transform.GetChild(1).gameObject);
            }
            e.target.transform.parent.transform.parent.GetChild(0).GetComponent<SiteManager>().SelectSite(e.target.GetSiblingIndex());
        }

        if (e.target.tag == "task")
        {
            e.target.transform.parent.transform.parent.transform.parent.Find("TaskProjectionPlane").GetComponent<TaskProjectionPlane>().SetTask(e.target.gameObject);
        }

        if (e.target.name == "MovementPlatform" && !menuHeld)
        {
            menuHeld = true;
            selectedMenu = e.target.transform.parent.gameObject;
            e.target.transform.parent.transform.Find("MenuHighlight").gameObject.SetActive(true);
            ogPos = selectedMenu.transform;
        }
        else if (e.target.name == "MovementPlatform" && menuHeld)
        {
            menuHeld = false;
            e.target.transform.parent.transform.Find("MenuHighlight").gameObject.SetActive(false);
        }

        if (e.target.tag == "planeButton")
        {
            e.target.gameObject.GetComponent<AttachedPlane>().projectionPlane.GetComponent<ProjectionPlane>().CreateMiniWorld(e.target.gameObject.GetComponent<AttachedPlane>().plane);
            e.target.gameObject.GetComponent<AttachedPlane>().projectionPlane.GetComponent<ProjectionPlane>().displayPlaneInfo(e.target.gameObject.GetComponent<AttachedPlane>().plane);

            GameObject teleportButton = e.target.gameObject.GetComponent<AttachedPlane>().teleportButton.gameObject;
            teleportButton.GetComponent<TeleportButton>().nextPlane = e.target.gameObject.GetComponent<AttachedPlane>().plane;
        }

        if (e.target.name == "TeleportButton_Button")
        {
            ChangePosition(e.target.gameObject);
        }
        else if (e.target.name == "DuplicatetButton_Button")
        {
            Debug.Log("Duplicate button clicked" + e.target.name);
            DuplicateMenu(e.target.transform.parent.transform.parent.transform.parent.gameObject);
        }
        else if (e.target.name == "MenuShowHideButton")
        {
            ToggleMenu(menusParent);
        }
        else if (e.target.name == "SiteMenuShowHideButton")
        {
            ToggleMenu(siteMenusParent);
        }
    }

    private void TimestampClicked(GameObject ts)
    {
        ts.transform.parent.GetComponent<Timestamps>().SetTimestamp(ts);
    }

    private void ToggleMenu(GameObject menusParent)
    {

        Vector3 playerPos = VRCamera.transform.position;
        Vector3 playerDirection = VRCamera.transform.forward;
        Quaternion playerRotation = VRCamera.transform.rotation;
        playerRotation *= Quaternion.Euler(0, -90, 0);

        Vector3 eulerRotation = playerRotation.eulerAngles;
        playerRotation = Quaternion.Euler(0, eulerRotation.y, 0);

        //playerRotation = Quaternion.Euler(0, playerRotation.y, 0);
        float spawnDistance = 1.5f;

        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
        menusParent.transform.position = spawnPos;
        menusParent.transform.rotation = playerRotation;
        menusParent.SetActive(!menusParent.activeSelf);
    }

    private void DuplicateMenu(GameObject menu)
    {
        Debug.Log("Inside Duplicate Menu");
        Quaternion q = menu.transform.rotation;
        Vector3 pos = new Vector3(menu.transform.position.x - 0.8f, menu.transform.position.y, menu.transform.position.z);
        Instantiate(menuGameobject, pos, q, menu.transform.parent.transform);
    }

    private void Grab(GameObject g)
    {
        g.transform.position = attachmentPoint.position;
    }

    private void PointerInside(object sender, PointerEventArgs e)
    {
        /*Pointer Inside not being used
        if (e.target.tag == "planeButton")
        {
            Debug.Log(nextPlane + " button entered");
        }

        if (e.target.name == "MovementPlatform")
        {
            Debug.Log("Inside Movement Platform");
        }*/
    }

    private void PointerOutside(object sender, PointerEventArgs e)
    {
        /* Pointer outside not being used
        if (e.target.tag == "planeButton")
        {
            Debug.Log(nextPlane + " button exited");
        }*/
    }

    private void ChangePosition(GameObject teleport_button)
    {
        GameObject currentPlane = teleport_button.GetComponent<TeleportButton>().currentPlane.gameObject;
        Debug.Log("CurrentPlane: " + currentPlane);

        currentPlane.SetActive(false);

        teleport_button.GetComponent<TeleportButton>().currentPlane = teleport_button.GetComponent<TeleportButton>().nextPlane;
        transform.position = teleport_button.GetComponent<TeleportButton>().nextPlane.transform.position;
        GameObject nextPlane = teleport_button.GetComponent<TeleportButton>().nextPlane.gameObject;
        Debug.Log("NextPlane: " + nextPlane);
        nextPlane.SetActive(true);
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

    // Not being used
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
        }

        CreateMiniWorld(projectionPlane, button);
        Debug.Log("Inside ProjectOnPlane");
        
        Debug.Log(projectionPlane.GetComponent<MeshRenderer>().materials[0]);
        
    }

    private void HideProjectedComponents()
    {
        // Destroy gameobjects and empty replicas collection
        if (replicas.Count != 0)
        {
            DestroyReplicas();
        }
        
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
    
    // Not being used
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
