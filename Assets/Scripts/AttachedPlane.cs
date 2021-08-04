using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedPlane : MonoBehaviour
{
    public GameObject plane;
    public GameObject projectionPlane;
    public GameObject teleportButton;

    private Material projectionTexture;
    //public GameObject teleportPlane;

    //private List<GameObject> replicas = new List<GameObject>();

    private void Start()
    {
        Debug.Log(plane.GetComponent<MeshRenderer>().sharedMaterial);
        this.GetComponent<MeshRenderer>().sharedMaterial = plane.GetComponent<MeshRenderer>().sharedMaterial;
        //teleportButton.GetComponent<TeleportButton>().currentPlane = plane;
        
    }

}
