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
    public GameObject visibleTaskIconBackground;
    public GameObject invisibleSitesParent;
    public GameObject visibleSitesParent;

    private Transform playerCamera;

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
        if (visibleTaskIconBackground != null)
        {
            visibleTaskIconBackground.SetActive(false);
        }
    }

    public void MoveSiteToVisibleParent()
    {
        Vector3 playerPos = playerCamera.transform.position;
        Vector3 playerDirection = playerCamera.transform.forward;
        Quaternion playerRotation = playerCamera.transform.rotation;
        float spawnDistance = 0.8f;

        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

        visibleTask.transform.parent = visibleSitesParent.transform;
        visibleTask.transform.position = spawnPos - new Vector3(0, spawnPos.y/3, 0);
        visibleTask.transform.eulerAngles = new Vector3(0, playerRotation.eulerAngles.y, 0);
        visibleTask.transform.localScale = new Vector3(1, 1, 1);

        visibleTaskIconBackground.SetActive(true);
    }
}