using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTask : MonoBehaviour
{
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
