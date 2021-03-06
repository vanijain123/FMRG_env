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

using MercuryMessaging;
using MercuryMessaging.Support.Extensions;
using UnityEngine;

namespace CGUI_Utilities.Widgets
{
    public class PlaceObject : MmBaseResponder
    {
        public Transform Object;
        public Vector3 Position;
        public Quaternion Rotation;

        public Vector3 origPosition;
        public Quaternion origRotation;

		public bool ReturnObjectToOrig = true;
		public bool AutoSetInActivate = true;

		public bool setPos = true;
		public bool setRot = true;

		public bool useGlobalPos = true;


        public override void SetActive(bool active)
        {
            base.SetActive(active);
            
			if (AutoSetInActivate) {
				SetPosRot (active);
			}       
        }

		public void SetPosRot(bool active)
		{	
			if (!active && ReturnObjectToOrig) {
				
				if(setPos)
					Object.SetPosition (origPosition, useGlobalPos);	
				if(setRot)
					Object.SetRotation (origRotation, useGlobalPos);	

			} else {
				Object.GetPosRot (out origPosition, out origRotation, useGlobalPos);

				if(setPos)
					Object.SetPosition (Position, useGlobalPos);	
				if(setRot)
					Object.SetRotation (Rotation, useGlobalPos);	
			}
		}
    }
}
