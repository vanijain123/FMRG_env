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
    private string grabbableTag = "Grabbable";

    void Start()
    {
        modelParts = ChildModels.modelParts;
    }

    public void ResetModelPositions()
    {
        Debug.Log("ResetModelPositions");
        //Check if an instruction set has been sent
        if (pm.manageInstructions.instructionsQueue.Count > 0)
        {
            Queue<GameObject[]> instructionSet = pm.manageInstructions.instructionsQueue.Dequeue();
            while (instructionSet.Count > 0)
            {
                GameObject[] instructions = instructionSet.Dequeue();
                GameObject movedObject = instructions[0].GetComponent<SimpleAttach>().movedObject;
                GameObject originalGO = instructions[1].GetComponent<SimpleAttach>().originalGO;

                if (movedObject != null && originalGO != null)
                {
                    movedObject.transform.localPosition = originalGO.transform.localPosition;
                    movedObject.transform.localScale = originalGO.transform.localScale;
                    movedObject.transform.localRotation = originalGO.transform.localRotation;
                    movedObject.tag = grabbableTag;
                    DestroyLR(movedObject);
                    //Destroy(movedObject.GetComponent<SimpleAttach>().lr);

                    instructions[0].GetComponent<SimpleAttach>().movedObject = null;
                    instructions[1].GetComponent<SimpleAttach>().originalGO = null;
                    Destroy(originalGO);
                }
            }
        }
        else
        {
            Debug.Log("Nothing to be undone");
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

                // Check if we have reset the object from its original position
                if (actionObject == modelParts[i] 
                    && t.Item2 == originalGO.transform.localPosition
                    && t.Item3 == originalGO.transform.localScale
                    && t.Item4 == originalGO.transform.localRotation)
                {
                    modelParts[i].GetComponent<SimpleAttach>().movedObject = null;
                    modelParts[i].GetComponent<SimpleAttach>().originalGO = null;
                    actionObject.GetComponent<Outline>().enabled = false;
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

            if (modelParts[i].GetComponent<SimpleAttach>().instructions[0] == null)
            {
                GameObject originalGO = modelParts[i].GetComponent<SimpleAttach>().originalGO;
                modelParts[i].GetComponent<SimpleAttach>().SetMaterial(modelParts[i], modelParts[i].GetComponent<SimpleAttach>().originalMaterial, true);
                if (modelParts[i].GetComponent<Outline>() != null)
                {
                    modelParts[i].GetComponent<Outline>().enabled = false;
                }

                // Add conditions to check if robot has performed the task
                Debug.Log($"modelParts[i].GetComponent<SimpleAttach>().movedObject {modelParts[i].GetComponent<SimpleAttach>().movedObject}");
                if (modelParts[i].GetComponent<SimpleAttach>().movedObject != null)
                {
                    modelParts[i].GetComponent<SimpleAttach>().movedObject.tag = grabbableTag;
                }
                DestroyLR(modelParts[i]);
                modelParts[i].GetComponent<SimpleAttach>().movedObject = null;
                modelParts[i].GetComponent<SimpleAttach>().originalGO = null;
                Destroy(originalGO);
            }
        }
    }
    
    private void DestroyLR(GameObject go)
    {
        if (go.GetComponent<SimpleAttach>().lr != null)
        {
            Destroy(go.GetComponent<SimpleAttach>().lr);
        }
    }
}
