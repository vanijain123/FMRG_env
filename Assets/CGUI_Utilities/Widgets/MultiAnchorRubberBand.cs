// Copyright (c) 2017, Columbia University 
// All rights reserved. 
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of Columbia University nor the names of its 
//    contributors may be used to endorse or promote products derived from 
//    this software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 
// 
// =============================================================
// Authors: 
// Carmine Elvezio, Mengu Sukan, Steven Feiner
// =============================================================
// 
// 

using System.Collections.Generic;
using UnityEngine;

namespace CGUI_Utilities.Widgets
{
    /// <summary>
    /// Simple, multi-endpoint rubber band visualization with
    /// GameObject endpoints.
    /// </summary>
    public class MultiAnchorRubberBand : MultiAnchorVisualization
    {
        /// <summary>
        /// Gets or sets the rubber band's line renderer.
        /// </summary>
        /// <value>The rubber band line renderer.</value>
        public LineRenderer RBLineRenderer;

        /// <summary>
        /// Gets or sets the rubber band's material.
        /// </summary>
        /// <value>The rubber band material.</value>
        public Material RBMaterial;

        /// <summary>
        /// Default width used for all endpoints of the rubberband
        /// </summary>
        public float RBDefaultLineWidth;

        /// <summary>
        /// Gets or sets the default endpoint scale.
        /// </summary>
        /// <value>The endpoint scale.</value>
        public Vector3 RBDefaultEndpointScale;

        /// <summary>
        /// Gets or sets the default endpoint material.
        /// </summary>
        /// <value>The default endpoint mat.</value>
        public Material RBDefaultEndpointMat;

        /// <summary>
        /// The default GameObject to use when no object list is provided.
        /// </summary>
        public GameObject RBDefaultGameObject;

        /// <summary>
        /// List of endpoint shapes usable to help define the ends of the rubber band
        /// </summary>
        public List<GameObject> EndpointShapes;
        
        public override void Awake()
        {
            base.Awake();
        }

        public virtual void InitializeRBVisualization(
            Dictionary<string, GameObject> anchors,
            List<GameObject> endpointShapes,
            bool anchorAutoAttach = true)
        {
            AnchorAutoAttach = anchorAutoAttach;

            AddEndpoints(anchors, endpointShapes);

            CreateRubberBandLineRenderer();
        }

        public virtual void InitializeRBVisualization(
            Dictionary<string, AnchorableObject> anchors,
            List<GameObject> endpointShapes,
            bool anchorAutoAttach = true)
        {
            AnchorAutoAttach = anchorAutoAttach;

            CreateRubberBandLineRenderer();
        }

        /// <summary>
        /// Update this instance.
        /// </summary>
        public override void Update()
        {
            UpdateAnchors();

            base.Update();
        }

        /// <summary>
        /// Updates the anchors.
        /// </summary>
        protected override void UpdateAnchors()
        {
            base.UpdateAnchors ();

            int i = 0;
            foreach(var endpoint in Anchors.Values)
            {
                RBLineRenderer.SetPosition (i, endpoint.VisualRepresentation.transform.position);
                i++;
            }
        }

        /// <summary>
        /// Add endpoint objects to the visualization base as gameobject dictionary
        /// </summary>
        /// <param name="endpointObjs"></param>
        protected virtual void AddEndpoints(Dictionary<string, GameObject> anchorObjs, 
            List<GameObject> endpointShapes = null)
        {
            if (endpointShapes == null)
            {
                endpointShapes = new List<GameObject>();
                endpointShapes.Add(RBDefaultGameObject);
            }

            //Todo: Figure out whats happening here - Why is there a second object still being created
            int endpointsIndex = 0;
            foreach (var anchor in anchorObjs)
            {
                AnchorableObject storedValue;
                Anchors.TryGetValue(anchor.Key, out storedValue);

                if (!storedValue)
                {
                    GameObject endpointShape = Instantiate(endpointShapes[endpointsIndex % endpointShapes.Count]);
                    endpointsIndex++;

                    //Assign the scale based on default.
                    endpointShape.transform.localScale = RBDefaultEndpointScale;

                    //Assign the material
                    if (RBDefaultEndpointMat != null)
                        endpointShape.GetComponent<MeshRenderer>().material =
                            RBDefaultEndpointMat;

                    AddAnchor(anchor.Key, anchor.Value, endpointShape);
                }
            }
        }

        /// <summary>
        /// Instantiate a rubber band based on current set parameters
        /// </summary>
        protected virtual void CreateRubberBandLineRenderer()
        {
            RBLineRenderer =
                GetComponent<LineRenderer>() ? GetComponent<LineRenderer>() :
                    gameObject.AddComponent<LineRenderer>();

            if (RBMaterial != null)
            {
                RBLineRenderer.material = RBMaterial;
            }
            
            RBLineRenderer.startWidth = RBDefaultLineWidth;
            RBLineRenderer.endWidth = RBDefaultLineWidth;
            RBLineRenderer.positionCount = Anchors.Count;
        }
        
        public virtual void ReAssignAnchors(
            Dictionary<string, GameObject> newAnchors, 
            bool destroyCurrentVisualRepresentation = false)
        {
            //Todo: Are the anchors not being moved?
            ClearAnchors(destroyCurrentVisualRepresentation);
            AddEndpoints(newAnchors);
        }
    }
}
