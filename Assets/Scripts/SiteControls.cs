using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteControls : MonoBehaviour
{
    private GameObject referenceWIM;

    public void SetReferenceWIM(GameObject wim)
    {
        referenceWIM = wim;
    }

    public void ActivateChildren()
    {
        foreach (Transform child in transform)
        {
            Debug.Log("Activating: ", child);
            child.gameObject.SetActive(true);
        }
    }

    public void DeactivateChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void DeactivateControls(ref GameObject activatedWorld)
    {
        GameObject temp = referenceWIM;
        referenceWIM.transform.Find("ActivateTask").GetComponent<ActivateTaskButton>().DeactivateTask(ref activatedWorld);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
