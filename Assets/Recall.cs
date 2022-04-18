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
        showRecall.AddOnStateUpListener(TriggerUp, handType);
    }

    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("X Down");
        recallGameobject.SetActive(true);
        //EnableRecallGameobject(10);
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("X Up");
        recallGameobject.SetActive(false);
    }

    public void EnableRecallGameobject(int seconds)
    {
        Debug.Log("EnableRecallGameobject");
        waitSeconds = seconds;
        StartCoroutine(ShowRecall());
    }

    IEnumerator ShowRecall()
    {
        Debug.Log("Inside coroutine");
        recallGameobject.SetActive(true);
        yield return new WaitForSeconds(10);
        recallGameobject.SetActive(false);
    }
}
