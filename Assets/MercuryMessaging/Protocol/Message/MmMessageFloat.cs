// Copyright (c) 2017-2019, Columbia University
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
using UnityEngine.Networking;

namespace MercuryMessaging
{
    /// <summary>
    /// MmMessage with float payload
    /// </summary>
    public class MmMessageFloat : MmMessage
    {
        /// <summary>
        /// Float payload
        /// </summary>
		public float value;

        /// <summary>
        /// Creates a basic MmMessageByteArray
        /// </summary>
		public MmMessageFloat()
		{}

        /// <summary>
        /// Creates a basic MmMessage, with a control block
        /// </summary>
        /// <param name="metadataBlock">Object defining the routing of messages</param>
		public MmMessageFloat(MmMetadataBlock metadataBlock = null)
			: base (metadataBlock)
		{
		}

        /// <summary>
        /// Create an MmMessage, with control block, MmMethod, and float
        /// </summary>
        /// <param name="iVal">Float payload</param>
        /// <param name="mmMethod">Identifier of target MmMethod</param>
        /// <param name="metadataBlock">Object defining the routing of messages</param>
		public MmMessageFloat(float iVal, 
			MmMethod mmMethod = default(MmMethod), 
            MmMetadataBlock metadataBlock = null)
            : base(mmMethod, metadataBlock)
        {
			value = iVal;
		}

        /// <summary>
        /// Duplicate an MmMessageFloat
        /// </summary>
        /// <param name="message">Item to duplicate</param>
		public MmMessageFloat(MmMessageFloat message) : base(message)
		{}

        /// <summary>
        /// Message copy method
        /// </summary>
        /// <returns>Duplicate of MmMessage</returns>
        public override MmMessage Copy()
        {
			MmMessageFloat newMessage = new MmMessageFloat (this);
            newMessage.value = value;

            return newMessage;
        }

        /// <summary>
        /// Deserialize the message
        /// </summary>
        /// <param name="reader">UNET based deserializer object</param>
        public override void Deserialize(NetworkReader reader)
		{
			base.Deserialize (reader);
			value = reader.ReadSingle();
		}

        /// <summary>
        /// Serialize the MmMessage
        /// </summary>
        /// <param name="writer">UNET based serializer</param>
        public override void Serialize(NetworkWriter writer)
		{
			base.Serialize (writer);
			writer.Write (value);
		}
    }
}