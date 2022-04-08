using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    public GameObject siteIconTimeline;
    public float amount;

    private void Start()
    {
        amount = 0.005f;
    }

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
            Resize(amount, new Vector3(1, 0, 0), amount/2, new Vector3(0, 1, 0));
            siteIconTimeline.GetComponent<Timeline>().Resize(amount, new Vector3(1, 0, 0), amount/2, new Vector3(0, 1, 0));
        }
        amount = 0.005f;
    }
}
