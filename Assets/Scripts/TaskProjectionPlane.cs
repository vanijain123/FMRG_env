using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskProjectionPlane : MonoBehaviour
{
    private GameObject currentTask;

    // Start is called before the first frame update
    void Start()
    {
        SetTask(this.transform.parent.Find("Tasks").GetChild(0).GetChild(0).gameObject);
    }

    public void SetTask(GameObject newTask)
    {
        if (currentTask)
        {
            currentTask.transform.Find("SelectedTaskHighlight").GetComponent<MeshRenderer>().enabled = false;
        }
        currentTask = newTask;
        currentTask.transform.Find("SelectedTaskHighlight").GetComponent<MeshRenderer>().enabled = true;
        this.GetComponent<MeshRenderer>().material = currentTask.GetComponent<MeshRenderer>().material;
    }
}
