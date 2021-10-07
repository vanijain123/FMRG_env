using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActivateTaskButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateTask(ref Transform activatedWorld)
    {
        if (activatedWorld) 
        {
            DeactivateTask(ref activatedWorld);
        }
        this.transform.Find("Text").GetComponent<TextMeshPro>().SetText("Deactivate");
        this.transform.tag = "activated";
        this.transform.parent.Find("MenuBackPlane").tag = "activated";
        activatedWorld = this.transform.parent;
    }

    public void DeactivateTask(ref Transform activatedWorld)
    {
        Transform button = activatedWorld.Find("ActivateTask").transform;
        button.transform.Find("Text").GetComponent<TextMeshPro>().SetText("Activate");
        button.transform.tag = "deactivated";
        button.transform.parent.Find("MenuBackPlane").tag = "deactivated";
        activatedWorld = null;
    }
}
