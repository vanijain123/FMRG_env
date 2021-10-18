using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using TMPro;


public class SimpleAttach : MonoBehaviour
{
    private Interactable interactable;
    //private Transform parent;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;

    private string originalTag;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
       originalTag = "";
    }

    private void OnHandHoverBegin(Hand hand)
    {
        //hand.ShowGrabHint();
    }

    private void OnHandHoverEnd(Hand hand)
    {
        //hand.HideGrabHint();
    }

    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes grabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(gameObject);

        if (interactable.attachedToHand == null && grabType != GrabTypes.None)
        {
            //if (gameObject.transform.parent.parent.Find("ActivateTask").tag == "activated")
            if (gameObject.transform.parent.parent.Find("MenuBackPlane").tag == "activated")
                {
                originalTag = gameObject.transform.parent.parent.tag;
                gameObject.transform.parent.parent.tag = "scaling";
            }
            startPosition = gameObject.transform.position;
            startRotation = gameObject.transform.rotation;
            startScale = gameObject.transform.localScale;

            hand.AttachObject(gameObject, grabType);
            hand.HoverLock(interactable);
            //hand.HideGrabHint();
        }
        else if (isGrabEnding)
        {
            hand.DetachObject(gameObject);
            hand.HoverUnlock(interactable);

            //if (gameObject.transform.parent.parent.Find("ActivateTask").tag != "activated")
            if (gameObject.transform.parent.parent.Find("MenuBackPlane").tag != "activated")
            {
                gameObject.transform.localScale = startScale;
                gameObject.transform.position = startPosition;
                gameObject.transform.rotation = startRotation;
            }
            else
            {
                gameObject.transform.parent.parent.tag = originalTag;
            }
        }
    }
}
