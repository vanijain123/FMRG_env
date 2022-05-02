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
        else
        {
            MovePlayerInFrontOfSite();
        }
    }

    private void MovePlayerInFrontOfSite()
    {
        Vector3 attachedSitePos = attachedSite.transform.position;
        Vector3 attachedSiteDirection = attachedSite.transform.forward;
        Quaternion attachedSiteRotation = attachedSite.transform.rotation;
        float spawnDistance = -0.8f;

        Vector3 spawnPos = attachedSitePos + attachedSiteDirection * spawnDistance;

        SitesManager.instance.player.transform.position = new Vector3 (spawnPos.x, 0, spawnPos.z);
        SitesManager.instance.player.transform.eulerAngles = new Vector3(0, attachedSiteRotation.eulerAngles.y, 0);
    }
}
