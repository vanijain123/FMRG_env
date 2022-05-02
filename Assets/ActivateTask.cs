using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivateTask : MonoBehaviour
{
    public bool activated;
    public TextMeshPro text;
    public ChildModels childModels;
    public Material locked;
    public Material unlocked;
    public GameObject undoButton;
    public GameObject sendInstructionsButton;
    public ProjectedSiteManager pm;

    private SitesManager sitesManager;

    // Start is called before the first frame update
    void Start()
    {
        activated = false;
        //text.text = "Activate";
        sitesManager = SitesManager.instance;
    }

    public void ToggleTaskActivation()
    {
        if (activated)
        {
            activated = false;
            //text.text = "Activate";
            this.GetComponent<MeshRenderer>().material = locked;
            pm.plane.GetComponent<MeshRenderer>().material = pm.lockedPlaneMaterial;
            sitesManager.activatedTask = null;
            undoButton.SetActive(false);
            //sendInstructionsButton.SetActive(false);
            sendInstructionsButton.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            if (sitesManager.activatedTask == null)
            {
                //Do nothing
            }
            else if(sitesManager.activatedTask != gameObject)
            {
                sitesManager.activatedTask.GetComponent<ActivateTask>().ToggleTaskActivation();
            }
            activated = true;
            undoButton.SetActive(true);
            //sendInstructionsButton.SetActive(true);
            sendInstructionsButton.GetComponent<MeshRenderer>().enabled = true;
            //text.text = "Deactivate";
            this.GetComponent<MeshRenderer>().material = unlocked;
            pm.plane.GetComponent<MeshRenderer>().material = pm.unlockedPlaneMaterial;
            sitesManager.activatedTask = gameObject;
        }
        ToggleModelPartsActivation(activated);
    }

    private void ToggleModelPartsActivation(bool status)
    {
        for (int i = 0; i < childModels.modelParts.Count; i++)
        {
            childModels.modelParts[i].GetComponent<SimpleAttach>().activated = status;
        }
    }
}
