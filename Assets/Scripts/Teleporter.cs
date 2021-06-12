using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Teleporter : MonoBehaviour
{
    public GameObject m_pointer;
    public SteamVR_Action_Boolean m_teleportAction;

    private SteamVR_Behaviour_Pose m_Pose = null;
    private bool m_HasPosition = false;

    private void Awake()
    {
        m_Pose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    // Update is called once per frame
    void Update()
    {
        //Pointer
        m_HasPosition = UpdatePointer();
        m_pointer.SetActive(m_HasPosition);
    }

    private void TryTeleport()
    {
        if (m_teleportAction.GetLastStateUp(m_Pose.inputSource))
            TryTeleport();
    }

    private IEnumerator MoveRig(Transform cameraRig, Vector3 translation)
    {
        yield return null;
    }

    private bool UpdatePointer()
    {
        // Ray from the controller
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // If it's a hit
        if(Physics.Raycast(ray, out hit))
        {
            m_pointer.transform.position = hit.point;
            return true;
        }

        // If not a hit

        return false;
    }
}
