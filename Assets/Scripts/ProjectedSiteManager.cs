using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectedSiteManager : MonoBehaviour
{
    private GameObject menuSite;

    public void SetMenuSite(GameObject ms)
    {
        menuSite = ms;
    }

    public GameObject GetMenuSite()
    {
        return menuSite;
    }
}
