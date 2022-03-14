using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectedSiteDeleteButton : MonoBehaviour
{
    public ProjectedSiteManager pm;

    public void DeleteProjectedSite()
    {
        //GameObject ms = this.transform.parent.parent.GetComponent<ProjectedSiteManager>().GetMenuSite();
        //ms.transform.Find("DeleteButton").GetComponent<DeletingWorld>().DeleteWorld();
        pm.menuSite.GetComponent<SiteManager>().deleteButton.GetComponent<DeletingWorld>().DeleteWorld();
    }
}
