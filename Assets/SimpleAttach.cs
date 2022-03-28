using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using TMPro;


public class SimpleAttach : MonoBehaviour
{
    public bool activated;
    public Material originalPositionMaterial;
    public GameObject siteParent;
    public GameObject movedObject = null;
    public GameObject originalGO = null;
    public GameObject[] instructions;

    private Interactable interactable;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private string originalTag;
    private GameObject emptyGO;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        instructions = new GameObject[] { originalGO, movedObject };
        emptyGO = new GameObject();
    }

    private void OnHandHoverBegin(Hand hand)
    {
        //hand.ShowGrabHint();
    }

    private void OnHandHoverEnd(Hand hand)
    {
        //hand.HideGrabHint();
    }

    //private void HandHoverUpdate(Hand hand)
    //{
    //    GrabTypes grabType = hand.GetGrabStarting();
    //    bool isGrabEnding = hand.IsGrabEnding(gameObject);
    //    GameObject menuBackPlane = gameObject.transform.parent.parent.parent.Find("MenuBackPlane").gameObject;

    //    if (interactable.attachedToHand == null && grabType != GrabTypes.None)
    //    {
    //        //if (gameObject.transform.parent.parent.Find("ActivateTask").tag == "activated")
    //        //if (gameObject.transform.parent.parent.Find("MenuBackPlane").tag == "activated")
    //        if (menuBackPlane != null && menuBackPlane.tag == "activated")
    //        {
    //            originalTag = menuBackPlane.tag;
    //            menuBackPlane.tag = "scaling";
    //        }
    //        startPosition = gameObject.transform.position;
    //        startRotation = gameObject.transform.rotation;
    //        startScale = gameObject.transform.localScale;

    //        hand.AttachObject(gameObject, grabType);
    //        hand.HoverLock(interactable);
    //        //hand.HideGrabHint();
    //    }
    //    else if (isGrabEnding)
    //    {
    //        hand.DetachObject(gameObject);
    //        hand.HoverUnlock(interactable);

    //        //if (gameObject.transform.parent.parent.Find("ActivateTask").tag != "activated")
    //        if (menuBackPlane != null && menuBackPlane.tag != "activated")
    //        {
    //            gameObject.transform.localScale = startScale;
    //            gameObject.transform.position = startPosition;
    //            gameObject.transform.rotation = startRotation;
    //        }
    //        else
    //        {
    //            menuBackPlane.tag = originalTag;
    //        }
    //    }
    //}

    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes grabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);
        Debug.Log("HandHoverUpdate");

        if (interactable.attachedToHand == null && grabType != GrabTypes.None)
        {
            if (movedObject!=null && gameObject!=movedObject && originalGO!=null)
            {
                Debug.Log("You cannot move this object");
            }

            startPosition = gameObject.transform.position;
            startRotation = gameObject.transform.rotation;

            if (activated)
            {
                Debug.Log("Calling AddInstructions");
                siteParent.GetComponent<ProjectedSiteManager>().addInstruction(gameObject, gameObject.transform.localPosition, gameObject.transform.localScale, gameObject.transform.localRotation);

                if (gameObject != movedObject)
                {
                    originalGO = GameObject.Instantiate(gameObject, transform.parent);
                    originalGO.transform.localPosition = gameObject.transform.localPosition;
                    originalGO.transform.localScale = gameObject.transform.localScale;
                    originalGO.transform.localRotation = gameObject.transform.localRotation;
                    Destroy(originalGO.GetComponent<SimpleAttach>());
                    SetMaterial(originalGO, originalPositionMaterial);
                    instructions[0] = originalGO;

                    movedObject = gameObject;
                }
            }

            hand.AttachObject(gameObject, grabType);
            hand.HoverLock(interactable);
            Debug.Log("Grabbing");
        }
        else if (isGrabEnding)
        {
            hand.DetachObject(gameObject);
            hand.HoverUnlock(interactable);
            Debug.Log("End Grabbing");

            if (!activated)
            {
                gameObject.transform.position = startPosition;
                gameObject.transform.rotation = startRotation;

                movedObject = null;
            }
            else
            {
                movedObject = gameObject;
                instructions[1] = movedObject;
            }
        }
    }

    public void SetMaterial(GameObject go, Material m)
    {
        for (int i=0; i<go.transform.childCount; i++)
        {
            go.transform.GetChild(i).GetComponent<MeshRenderer>().material = m;
        }
    }
}
