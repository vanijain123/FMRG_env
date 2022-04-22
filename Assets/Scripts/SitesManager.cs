using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve;

public class SitesManager : MonoBehaviour
{
    public static SitesManager instance = null;
    public WIMPositions positions;
    public GameObject sites;
    public GameObject activatedTask = null;
    public GameObject menuParent;

    public GameObject visibleTask;
    private Vector3 visibleTaskScale;

    public GameObject invisibleSitesParent;
    public GameObject visibleSitesParent;

    private Transform playerCamera;
    private Transform player;

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
    }

    private void Start()
    {
        playerCamera = GameObject.Find("VRCamera").transform;
        player = GameObject.Find("Player").transform;
        StartCoroutine("MoveSiteToVisibleParentCoroutine");
    }

    IEnumerable MoveSiteToVisibleParentCoroutine()
    {
        yield return null;
        if (visibleTask != null)
        {
            MoveSiteToVisibleParent();
        }
        menuParent.SetActive(false);
    }

    public void MoveSiteToInvisibleParent()
    {
        if (visibleTask != null)
        {
            visibleTask.transform.parent = invisibleSitesParent.transform;
        }
    }

    public void MoveSiteToVisibleParent()
    {
        Vector3 playerPos = playerCamera.transform.position;
        Vector3 playerDirection = playerCamera.transform.forward;
        Quaternion playerRotation = playerCamera.transform.rotation;
        float spawnDistance = 1;

        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

        visibleTask.transform.parent = visibleSitesParent.transform;
        visibleTask.transform.position = spawnPos - new Vector3(0, spawnPos.y/3, 0);
        visibleTask.transform.eulerAngles = new Vector3(0, playerRotation.eulerAngles.y, 0);
        visibleTask.transform.localScale = new Vector3(1, 1, 1);
    }
}