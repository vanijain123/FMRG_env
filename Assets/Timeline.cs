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

    public IEnumerator WaitCoroutine()
    {
        while (transform.localScale.y < 0.19f)
        {
            yield return null;
            Resize(0.001f, new Vector3(1, 0, 0), 0.0005f, new Vector3(0, 1, 0));
        }
    }
}
