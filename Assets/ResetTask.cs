using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTask : MonoBehaviour
{
    public ProjectedSiteManager pm;
    public ChildModels ChildModels;
    public Material objectMaterial;

    private List<GameObject> modelParts;

    public void ResetModelPositions()
    {
        for (int i=0; i<modelParts.Count; i++)
        {
            GameObject movedObject = modelParts[i].GetComponent<SimpleAttach>().movedObject;
            GameObject originalGO = modelParts[i].GetComponent<SimpleAttach>().originalGO;

            if (movedObject!=null && originalGO != null)
            {
                movedObject.transform.localPosition = originalGO.transform.localPosition;
                movedObject.transform.localScale = originalGO.transform.localScale;
                movedObject.transform.localRotation = originalGO.transform.localRotation;
            }
            modelParts[i].GetComponent<SimpleAttach>().movedObject = null;
            modelParts[i].GetComponent<SimpleAttach>().originalGO = null;
            Destroy(originalGO);
        }
    }

    public void undo()
    {
        Tuple<GameObject, Vector3, Vector3, Quaternion> t = pm.popInstruction();
        GameObject actionObject = t.Item1;
        if (actionObject != null)
        {
            Debug.Log($"{actionObject.transform.localPosition}");
            actionObject.transform.localPosition = t.Item2;
            actionObject.transform.localScale = t.Item3;
            actionObject.transform.localRotation = t.Item4;

            for (int i = 0; i < modelParts.Count; i++)
            {
                GameObject originalGO = modelParts[i].GetComponent<SimpleAttach>().originalGO;
                if (actionObject == modelParts[i] 
                    && t.Item2 == originalGO.transform.localPosition
                    && t.Item3 == originalGO.transform.localScale
                    && t.Item4 == originalGO.transform.localRotation)
                {
                    modelParts[i].GetComponent<SimpleAttach>().movedObject = null;
                    modelParts[i].GetComponent<SimpleAttach>().originalGO = null;
                    Destroy(originalGO);
                }
            } 
        }
        else
        {
            ResetModelPositions();
        }
    }

    public void ReevaluateTask()
    {
        pm.instructionCache.Clear();
        for (int i = 0; i < modelParts.Count; i++)
        {
            //GameObject movedObject = modelParts[i].GetComponent<SimpleAttach>().movedObject;
            GameObject originalGO = modelParts[i].GetComponent<SimpleAttach>().originalGO;
            modelParts[i].GetComponent<SimpleAttach>().SetMaterial(modelParts[i], objectMaterial);

            // Add conditions to check if robot has performed the task

            modelParts[i].GetComponent<SimpleAttach>().movedObject = null;
            modelParts[i].GetComponent<SimpleAttach>().originalGO = null;
            Destroy(originalGO);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        modelParts = ChildModels.modelParts;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
