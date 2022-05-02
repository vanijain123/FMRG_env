using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve;

public class SitesManager : MonoBehaviour
{
    public static SitesManager instance = null;
    public GameObject sites;
    public GameObject activatedTask = null;
    public GameObject menuParent;

    public GameObject visibleTask;
    public GameObject visibleTaskIconBackground;
    public GameObject visibleSitesParent;
    public GameObject[] siteGameobjects;

    public GameObject player;

    public bool singleWIM;
    private Transform[] originalHiddenSiteTransforms = new Transform[4];
    private Transform[] originalVisibleSiteTransforms = new Transform[4];

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
        for (int i = 0; i < siteGameobjects.Length; i++)
        {
            originalHiddenSiteTransforms[i].position = new Vector3(siteGameobjects[i].transform.position.x, -5, siteGameobjects[i].transform.position.z);
            //originalHiddenSiteTransforms[i].position = new Vector3(originalHiddenSiteTransforms[i].position.x, -5, originalHiddenSiteTransforms[i].position.z);
            originalVisibleSiteTransforms[i].position = new Vector3(siteGameobjects[i].transform.position.x, 0.5f, siteGameobjects[i].transform.position.z);
        }
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

    private void ResetWIMPositions(Transform[] wimPositions)
    {
        for (int i = 0; i < siteGameobjects.Length; i++)
        {
            siteGameobjects[i].transform.position = wimPositions[i].position;
            siteGameobjects[i].transform.rotation = wimPositions[i].rotation;
        }
    }

    public void SwitchToMultipleWIM()
    {
        singleWIM = false;
        ResetWIMPositions(originalVisibleSiteTransforms);
    }

    public void SwitchToSingleWIM()
    {
        singleWIM = true;
        ResetWIMPositions(originalHiddenSiteTransforms);
    }
}