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
        if (visibleTask != null)
        {
            MoveSiteToVisibleParent();
        }
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
        Vector3 playerPos = player.transform.position;
        Vector3 playerDirection = playerCamera.transform.forward;
        Debug.Log($"playerDirection {playerDirection}");
        Quaternion playerRotation = playerCamera.transform.rotation;
        float spawnDistance = 1;

        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

        visibleTask.transform.parent = visibleSitesParent.transform;
        //visibleTask.transform.position = playerCamera.position + new Vector3(0, -playerCamera.position.y/2, 0.8f);
        visibleTask.transform.position = spawnPos + new Vector3(0, -playerCamera.position.y / 2, 0.8f);
        visibleTask.transform.rotation = playerRotation;
        //visibleTask.transform.rotation = Quaternion.Euler(new Vector3(0, playerRotation.y, 0));
        visibleTask.transform.localScale = new Vector3(1, 1, 1);
    }
}