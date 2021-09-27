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

    public void ActivateTask()
    {
        this.transform.Find("Text").GetComponent<TextMeshPro>().SetText("Deactivate");
        this.transform.tag = "activated";
        this.transform.parent.Find("MenuBackPlane").tag = "activated";
    }

    public void DeactivateTask()
    {
        this.transform.Find("Text").GetComponent<TextMeshPro>().SetText("Activate");
        this.transform.tag = "deactivated";
        this.transform.parent.Find("MenuBackPlane").tag = "deactivated";
    }
}
