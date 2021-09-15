using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{

    public int steps;

    private List<float> timestamps = new List<float>(); 

    public int GetSteps()
    {
        return steps;
    }
}
