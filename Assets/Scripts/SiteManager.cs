using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteManager : MonoBehaviour
{
    public GameObject taskGroup;
    public GameObject ts;

    private List<GameObject> tasks = new List<GameObject>();
    private GameObject projectionComponents;
    private GameObject firstTask;

    // Start is called before the first frame update
    void Awake()
    {
        Transform t = transform.parent.transform.Find("Tasks");
        foreach (Transform tr in t)
        {
            tasks.Add(tr.gameObject);
            //Debug.Log(tasks);
        }

        firstTask = tasks[0].gameObject;


        projectionComponents = transform.parent.transform.parent.transform.parent.Find("ProjectionComponents").gameObject;
        //SelectSite();
    }

    public void SelectSite(int index = 0)
    {
        transform.parent.transform.parent.GetComponent<AllSitesManager>().UpdateActiveSite(this.gameObject);
        Debug.Log(index);
        //projectionComponents.GetComponent<TaskProjectionPlane>().FindTask(index);
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

            int steps = task.GetComponent<Task>().steps;

            GameObject timestampParent = t.transform.Find("Timeline").Find("Cylinder").Find("Timestamps").gameObject;
            //Debug.Log(timestampParent.GetComponent<MeshRenderer>().bounds);

            float a = 0, b = 1.0f, c = 0;

            for (int i=0; i<steps; i++)
            {
                GameObject temp = Instantiate(ts, timestampParent.transform);
                temp.transform.localPosition = new Vector3(a, b, c);
                b -= 0.3f;
            }
            
            y -= 0.1f;
            z -= 0.1f;
        }
    }
}
