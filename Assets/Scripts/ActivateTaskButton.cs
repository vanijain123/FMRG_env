using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivateTaskButton : MonoBehaviour
{

    private GameObject referenceButton;
    private Vector3 timelineOriginalScale;
    private Vector3 activateButtonOriginalScale;
    private Vector3 siteOriginalScale;

    // Start is called before the first frame update
    void Start()
    {
        timelineOriginalScale = new Vector3(1, 1, 1);
        activateButtonOriginalScale = new Vector3(0.05f, 1, 0.01f);
        siteOriginalScale = new Vector3(0.099f, 0.099f, 0.01f);
        referenceButton = GameObject.Find("SiteControls");
    }

    public void ActivateTask(ref GameObject activatedWorld)
    {
        if (activatedWorld) 
        {
            DeactivateTask(ref activatedWorld);
        }

        activatedWorld = this.transform.parent.gameObject;

        referenceButton = GameObject.Find("SiteControls");
        string siteName = activatedWorld.transform.Find("Site").Find("Text").GetComponent<TextMeshPro>().text;
        referenceButton.GetComponent<SiteControls>().ActivateChildren(siteName);
        referenceButton.GetComponent<SiteControls>().SetReferenceWIM(activatedWorld.gameObject);

        activatedWorld.transform.Find("MenuBackPlane").tag = "activated";

        //timelineOriginalScale = activatedWorld.transform.Find("Timeline").localScale;
        activatedWorld.transform.Find("Timeline").gameObject.SetActive(false);

        //activateButtonOriginalScale = activatedWorld.transform.Find("ActivateTask").localScale;
        activatedWorld.transform.Find("ActivateTask").gameObject.SetActive(false);

        //siteOriginalScale = activatedWorld.transform.Find("Site").localScale;
        activatedWorld.transform.Find("Site").gameObject.SetActive(false);
    }

    public void DeactivateTask(ref GameObject activatedWorld)
    {
        //Transform button = activatedWorld.Find("ActivateTask").transform;
        //button.transform.Find("Text").GetComponent<TextMeshPro>().SetText("Activate");
        //button.transform.tag = "deactivated";
        //button.transform.parent.Find("MenuBackPlane").tag = "deactivated";

        Debug.Log("Deactivating");
        //Debug.Log()

        activatedWorld.transform.Find("MenuBackPlane").tag = "deactivated";

        activatedWorld.transform.Find("Timeline").gameObject.SetActive(true);
        maintainScale(activatedWorld.transform.Find("Timeline").transform, timelineOriginalScale);

        activatedWorld.transform.Find("ActivateTask").gameObject.SetActive(true);
        maintainScale(activatedWorld.transform.Find("ActivateTask").transform, activateButtonOriginalScale);

        activatedWorld.transform.Find("Site").gameObject.SetActive(true);
        maintainScale(activatedWorld.transform.Find("Site").transform, siteOriginalScale);

        activatedWorld = null;

        referenceButton.GetComponent<SiteControls>().DeactivateChildren();
        
    }

    private void maintainScale(Transform localObject, Vector3 vector)
    {
        //Transform activateTask = activatedWorld.transform.Find("ActivateTask").transform;
        Transform tempParent = localObject.parent;
        localObject.parent = null;
        localObject.localScale = vector;
        localObject.parent = tempParent;
    }
}
