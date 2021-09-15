using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelection : MonoBehaviour
{
    public GameObject player;

    private Vector3 redPlaneLocation;
    private Vector3 greenPlaneLocation;
    private Vector3 yellowPlaneLocation;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");

        redPlaneLocation = new Vector3(0, 0, -15f);
        greenPlaneLocation = new Vector3(0, 0, 0);
        yellowPlaneLocation = new Vector3(0, 0, 15f);

        Debug.Log("Debugging");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision with " + collision.gameObject.tag);
        if (collision.gameObject.tag == "redPlaneButton")
        {
            player.transform.position = redPlaneLocation;
        }
        else if (collision.gameObject.tag == "greenPlaneButton")
        {
            player.transform.position = greenPlaneLocation;
        }
        else if (collision.gameObject.tag == "yellowPlaneButton")
        {
            player.transform.position = yellowPlaneLocation;
        }
    }

}
