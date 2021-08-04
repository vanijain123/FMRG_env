using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timestamps : MonoBehaviour
{
    public Material white;
    public Material green;

    private GameObject activeTimestamp;

    public GameObject CurrentTimestamp()
    {
        return activeTimestamp;
    }

    public void SetTimestamp(GameObject ts)
    {
        activeTimestamp.GetComponent<MeshRenderer>().material = white;
        activeTimestamp = ts;
        activeTimestamp.GetComponent<MeshRenderer>().material = green;
        GameObject task = activeTimestamp.transform.parent.transform.parent.transform.parent.transform.parent.GetChild(0).gameObject;
        task.transform.parent.transform.parent.transform.parent.Find("TaskProjectionPlane").GetComponent<TaskProjectionPlane>().SetTask(task);
    }
    // Start is called before the first frame update
    void Start()
    {
        Transform firstChild = this.transform.GetChild(0);
        activeTimestamp = firstChild.gameObject;
        firstChild.GetComponent<MeshRenderer>().material = green;
    }
}
