using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSites : MonoBehaviour
{
    public List<GameObject> sites;

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i<sites.Count; i++)
        {
            List<GameObject> models = sites[i].GetComponent<ChildModels>().modelParts;
            for (int j=0; j<models.Count; j++)
            {
                models[j].transform.localPosition = new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(0.1f, 0.4f), Random.Range(-0.4f, 0.4f));
                models[j].transform.rotation = Random.rotation;
            }
            sites[i].SetActive(false);
        }
    }
}
