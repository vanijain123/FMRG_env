using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletingWorld : MonoBehaviour
{
    public GameObject site;
    public GameObject addButton;

    public void DeleteWorld()
    {
        //site.GetComponent<SiteManager>().DeleteProjectedSite();
        SiteManager sm = site.GetComponent<SiteManager>();
        int siteID = sm.getSiteID();
        site.transform.parent.parent.GetComponent<SitesManager>().positions.available[siteID] = true; 
        sm.projectionSite.SetActive(false);
        addButton.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
