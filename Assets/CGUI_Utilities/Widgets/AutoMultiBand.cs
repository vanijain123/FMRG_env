using System.Collections;
using System.Collections.Generic;
using CGUI_Utilities.Widgets;
using UnityEngine;

using System.Collections.Generic;

public class AutoMultiBand : MultiAnchorRubberBand
{
    public List<GameObject> ProvidedAnchors;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();

        //Create a dictionary here - refactor this
        Dictionary<string, GameObject> anchorsDict = new Dictionary<string, GameObject>();
        foreach (GameObject anchor in ProvidedAnchors)
        {
            anchorsDict.Add(anchor.name, anchor);
        }

        InitializeRBVisualization(anchorsDict, null, true);
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.E))
        {
            Dictionary<string, GameObject> anchorsDict = new Dictionary<string, GameObject>();
            foreach (GameObject anchor in ProvidedAnchors)
            {
                anchorsDict.Add(anchor.name, anchor);
            }
            ReAssignAnchors(anchorsDict);
        }
    }
}
