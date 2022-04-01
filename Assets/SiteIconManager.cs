using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiteIconManager : MonoBehaviour
{
    public GameObject siteIconTimeline;
    public GameObject siteIcon;
    public Material green;
    public Material white;

    private void Start()
    {
        white = siteIcon.GetComponent<MeshRenderer>().material;
    }
}
