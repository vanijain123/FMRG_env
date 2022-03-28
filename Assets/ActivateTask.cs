using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivateTask : MonoBehaviour
{
    public bool activated;
    public TextMeshPro text;
    public GameObject siteDeleteButton;
    public ChildModels childModels;
    public Material locked;
    public Material unlocked;
    public GameObject undoButton;
    public GameObject sendInstructionsButton;

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
        siteDeleteButton.SetActive(activated);
        if (activated)
        {
            activated = false;
            //text.text = "Activate";
            this.GetComponent<MeshRenderer>().material = locked;
            sitesManager.activatedTask = null;
            undoButton.SetActive(false);
            sendInstructionsButton.SetActive(false);
        }
        else
        {
            if (sitesManager.activatedTask == null)
            {
                //Do nothing
            }
            else if(sitesManager.activatedTask != gameObject)
            {
                //if (sitesManager.activatedTask.GetComponent<ActivateTask>() != null)
                //{
                    sitesManager.activatedTask.GetComponent<ActivateTask>().ToggleTaskActivation();
                //}
            }
            activated = true;
            undoButton.SetActive(true);
            sendInstructionsButton.SetActive(true);
            //text.text = "Deactivate";
            this.GetComponent<MeshRenderer>().material = unlocked;
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
