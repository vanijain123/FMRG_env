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

    public Material originalMaterial;
    public Material objectMovedMaterial;
    public Material instructionSentMaterial;
    public LineRenderer lr;

    private Interactable interactable;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private string originalTag;
    private string grabbableTag;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
        instructions = new GameObject[] { originalGO, movedObject };
        grabbableTag = this.gameObject.tag;
    }

    private void Update()
    {
        //if (originalGO != null)
        //{
        //    lr.SetPosition(0, this.transform.position);
        //}
        //else
        //{
        //    if (lr != null)
        //    {
        //        Destroy(lr);
        //    }
        //}
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
        Debug.Log("HandHoverUpdate");

        if (interactable.attachedToHand == null && grabType != GrabTypes.None && this.tag == grabbableTag)
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

                    SetMaterial(originalGO, originalGO.GetComponent<SimpleAttach>().objectMovedMaterial);
                    Destroy(originalGO.GetComponent<SimpleAttach>());
                    instructions[0] = originalGO;

                    movedObject = gameObject;

                    //movedObject.GetComponent<CreateLineRenderer>().enabled = true;
                    //originalGO.GetComponent<CreateLineRenderer>().StartLR(originalGO, movedObject, originalPositionMaterial);

                    //lr = movedObject.AddComponent<LineRenderer>();
                    //lr.SetPosition(0, this.transform.position);
                    //lr.SetPosition(1, originalGO.transform.position);
                    //lr.material = objectMovedMaterial;
                    //lr.startWidth = 0.01f;
                    //lr.endWidth = 0.01f;
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

    public void SetMaterial(GameObject go, Material m, bool originalMaterial = true)
    {
        for (int i=0; i<go.transform.childCount; i++)
        {
            go.transform.GetChild(i).GetComponent<MeshRenderer>().material = m;
        }
    }
}
