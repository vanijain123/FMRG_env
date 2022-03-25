using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationButton : MonoBehaviour
{

    public GameObject objectToMove;

    private Vector3 up = new Vector3(0, 0.25f, 0);
    private Vector3 down = new Vector3(0, -0.25f, 0);
    private Vector3 left = new Vector3(0.25f, 0, 0);
    private Vector3 right = new Vector3(-0.25f, 0, 0);

    public void MoveParent(string direction)
    {
        Vector3 move = new Vector3(0, 0, 0);
        switch (direction)
        {
            case "up":
                move = up;
                break;
            case "down":
                move = down;
                break;
            case "left":
                move = left;
                break;
            case "right":
                move = right;
                break;
        }
        objectToMove.transform.localPosition += move;
    }
}
