using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllSitesManager : MonoBehaviour
{
    //public GameObject siteMenuParent;
    //public GameObject helpNeededHighlight;
    //public GameObject mainPlane;

    //private GameObject activeSite;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    activeSite = transform.GetChild(0).GetChild(0).gameObject;
    //    activeSite.GetComponent<SiteManager>().SelectSite();
    //    //siteMenuParent.SetActive(false);

    //    Invoke("RandomNotif", Random.Range(10, 15));
    //}

    //private void RandomNotif()
    //{
    //    float delay = Random.Range(10.0f, 30.0f);

    //    int s = Random.Range(0, 2);
    //    Transform siteParent = this.transform.GetChild(s);
    //    Transform tasks = siteParent.GetChild(1);
    //    int t = Random.Range(0, tasks.childCount);
    //    Transform helpTask = tasks.GetChild(t);
    //    if (helpTask.childCount == 1)
    //    {
    //        Instantiate(helpNeededHighlight, helpTask);
    //    }
    //    //helpTask.Find("HelpNeededHighlight").gameObject.SetActive(true);

    //    Invoke("RandomNotif", delay);
    //}

    //public void UpdateActiveSite(GameObject newSite)
    //{
    //    //activeSite.transform.Find("SelectedTaskHighlight").GetComponent<MeshRenderer>().enabled = false;
    //    activeSite.GetComponent<SiteManager>().UnselectSite();
    //    activeSite = newSite;
    //    activeSite.transform.Find("SelectedTaskHighlight").GetComponent<MeshRenderer>().enabled = true;
    //}
}
