using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    public void Resize(float amount, Vector3 direction, float scaleAmount, Vector3 scaleDirection)
    {
            transform.localPosition += direction * amount / 2; // Move the object in the direction of scaling, so that the corner on ther side stays in place
            transform.localScale += scaleDirection * scaleAmount; // Scale object in the specified direction
    }

    private void Update()
    {
    }
}
