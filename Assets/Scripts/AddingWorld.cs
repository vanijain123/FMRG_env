using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddingWorld : MonoBehaviour
{
    public GameObject site;
    public GameObject deleteButton;

    public void AddWorld()
    {
        site.GetComponent<SiteManager>().ProjectSelectedSite();
        deleteButton.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void AddWorld(GameObject cube, GameObject cylinder, GameObject sphere)
    {
        //parent.GetComponent<SiteManager>().ProjectSelectedSite(cube, cylinder, sphere);
        deleteButton.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
