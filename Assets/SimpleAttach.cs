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
    public Material lineRendererMaterial;

    public bool useUniqueOutlineColor;
    public Color UniqueOutlineColor;

    public bool isOutlined;

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


        //lineRendererMaterial = new Material(Shader.Find("Standard"));
        //lineRendererMaterial. SetOverrideTag("RenderType", "Transparent");
        //Color lrColor = originalMaterial.color;
        //lrColor.a = 200;
        //lineRendererMaterial.color = lrColor;
    }

    private void Update()
    {
        if (originalGO != null)
        {
            lr.SetPosition(0, this.transform.GetChild(0).GetChild(0).position);
            lr.SetPosition(1, originalGO.transform.GetChild(0).GetChild(0).position);
        }
        else
        {
            if (lr != null)
            {
                Destroy(lr);
            }
        }
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

                    SetMaterial(originalGO, originalGO.GetComponent<SimpleAttach>().lineRendererMaterial);
                    Destroy(originalGO.GetComponent<SimpleAttach>());
                    instructions[0] = originalGO;

                    movedObject = gameObject;

                    lr = movedObject.AddComponent<LineRenderer>();
                    lr.SetPosition(0, this.transform.GetChild(0).GetChild(0).position);
                    lr.SetPosition(1, originalGO.transform.GetChild(0).GetChild(0).position);
                    lr.material = lineRendererMaterial;
                    lr.startWidth = 0.005f;
                    lr.endWidth = 0.005f;

                    // OUTLINE ON OBJECTS BEFORE INSTRUCTIONS ARE SENT
                    /*
                     if (originalGO.GetComponent<Outline>() != null)
                    {
                        originalGO.GetComponent<Outline>().enabled = true;
                        if (useUniqueOutlineColor)
                        {
                            Color c = new Color(UniqueOutlineColor.r, UniqueOutlineColor.g, UniqueOutlineColor.b);
                            originalGO.GetComponent<Outline>().OutlineColor = c;
                        }
                        else
                        {
                            originalGO.GetComponent<Outline>().OutlineColor = Color.red;
                        }
                    }

                    if (movedObject.GetComponent<Outline>() != null)
                    {
                        movedObject.GetComponent<Outline>().enabled = true;
                        if (useUniqueOutlineColor)
                        {
                            Color c = new Color(UniqueOutlineColor.r, UniqueOutlineColor.g, UniqueOutlineColor.b);
                            movedObject.GetComponent<Outline>().OutlineColor = c;
                            lr.material.color = c;
                        }
                        else
                        {
                            movedObject.GetComponent<Outline>().OutlineColor = Color.yellow;
                        }
                    }
                    */
                }
            }

            //this.GetComponent<ComplexThrowable>().PhysicsAttach(hand, grabType);
            hand.AttachObject(gameObject, grabType);
            hand.HoverLock(interactable);
            Debug.Log("Grabbing");
        }
        else if (isGrabEnding)
        {
            //this.GetComponent<ComplexThrowable>().PhysicsDetach(hand);
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
