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
    public GameObject visibleSitesParent;

    public GameObject player;

    public bool singleWIM;

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
    }

    public void MakeSiteInvisible()
    {
        if (visibleTask != null)
        {
            visibleTask.transform.position = new Vector3(visibleTask.transform.position.x, -5, visibleTask.transform.position.z);
            ShowHelpers(false);
        }
        if (visibleTaskIconBackground != null)
        {
            visibleTaskIconBackground.SetActive(false);
        }
    }

    public void MakeSiteVisible()
    {
        Vector3 playerPos = playerCamera.transform.position;
        Vector3 playerDirection = playerCamera.transform.forward;
        Quaternion playerRotation = playerCamera.transform.rotation;
        float spawnDistance = 0.8f;

        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

        visibleTask.transform.position = spawnPos - new Vector3(0, spawnPos.y/3, 0);
        visibleTask.transform.eulerAngles = new Vector3(0, playerRotation.eulerAngles.y, 0);
        visibleTask.transform.localScale = new Vector3(1, 1, 1);

        ShowHelpers(true);
    }

    private void ShowHelpers(bool show)
    {
        List<GameObject> modelParts = visibleTask.GetComponent<ChildModels>().modelParts;
        for (int i = 0; i < modelParts.Count; i++)
        {
            if (modelParts[i].GetComponent<SimpleAttach>() != null)
            {
                if (show && modelParts[i].GetComponent<SimpleAttach>().isOutlined == true)
                {
                    modelParts[i].GetComponent<SimpleAttach>().isOutlined = false;
                    modelParts[i].GetComponent<Outline>().enabled = true;
                    modelParts[i].GetComponent<LineRenderer>().enabled = true;
                }
                else
                {
                    if (modelParts[i].GetComponent<Outline>().enabled == true)
                    {
                        modelParts[i].GetComponent<SimpleAttach>().isOutlined = true;
                        modelParts[i].GetComponent<Outline>().enabled = false;
                        modelParts[i].GetComponent<LineRenderer>().enabled = false;
                    }
                }
            }
        }
    }
}