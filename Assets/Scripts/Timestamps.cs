using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Timestamps : MonoBehaviour
{
    public Material white;
    public Material green;

    private GameObject activeTimestamp;

    public GameObject go;
    public List<GameObject> gos = new List<GameObject>();

    public List<GameObject> objs = new List<GameObject>();

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
        GameObject taskProjectionPlane = task.transform.parent.transform.parent.transform.parent.Find("TaskProjectionPlane").gameObject;
        taskProjectionPlane.GetComponent<TaskProjectionPlane>().SetTask(task);

        gos = taskProjectionPlane.GetComponent<TaskProjectionPlane>().gos;

        foreach (Transform child in taskProjectionPlane.transform)
        {
            objs.Clear();
            Destroy(child.gameObject);
        }

        System.Random r = new System.Random();

        if (gos.Count > 0)
        {
            Debug.Log("InsideStart Timestamps");
            

            int n = Random.Range(1, 5);
            for (int i = 0; i <= n; i++)
            {
                List<int> integers = new List<int>();
                go = gos[Random.Range(0, gos.Count)];
                go.transform.position = new Vector3(Random.Range(-5.00f, 5.00f), Random.Range(1.00f, 5.00f), Random.Range(-5.00f, 5.00f));
                objs.Add(go);
            }
        }

        

        foreach (GameObject ob in objs)
        {
            GameObject x = Instantiate(ob, taskProjectionPlane.transform);
            x.layer = 8;

            x.AddComponent<Interactable>();
            x.AddComponent<InteractableHoverEvents>();
            x.AddComponent<SimpleAttach>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Transform firstChild = this.transform.GetChild(0);
        activeTimestamp = firstChild.gameObject;
        firstChild.GetComponent<MeshRenderer>().material = green;

        
    }
}
