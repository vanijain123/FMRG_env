using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSitesManager : MonoBehaviour
{
    public GameObject siteMenuParent;

    private GameObject activeSite;

    // Start is called before the first frame update
    void Start()
    {
        activeSite = transform.GetChild(0).GetChild(0).gameObject;
        activeSite.GetComponent<SiteManager>().SelectSite();
        //siteMenuParent.SetActive(false);
    }

    public void UpdateActiveSite(GameObject newSite)
    {
        //activeSite.transform.Find("SelectedTaskHighlight").GetComponent<MeshRenderer>().enabled = false;
        activeSite.GetComponent<SiteManager>().UnselectSite();
        activeSite = newSite;
        activeSite.transform.Find("SelectedTaskHighlight").GetComponent<MeshRenderer>().enabled = true;
    }
}
