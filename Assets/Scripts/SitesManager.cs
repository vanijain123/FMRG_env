using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitesManager : MonoBehaviour
{
    public static SitesManager instance = null;
    public WIMPositions positions;
    public GameObject sites;
    public GameObject activatedTask = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //activatedTask = gameObject;
    }
}
