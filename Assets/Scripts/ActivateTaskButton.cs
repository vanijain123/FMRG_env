using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivateTaskButton : MonoBehaviour
{

    private GameObject referenceButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateTask(ref GameObject activatedWorld)
    {
        if (activatedWorld) 
        {
            DeactivateTask(ref activatedWorld);
        }

        //this.transform.Find("Text").GetComponent<TextMeshPro>().SetText("Deactivate");
        //this.transform.tag = "activated";
        //this.transform.parent.Find("MenuBackPlane").tag = "activated";

        

        activatedWorld = this.transform.parent.gameObject;

        referenceButton = GameObject.Find("SiteControls");
        referenceButton.GetComponent<SiteControls>().ActivateChildren();
        referenceButton.GetComponent<SiteControls>().SetReferenceWIM(activatedWorld.gameObject);

        activatedWorld.transform.Find("MenuBackPlane").tag = "activated";
        activatedWorld.transform.Find("Timeline").gameObject.SetActive(false);
        activatedWorld.transform.Find("ActivateTask").gameObject.SetActive(false);
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
        activatedWorld.transform.Find("ActivateTask").gameObject.SetActive(true);
        activatedWorld.transform.Find("Site").gameObject.SetActive(true);

        activatedWorld = null;

        referenceButton.GetComponent<SiteControls>().DeactivateChildren();
        
    }
}
