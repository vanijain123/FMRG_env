using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManageInstructions : MonoBehaviour
{
    public ChildModels childModels;
    public TextMeshPro text;
    public Material instructionsSentMaterial;

    private List<GameObject> children;
    private bool instructionSent;


    // Start is called before the first frame update
    void Start()
    {
        children = childModels.modelParts;
        instructionSent = false;
    }

    public void buttonInteraction()
    {
        if (instructionSent == true)
        {
            Debug.Log("buttonInteraction already pressed");
            text.text = "Task In Progress";
        }
        else
        {
            Debug.Log("buttonInteraction");
            for (int i = 0; i < children.Count; i++)
            {
                GameObject[] instructions = children[i].GetComponent<SimpleAttach>().instructions;
                if (instructions[0] != null && instructions[1] != null)
                {
                    if (instructions[0].transform != instructions[1].transform)
                    {
                        SetMaterial(children[i], instructionsSentMaterial);
                        instructions[0] = null;
                        instructions[1] = null;
                    }
                }
            }
            text.text = "Task In Progress";
        }
    }

    private void SetMaterial(GameObject go, Material m)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            go.transform.GetChild(i).GetComponent<MeshRenderer>().material = m;
        }
    }

}
