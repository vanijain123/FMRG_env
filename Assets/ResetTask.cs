using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTask : MonoBehaviour
{
    public ProjectedSiteManager pm;
    public ChildModels ChildModels;
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
        Debug.Log($"Vani {actionObject}");
        if (actionObject != null)
        {
            Debug.Log($"Vani insice if");
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
