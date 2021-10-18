using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Valve.VR.InteractionSystem;

public class SiteManager : MonoBehaviour
{
    //public GameObject taskGroup;
    //public GameObject ts;

    //public int siteNumber;

    //private List<GameObject> tasks = new List<GameObject>();

    //private GameObject firstTask;
    //private GameObject mainPlane;

    public GameObject projectionSite;
    public string siteName;

    private Transform projectionComponents;
    private GameObject projectedComponents;

    private GameObject cylinder;
    private GameObject cube;
    private GameObject sphere;

    private Material red;
    private Material blue;
    private Material green;

    private void Start()
    {
        projectionComponents = this.transform.parent.parent.parent.parent.Find("ProjectionGameObject");
        this.transform.Find("Text").GetComponent<TextMeshPro>().SetText(siteName);
    }

    // Unused code
    // Start is called before the first frame update
    //void Awake()
    //{
    //    Transform t = transform.parent.transform.Find("Tasks");
    //    foreach (Transform tr in t)
    //    {
    //        tasks.Add(tr.gameObject);
    //        //Debug.Log(tasks);
    //    }

    //    firstTask = tasks[0].gameObject;


    //    projectionComponents = transform.parent.transform.parent.transform.parent.Find("ProjectionComponents").gameObject;
    //    //SelectSite();

    //    mainPlane = transform.parent.transform.parent.GetComponent<AllSitesManager>().mainPlane;
    //}

    //public void SelectSite(int index = 0)
    //{
    //    transform.parent.transform.parent.GetComponent<AllSitesManager>().UpdateActiveSite(this.gameObject);
    //    Debug.Log(index);
    //    //projectionComponents.GetComponent<TaskProjectionPlane>().FindTask(index);
    //    ProjectSite();
    //}

    //public void UnselectSite()
    //{
    //    transform.Find("SelectedTaskHighlight").GetComponent<MeshRenderer>().enabled = false;
    //    Transform t = projectionComponents.transform.Find("Tasks");
    //    foreach (Transform tr in t)
    //    {
    //        Destroy(tr.gameObject);
    //    }
    //}

    //public void ProjectSite()
    //{
    //    float x = -0.2f, y = 0, z = 0;
    //    foreach (GameObject task in tasks)
    //    {
    //        Transform parent = this.transform.parent.transform.parent.transform.parent.Find("ProjectionComponents").Find("Tasks");
    //        GameObject t = Instantiate(taskGroup, parent);
    //        t.transform.Find("Task").GetComponent<MeshRenderer>().material = task.GetComponent<MeshRenderer>().material;
    //        t.transform.localPosition = new Vector3(x, y, z);

    //        int steps = task.GetComponent<Task>().steps;

    //        GameObject timestampParent = t.transform.Find("Timeline").Find("Cylinder").Find("Timestamps").gameObject;
    //        //Debug.Log(timestampParent.GetComponent<MeshRenderer>().bounds);

    //        float a = 0, b = 1.0f, c = 0;

    //        for (int i=0; i<steps; i++)
    //        {
    //            GameObject temp = Instantiate(ts, timestampParent.transform);
    //            temp.transform.localPosition = new Vector3(a, b, c);
    //            b -= 0.3f;
    //        }

    //        y -= 0.1f;
    //        z -= 0.1f;
    //    }
    //}

    public void ProjectSelectedSite(GameObject cube, GameObject cylinder, GameObject sphere)
    {
        projectedComponents = Instantiate(projectionSite, projectionComponents);
        //Debug.Log(projectedComponents.transform.Find("Site").Find("Text").GetComponent<TextMeshPro>().GetParsedText());
        projectedComponents.transform.Find("Site").Find("Text").GetComponent<TextMeshPro>().SetText(siteName);
        projectedComponents.GetComponent<ProjectedSiteManager>().SetMenuSite(this.gameObject);
        createMiniWorldObjects(projectedComponents, cube, cylinder, sphere);
    }

    public void DeleteProjectedSite()
    {
        Destroy(projectedComponents);
    }

    private void createMiniWorldObjects(GameObject projectedComponents, GameObject cube, GameObject cylinder, GameObject sphere)
    {
        //cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        //sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        
        // Create a list of the primitives to choose from
        List<GameObject> primitives = new List<GameObject>();
        primitives.Add(cube);
        primitives.Add(cylinder);
        primitives.Add(sphere);

        // Create a shader
        //Material shader = new Material(Shader.Find("Unlit/Color"));
        Material shader = new Material(Shader.Find("Standard"));

        // Create possible colors
        Color red = new Color(255, 0, 0, 255);
        Color green = new Color(0, 255, 0, 255);
        Color blue = new Color(0, 0, 255, 255);

        // Create a list of colors to choose from
        List<Color> colors = new List<Color>();
        colors.Add(red);
        colors.Add(green);
        colors.Add(blue);

        // Number of objects in the world
        int n = Random.Range(1, 5);

        for(int i = 0; i<n; i++)
        {
            GameObject g = Instantiate(primitives[Random.Range(0, 2)], projectedComponents.transform.Find("ProjectedObjects").transform);
            g.GetComponent<MeshRenderer>().material.SetColor("_Color", colors[Random.Range(0, 2)]);
            g.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            g.transform.localPosition = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(0.1f, 0.4f), Random.Range(-0.4f, 0.4f));
            g.layer = 8;

            g.AddComponent<Interactable>();
            g.AddComponent<InteractableHoverEvents>();
            g.AddComponent<SimpleAttach>();
        }
    }
}
