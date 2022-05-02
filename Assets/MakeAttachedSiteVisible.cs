using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using SitesManager;

public class MakeAttachedSiteVisible : MonoBehaviour
{
    public GameObject attachedSite;
    public GameObject siteSelectedBackground;

    public void SwitchSite()
    {
        if (SitesManager.instance.singleWIM)
        {
            SitesManager.instance.MoveSiteToInvisibleParent();
            SitesManager.instance.visibleTask = attachedSite;
            SitesManager.instance.visibleTaskIconBackground = siteSelectedBackground;
            SitesManager.instance.MoveSiteToVisibleParent();
        }
    }
}
