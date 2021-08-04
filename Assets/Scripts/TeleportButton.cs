using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportButton : MonoBehaviour
{
    public GameObject currentPlane;
    public GameObject nextPlane;
    

    private void Start()
    {
        currentPlane = GameObject.Find("Player").GetComponent<PointerHandler>().greenPlane;
        nextPlane = currentPlane;
    }
}
