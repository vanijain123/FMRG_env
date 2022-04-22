using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using SitesManager;

public class MakeAttachedSiteVisible : MonoBehaviour
{
    public GameObject attachedSite;

    public void SwitchSite()
    {
        SitesManager.instance.MoveSiteToInvisibleParent();
        SitesManager.instance.visibleTask = attachedSite;
        SitesManager.instance.MoveSiteToVisibleParent();
    }
}
