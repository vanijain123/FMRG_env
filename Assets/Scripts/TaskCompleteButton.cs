using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskCompleteButton : MonoBehaviour
{
    private GameObject resetButton;
    private GameObject manageInstructionsButton;

    private ManageInstructions manageInstructions;

    public void SetVariables(GameObject reset, GameObject unlock, GameObject instructions)
    {
        resetButton = reset;
        manageInstructionsButton = instructions;
        manageInstructions = manageInstructionsButton.GetComponent<ManageInstructions>();
    }

    public void TaskCompleteAcknowledge()
    {
        manageInstructions.ChangeButtonInteractability(true);

        manageInstructions.instructionSent = false;
        resetButton.GetComponent<ResetTask>().ReevaluateTask();

        manageInstructions.ResetTimeline(manageInstructions.timeline);
        manageInstructions.ResetTimeline(manageInstructions.siteIconTimeline);
        manageInstructions.siteIcon.siteIcon.GetComponent<MeshRenderer>().material = manageInstructions.siteIcon.white;

        this.gameObject.SetActive(false);
    }
}
