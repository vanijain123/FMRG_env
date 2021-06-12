using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FadeTest : MonoBehaviour
{

    private SteamVR_Fade f;

    // Start is called before the first frame update
    void Start()
    {
        f = GetComponent<SteamVR_Fade>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeMethod()
    {
        Debug.Log("Fade Method Called");
        SteamVR_Fade.Start(Color.black, 0);
        SteamVR_Fade.Start(Color.clear, 2);
    }
}
