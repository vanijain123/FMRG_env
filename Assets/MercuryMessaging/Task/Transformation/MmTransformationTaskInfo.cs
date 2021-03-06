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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MercuryMessaging.Task
{
    /// <summary>
    /// A task info that supports tasks requiring 
    /// 3D transformations meet particular requirements.
    /// If you want a lighter-weight version of the TransformationTask 
    /// (without all the header information), set InlcudeTaskInfoData
    /// to true. Please note, this will not stop the creation of the
    /// header fields. It will only make it 
    /// </summary>
    public class MmTransformationTaskInfo : MmTaskInfo
    {
        /// <summary>
        /// If set to false, will skip parsing and serialization of root metadata.
        /// Data will be replaced by <see cref="Step"/>
        /// </summary>
        public bool InlcudeTaskInfoData = true;

        /// <summary>
        /// Optional. Indicates what step this transformation represents.
        /// Useful for chaining transformation tasks together as subtasks.
        /// </summary>
        public int Step;

        #region Rotation
        /// <summary>
        /// Angle requirement.
        /// </summary>
        public float Angle;
        /// <summary>
        /// Rotation Axis requirement.
        /// </summary>
        public Vector3 Axis;
        /// <summary>
        /// Rotational requirement in Quaternion form.
        /// </summary>
        public Quaternion RotationFromTo;

        #endregion

        #region Translation
        /// <summary>
        /// Positional requirement. Can also be used for distance.
        /// </summary>
		public Vector3 Position;

        #endregion

        #region Scale
        /// <summary>
        /// Scale requirement.
        /// </summary>
		public Vector3 Scale;

        #endregion

        /// <summary>
        /// The thresholds for requirements for transformation tasks.
        /// </summary>
        public MmTransformTaskThreshold Threshold;

        /// <summary>
        /// Default constructor for MmTransformationTaskInfo.
        /// </summary>
        public MmTransformationTaskInfo()
        {}

        /// <summary>
        /// Duplicate an MmTransformationTaskInfo
        /// </summary>
        /// <param name="orig">The original MmTransformationTaskInfo.</param>
        public MmTransformationTaskInfo(MmTransformationTaskInfo orig)
        {
            Angle = orig.Angle;
            Axis = orig.Axis;
            RotationFromTo = orig.RotationFromTo;
            Position = orig.Position;
            Scale = orig.Scale;

            InlcudeTaskInfoData = orig.InlcudeTaskInfoData;
            Step = orig.Step;
        }

        /// <summary>
        /// Construct the MmTransformationTaskInfo from String.
        /// </summary>
        /// <param name="str"></param>
        public MmTransformationTaskInfo(string str)
        {
            Parse(str);
        }

        /// <summary>
        /// Convert the MmTransformationTaskInfo to a string.
        /// </summary>
        /// <returns>Outputs MmTransformationTaskInfo in the following form:
        /// Axis.X, Axis.Y, Axis.Z, Angle, Position.X, Position.Y, Position.Z
        /// Scale.X, Scale.Y, Scale.Z</returns>
		public override string ToString()
		{
			return string.Format("{0}," +
			                     "{1:0.0000000}," +
			                     "{2:0.0000000}," +
			                     "{3:0.0000000}," +
			                     "{4:0.0000}," +
								 "{5:0.0000000}," +
								 "{6:0.0000000}," +
								 "{7:0.0000000}," +
								 "{8:0.0000000}," +
								 "{9:0.0000000}," +
								 "{10:0.0000000}",

				InlcudeTaskInfoData ? base.ToString() : Step.ToString(),
				Axis.x,Axis.y, Axis.z, Angle,
				Position.x, Position.y, Position.z,
				Scale.x, Scale.y, Scale.z);
		}

        /// <summary>
        /// Parts a string to set members of MmTransformationTaskInfo.
        /// </summary>
        /// <param name="str">String to be parsed.</param>
        public override int Parse(string str)
		{
			int index = InlcudeTaskInfoData ? base.Parse (str) : 0;
			string[] words = str.Split(',');

			Axis.x = float.Parse(words[index]);
			Axis.y = float.Parse(words[index + 1]);
			Axis.z = float.Parse(words[index + 2]);
			Angle  = float.Parse(words[index + 3]);

			Position.x = float.Parse(words[index + 4]);
			Position.y = float.Parse(words[index + 5]);
			Position.z = float.Parse(words[index + 6]);

			Scale.x = float.Parse(words[index + 7]);
			Scale.y = float.Parse(words[index + 8]);
			Scale.z = float.Parse(words[index + 9]);

            return index + 10;
        }

        /// <summary>
        /// Parts a string to set members of MmTransformationTaskInfo.
        /// </summary>
        /// <param name="str">String to be parsed.</param>
        public override int Parse(string[] str, int offset = 0)
        {
            int index = InlcudeTaskInfoData ? base.Parse(str) : offset;

            Axis.x = float.Parse(str[index]);
            Axis.y = float.Parse(str[index + 1]);
            Axis.z = float.Parse(str[index + 2]);
            Angle = float.Parse(str[index + 3]);

            Position.x = float.Parse(str[index + 4]);
            Position.y = float.Parse(str[index + 5]);
            Position.z = float.Parse(str[index + 6]);

            Scale.x = float.Parse(str[index + 7]);
            Scale.y = float.Parse(str[index + 8]);
            Scale.z = float.Parse(str[index + 9]);

            return index + 10;
        }

        /// <summary>
        /// Parts a string to set members of MmTransformationTaskInfo.
        /// </summary>
        /// <param name="str">String to be parsed.</param>
        public override int Parse(List<string> str, int offset = 0)
        {
            int wordsParsed = InlcudeTaskInfoData ? base.Parse(str) : offset;

            Axis.x = float.Parse(str[wordsParsed]);
            Axis.y = float.Parse(str[wordsParsed + 1]);
            Axis.z = float.Parse(str[wordsParsed + 2]);
            Angle = float.Parse(str[wordsParsed + 3]);

            Position.x = float.Parse(str[wordsParsed + 4]);
            Position.y = float.Parse(str[wordsParsed + 5]);
            Position.z = float.Parse(str[wordsParsed + 6]);

            Scale.x = float.Parse(str[wordsParsed + 7]);
            Scale.y = float.Parse(str[wordsParsed + 8]);
            Scale.z = float.Parse(str[wordsParsed + 9]);

            return wordsParsed + 10;
        }

        /// <summary>
        /// Generates headers for file/stream storing MmTransformationTaskInfos
        /// </summary>
        /// <returns>Outputs header in the following form:
        /// Axis.X, Axis.Y, Axis.Z, Angle, Position.X, Position.Y, Position.Z
        /// Scale.X, Scale.Y, Scale.Z</returns>
		public override string Headers()
		{
			return InlcudeTaskInfoData ? base.Headers() : "Step" + ",Rotation.Axis.X,Rotation.Axis.Y,Rotation.Axis.Z,Rotation.Angle" +
				",Position.X,Position.Y,Position.Z" + 
				",Scale.X,Scale.Y,Scale.Z";
		}

        /// <summary>
        /// Create a duplicate MmTransformationTaskInfo.
        /// </summary>
        /// <returns>Duplicated MmTransformationTaskInfo</returns>
        public override IMmSerializable Copy()
        {
            return new MmTransformationTaskInfo(this);
        }

        /// <summary>
        /// Deserialize the task info from serialized form.
        /// </summary>
        /// <param name="reader">UNET deserializer.</param>
        public override void Deserialize(NetworkReader reader)
        {
            if (InlcudeTaskInfoData)
            {
                base.Deserialize(reader);
            }
            else
            {
                Step = reader.ReadInt32();
            }
            Angle = reader.ReadSingle();
            Axis = reader.ReadVector3();
            RotationFromTo = reader.ReadQuaternion();
			Position = reader.ReadVector3();
			Scale = reader.ReadVector3();
            InlcudeTaskInfoData = reader.ReadBoolean();

        }

        /// <summary>
        /// Serialize the task info into serialized form.
        /// </summary>
        /// <param name="writer">UNET serializer.</param>
        public override void Serialize(NetworkWriter writer)
        {
            if (InlcudeTaskInfoData)
            {
                base.Serialize(writer);
            }
            else
            {
                writer.Write(Step);
            }
            writer.Write(Angle);
            writer.Write(Axis);
            writer.Write(RotationFromTo);
            writer.Write(Position);
            writer.Write(Scale);
            writer.Write(InlcudeTaskInfoData);
        }
    }
}
