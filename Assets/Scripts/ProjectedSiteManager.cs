using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ProjectedSiteManager : MonoBehaviour
{
    public SiteIconManager siteIconManager;
    public ManageInstructions manageInstructions;
    
    public GameObject plane;
    public Material lockedPlaneMaterial;
    public Material unlockedPlaneMaterial;

    public Recall recall;

    public class Instruction
    {
        public GameObject actionObject;
        public Vector3 localPosition;
        public Vector3 localScale;
        public Quaternion localRotation;
    }

    public List<Instruction> instructionCache = new List<Instruction>();

    private void Start()
    {
        if (SitesManager.instance.singleWIM)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, -5, gameObject.transform.position.z);
        }
    }

    public void addInstruction(GameObject g, Vector3 localPosition, Vector3 localScale, Quaternion localRotation)
    {
        Debug.Log("Adding instruction");
        Debug.Log($"{g.transform.localPosition}");
        instructionCache.Add(new Instruction { actionObject = g, localPosition = localPosition, localScale = localScale, localRotation = localRotation});
        Debug.Log($"Instruction added: {instructionCache.Count}");
    }

    public Tuple<GameObject, Vector3, Vector3, Quaternion> popInstruction()
    {
        Instruction i = new Instruction { actionObject=null, localPosition = new Vector3(0,0,0), localScale = new Vector3(0, 0, 0), localRotation = Quaternion.identity};
        if (instructionCache.Count > 0)
        {
            i = instructionCache[instructionCache.Count - 1];
            instructionCache.RemoveAt(instructionCache.Count - 1); 
        }
        return Tuple.Create(i.actionObject, i.localPosition, i.localScale, i.localRotation);
    }
}
