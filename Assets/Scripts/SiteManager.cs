using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteManager : MonoBehaviour
{
    public GameObject taskGroup;

    private List<GameObject> tasks = new List<GameObject>();
    private GameObject projectionComponents;

    // Start is called before the first frame update
    void Awake()
    {
        Transform t = transform.parent.transform.Find("Tasks");
        foreach (Transform tr in t)
        {
            tasks.Add(tr.gameObject);
            //Debug.Log(tasks);
        }

        projectionComponents = transform.parent.transform.parent.transform.parent.Find("ProjectionComponents").gameObject;
    }

    public void SelectSite()
    {
        transform.parent.transform.parent.GetComponent<AllSitesManager>().UpdateActiveSite(this.gameObject);
        ProjectSite();
    }

    public void UnselectSite()
    {
        transform.Find("SelectedTaskHighlight").GetComponent<MeshRenderer>().enabled = false;
        Transform t = projectionComponents.transform.Find("Tasks");
        foreach (Transform tr in t)
        {
            Destroy(tr.gameObject);
        }
    }

    public void ProjectSite()
    {
        float x = -0.2f, y = 0, z = 0;
        foreach (GameObject task in tasks)
        {
            Transform parent = this.transform.parent.transform.parent.transform.parent.Find("ProjectionComponents").Find("Tasks");
            GameObject t = Instantiate(taskGroup, parent);
            t.transform.Find("Task").GetComponent<MeshRenderer>().material = task.GetComponent<MeshRenderer>().material;
            t.transform.localPosition = new Vector3(x, y, z);
            y -= 0.1f;
            z -= 0.1f;
        }
    }
}
