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

    private SitesManager sitesManager;

    // Start is called before the first frame update
    void Start()
    {
        activated = false;
        text.text = "Activate";
        sitesManager = SitesManager.instance;
    }

    public void ToggleTaskActivation()
    {
        siteDeleteButton.SetActive(activated);
        if (activated)
        {
            activated = false;
            text.text = "Activate";
            sitesManager.activatedTask = null;
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
            text.text = "Deactivate";
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
