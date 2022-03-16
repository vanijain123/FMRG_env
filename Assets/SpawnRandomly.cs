using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomly : MonoBehaviour
{
    public Transform spawnPoints;
    public List<GameObject> modelParts;

    private List<List<Transform>> spawnPointsList = new List<List<Transform>>();
    private List<int> usedQuadrants = new List<int>();
    //private int[] possibleQuadrants = new int[] {0, 1, 2, 3, 4};
    
    private void Start()
    {
        GetSpawnPoints();
        SpawnParts();
    }

    private void GetSpawnPoints()
    {
        foreach(Transform quadrant in spawnPoints)
        {
            List<Transform> points = new List<Transform>();
            foreach(Transform child in quadrant)
            {
                points.Add(child);
            }
            spawnPointsList.Add(points);
        }
    }

    private void SpawnParts()
    {
        for (int i=0; i<modelParts.Count; i++)
        {
            Debug.Log("Spawning");
            Random r = new Random();
            int q = Random.Range(0, 5);
            while (usedQuadrants.Contains(q))
            {
                q = Random.Range(0, 5);
                Debug.Log($"Changing q: {q}");
            }
            usedQuadrants.Add(q);
            Debug.Log($"q: {q}");
            int p = Random.Range(0, 4);
            Debug.Log($"p: {p}");
            modelParts[i].transform.position = spawnPointsList[q][p].position;
            modelParts[i].transform.localRotation = spawnPointsList[q][p].localRotation;
        }
    }
}
