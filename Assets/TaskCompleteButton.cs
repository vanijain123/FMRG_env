using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCompleteButton : MonoBehaviour
{
    private GameObject resetButton;
    private GameObject unlockButton;
    private GameObject manageInstructionsButton;
    private bool instructionSent;

    private ManageInstructions manageInstructions;

    public void SetVariables(GameObject reset, GameObject unlock, GameObject instructions)
    {
        resetButton = reset;
        unlockButton = unlock;
        manageInstructionsButton = instructions;
        manageInstructions = manageInstructionsButton.GetComponent<ManageInstructions>();
    }

    public void TaskCompleteAcknowledge()
    {
        if (unlockButton.GetComponent<ActivateTask>().activated)
        {
            resetButton.SetActive(true);
            manageInstructionsButton.GetComponent<MeshRenderer>().enabled = true;
        }
        unlockButton.SetActive(true);
        manageInstructions.instructionSent = false;
        resetButton.GetComponent<ResetTask>().ReevaluateTask();

        manageInstructions.ResetTimeline(manageInstructions.timeline);
        manageInstructions.ResetTimeline(manageInstructions.siteIconTimeline);
        manageInstructions.siteIcon.siteIcon.GetComponent<MeshRenderer>().material = manageInstructions.siteIcon.white;

        this.gameObject.SetActive(false);
    }
}
