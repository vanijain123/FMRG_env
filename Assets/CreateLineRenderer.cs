using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLineRenderer : MonoBehaviour
{
    private GameObject startGO;
    private GameObject endGo;
    private LineRenderer lr;

    // Start is called before the first frame update
    public void StartLR(GameObject startObject, GameObject endObject, Material m)
    {
        startGO = startObject;
        endGo = endObject;
        lr = startGO.AddComponent<LineRenderer>();
        lr.SetPosition(0, startGO.transform.localPosition);
        lr.SetPosition(1, endGo.transform.localPosition);
        lr.material = m;
        lr.startWidth = 1;
        lr.endWidth = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (lr != null)
        {
            lr.SetPosition(1, endGo.transform.localPosition);
        }
    }

    public void DestroyLR()
    {
        Destroy(lr);
    }
}
