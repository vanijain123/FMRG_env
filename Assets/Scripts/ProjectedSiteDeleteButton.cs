using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectedSiteDeleteButton : MonoBehaviour
{
    public void DeleteProjectedSite()
    {
        GameObject ms = this.transform.parent.parent.GetComponent<ProjectedSiteManager>().GetMenuSite();
        ms.transform.Find("DeleteButton").GetComponent<DeletingWorld>().DeleteWorld();
    }
}
