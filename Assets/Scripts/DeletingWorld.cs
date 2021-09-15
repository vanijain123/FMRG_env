using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletingWorld : MonoBehaviour
{
    private GameObject addButton;
    private Transform parent;

    // Start is called before the first frame update
    void Awake()
    {
        parent = this.transform.parent;
        addButton = parent.Find("AddButton").gameObject;
    }

    public void DeleteWorld()
    {
        parent.GetComponent<SiteManager>().DeleteProjectedSite();
        addButton.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
