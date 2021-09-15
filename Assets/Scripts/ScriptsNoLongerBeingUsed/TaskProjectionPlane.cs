using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskProjectionPlane : MonoBehaviour
{
    //private GameObject currentTask;

    //public List<GameObject> gos = new List<GameObject>();
    //public GameObject mainPlane;

    //public GameObject tasks;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    SetTask(this.transform.parent.Find("Tasks").GetChild(0).GetChild(0).gameObject);
    //}

    //public void SetTask(GameObject newTask)
    //{
    //    if (currentTask)
    //    {
    //        currentTask.transform.Find("SelectedTaskHighlight").GetComponent<MeshRenderer>().enabled = false;
    //    }
    //    currentTask = newTask;
    //    currentTask.transform.Find("SelectedTaskHighlight").GetComponent<MeshRenderer>().enabled = true;
    //    this.GetComponent<MeshRenderer>().material = currentTask.GetComponent<MeshRenderer>().material;
    //}

    //public void TeleportToTask()
    //{
    //    mainPlane.GetComponent<MeshRenderer>().material = currentTask.GetComponent<MeshRenderer>().material;
    //    if (mainPlane.transform.childCount > 0)
    //    {
    //        foreach (Transform child in mainPlane.transform)
    //        {
    //            if(child.name != "Plane")
    //            Destroy(child.gameObject);
    //        }
    //    }
    //    foreach (Transform child in this.transform)
    //    {
    //        Instantiate(child.gameObject, mainPlane.transform);
    //    }
    //}

    //public void FindTask(int index)
    //{
    //    GameObject task = transform.parent.Find("Tasks").GetChild(index).gameObject;
    //    SetTask(task);
    //}
}