using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddingWorld : MonoBehaviour
{
    private GameObject deleteButton;
    private Transform parent;

    private void Start()
    {
        parent = this.transform.parent;
        deleteButton = parent.Find("DeleteButton").gameObject;
        deleteButton.SetActive(false);
    }

    public void AddWorld()
    {
        parent.GetComponent<SiteManager>().ProjectSelectedSite();
        deleteButton.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
