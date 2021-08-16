using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskProjectionPlane : MonoBehaviour
{
    private GameObject currentTask;

    public List<GameObject> gos = new List<GameObject>();

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

    public void FindTask(int index)
    {
        GameObject task = transform.parent.Find("Tasks").GetChild(index).gameObject;
        SetTask(task);
    }
}
