using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Recall : MonoBehaviour
{
    public SteamVR_Action_Boolean showRecall;
    public SteamVR_Input_Sources handType;

    public GameObject recallGameobject;
    private int waitSeconds;

    private void Start()
    {
        showRecall.AddOnStateDownListener(TriggerDown, handType);
    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("X Down");
        EnableRecallGameobject(10);
    }

    public void EnableRecallGameobject(int seconds)
    {
        waitSeconds = seconds;
        StartCoroutine("ShowRecall");
    }

    IEnumerable ShowRecall()
    {
        recallGameobject.SetActive(true);
        yield return new WaitForSeconds(10);
        recallGameobject.SetActive(false);
    }
}
