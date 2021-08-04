using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using TMPro;

public class ProjectionPlane : MonoBehaviour
{
    public GameObject plane;
    

    private Material projectionTexture;
    
    //public GameObject teleportPlane;

    private List<GameObject> replicas = new List<GameObject>();
    private GameObject planeInfoText;

    private void Start()
    {
        Debug.Log(plane.GetComponent<MeshRenderer>().sharedMaterial);
        this.GetComponent<MeshRenderer>().sharedMaterial = plane.GetComponent<MeshRenderer>().sharedMaterial;
        planeInfoText = this.transform.parent.Find("PlaneInfo").Find("PlaneInfoText").gameObject;
        CreateMiniWorld(plane);
    }

    

    private void DestroyReplicas()
    {
        List<GameObject> temp = new List<GameObject>(replicas);
        int count = 0;
        foreach (GameObject x in temp)
        {
            Debug.Log(replicas[count]);
            Destroy(replicas[count]);
            replicas.Remove(replicas[count]);
        }
    }

    public void CreateMiniWorld(GameObject plane)
    {
        //displayPlaneInfo(plane);

        DestroyReplicas();
        this.GetComponent<MeshRenderer>().sharedMaterial = plane.GetComponent<MeshRenderer>().sharedMaterial;
        foreach (Transform tr in plane.transform)
        {
            if (tr.tag == "replicable")
            {
                replicas.Add(Instantiate(tr.gameObject, this.transform) as GameObject);
            }
        }

        foreach (GameObject x in replicas)
        {
            Debug.Log(x.name);

            x.transform.SetParent(this.transform);
            x.layer = 8;

            x.AddComponent<Interactable>();
            x.AddComponent<InteractableHoverEvents>();
            x.AddComponent<SimpleAttach>();
        }
    }

    public void displayPlaneInfo(GameObject plane)
    {
        Debug.Log(planeInfoText.name);
        Debug.Log(planeInfoText.GetComponent<TMPro.TextMeshProUGUI>());

        string text = plane.GetComponent<PlaneInfo>().info;
        string[] lines = text.Split('-');
        Debug.Log(lines[0]);
        string new_text = string.Join("\n", lines);
        Debug.Log(new_text);
        planeInfoText.GetComponent<TMPro.TextMeshPro>().text = new_text;
    }
}
