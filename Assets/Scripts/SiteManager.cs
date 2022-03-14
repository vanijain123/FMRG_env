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

    //OLD
    //private Transform projectionComponents;
    //private GameObject projectedComponents;

    //private GameObject cylinder;
    //private GameObject cube;
    //private GameObject sphere;

    //private Material red;
    //private Material blue;
    //private Material green;

    private void Start()
    {
        //projectionComponents = this.transform.parent.parent.parent.parent.Find("ProjectionGameObject");
        this.transform.Find("Text").GetComponent<TextMeshPro>().SetText(siteName);
    }

    public void ProjectSelectedSite()
    {
        bool placed = false;
        List<Transform> wimPositions = transform.parent.parent.GetComponent<WIMPositions>().positions;
        List<bool> available = transform.parent.parent.GetComponent<WIMPositions>().available;
        for (int i=0; i<wimPositions.Count; i++)
        {
            if (available[i])
            {
                projectionSite.SetActive(true);
                projectionSite.transform.localPosition = wimPositions[i].localPosition;
                placed = true;
                available[i] = false;
                Debug.Log(transform.parent.parent.GetComponent<WIMPositions>().available[i]);
                break;
            }
        }

    }

    public void ProjectSelectedSite(GameObject cube, GameObject cylinder, GameObject sphere)
    {
        //NEW ENABLING THE ATTACHED SITE
        projectionSite.SetActive(true);
        
        //OLD CREATING A NEW SITE
        //projectedComponents = Instantiate(projectionSite, projectionComponents);
        //projectedComponents.transform.Find("Site").Find("Text").GetComponent<TextMeshPro>().SetText(siteName);
        //projectedComponents.GetComponent<ProjectedSiteManager>().SetMenuSite(this.gameObject);
        //createMiniWorldObjects(projectedComponents, cube, cylinder, sphere);
    }

    public void DeleteProjectedSite()
    {
        //NEW DEACTIVATE SITE
        projectionSite.SetActive(false);

        //OLD DELETE SITE
        //Destroy(projectedComponents);
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
            //g.GetComponent<MeshRenderer>().material.
            g.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            g.transform.localPosition = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(0.1f, 0.4f), Random.Range(-0.4f, 0.4f));
            g.layer = 8;

            g.AddComponent<Interactable>();
            g.AddComponent<InteractableHoverEvents>();
            g.AddComponent<SimpleAttach>();
        }
    }
}
