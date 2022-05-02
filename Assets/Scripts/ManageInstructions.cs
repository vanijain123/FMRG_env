using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ManageInstructions : MonoBehaviour
{
    public ProjectedSiteManager pm;
    public ChildModels childModels;
    public TextMeshPro text;
    public Material instructionsSentMaterial;
    public GameObject timeline;
    public TaskCompleteButton taskCompleteButton;
    public GameObject siteIconTimeline;

    public GameObject resetButton;
    public GameObject unlockButton;
    public bool instructionSent;
    public List<GameObject> instructionsSentList = new List<GameObject>();

    public Queue<Queue<GameObject[]>> instructionsQueue = new Queue<Queue<GameObject[]>>();

    private List<GameObject> children;
    
    public SiteIconManager siteIcon;

    private int numberOfInstructions;
    private Vector3 timelineOriginalScale;
    private string ungrabbableTag = "Ungrabbable";

    public Material greyManageInstructionsMaterial;
    public Material greyResetButtonMaterial;
    public Material greyUnlockButtonMaterial;
    public Material greyLockButtonMaterial;

    private Material originalManageInstructionsMaterial;
    private Material originalResetButtonMaterial;

    // Start is called before the first frame update
    void Start()
    {
        children = childModels.modelParts;
        instructionSent = false;
        siteIcon = pm.siteIconManager;
        timeline.GetComponent<Timeline>().siteIconTimeline = siteIcon.GetComponent<SiteIconManager>().siteIconTimeline;
        siteIconTimeline = timeline.GetComponent<Timeline>().siteIconTimeline;
        taskCompleteButton.SetVariables(resetButton, unlockButton, gameObject);
        timelineOriginalScale = timeline.transform.localScale;

        pm.manageInstructions = this.GetComponent<ManageInstructions>();
    }

    public void buttonInteraction()
    {
        if (instructionSent == true)
        {
            Debug.Log("buttonInteraction already pressed");
        }
        else
        {
            Debug.Log("buttonInteraction");
            numberOfInstructions = 0;

            Queue<GameObject[]> instructionSet = new Queue<GameObject[]>();

            for (int i = 0; i < children.Count; i++)
            {
                GameObject[] instructions = children[i].GetComponent<SimpleAttach>().instructions;
                if (instructions[0] != null && instructions[1] != null)
                {
                    if (instructions[0].transform != instructions[1].transform)
                    {
                        SetMaterial(children[i], children[i].GetComponent<SimpleAttach>().instructionSentMaterial);
                        if (children[i].GetComponent<Outline>() != null)
                        {
                            children[i].GetComponent<Outline>().enabled = true;
                            children[i].GetComponent<Outline>().OutlineColor = Color.yellow;
                        }

                        //Add to the instruction set
                        instructionSet.Enqueue(instructions);

                        instructionsSentList.Add(instructions[0]);
                        children[i].GetComponent<SimpleAttach>().movedObject.tag = ungrabbableTag;

                        instructions[0] = null;
                        instructions[1] = null;

                        numberOfInstructions += 1;
                        Debug.Log($"Number of instructions: {numberOfInstructions}");
                        instructionSent = true;
                    }
                }
            }
            if (instructionSent == true)
            {
                instructionsQueue.Enqueue(instructionSet);

                ChangeButtonInteractability(false);
                StartCoroutine("StartTask");
            }
        }
    }

    private void SetMaterial(GameObject go, Material m)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            go.transform.GetChild(i).GetComponent<MeshRenderer>().material = m;
        }
    }

    IEnumerator StartTask()
    {
        ResetTimeline(timeline);
        ResetTimeline(siteIconTimeline);
        timeline.GetComponent<Timeline>().amount /= instructionsQueue.Peek().Count;
        yield return StartCoroutine(timeline.GetComponent<Timeline>().WaitCoroutine());
        siteIcon.siteIcon.GetComponent<MeshRenderer>().material = siteIcon.green;
        taskCompleteButton.gameObject.SetActive(true);
    }

    public void ResetTimeline(GameObject timeline)
    {
        timeline.transform.localPosition = new Vector3(-0.1412f, 0, 0);
        timeline.transform.localScale = timelineOriginalScale;
    }

    public void ChangeButtonInteractability(bool isInteractable)
    {
        resetButton.GetComponent<Button>().interactable = isInteractable;
        this.GetComponent<Button>().interactable = isInteractable;
        GreyOutButtons(isInteractable);
    }

    private void GreyOutButtons(bool isInteractable)
    {
        if (!isInteractable)
        {
            originalResetButtonMaterial = resetButton.GetComponent<MeshRenderer>().material;
            resetButton.GetComponent<MeshRenderer>().material = greyResetButtonMaterial;

            originalManageInstructionsMaterial = this.GetComponent<MeshRenderer>().material;
            this.GetComponent<MeshRenderer>().material = greyManageInstructionsMaterial;
        }
        else
        {
            resetButton.GetComponent<MeshRenderer>().material = originalResetButtonMaterial;
            this.GetComponent<MeshRenderer>().material = originalManageInstructionsMaterial;
        }
    }
}
