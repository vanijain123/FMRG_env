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

using System;
using System.Collections.Generic;
using MercuryMessaging.Task;
using UnityEngine;
using UnityEngine.Networking;

namespace Projects.FMRG.Scripts.UserStudy
{
    public class FMRGTaskInfo : MmTaskInfo
    {
        public int Block;
        public int NumOfQuads;
        public bool LeftSide; //0 -> right, 1-> left;
        public List<int> ItemNums;

        public FMRGTaskInfo() : base()
        {
            ItemNums = new List<int>();
        }

        /// <summary>
        /// Copy constructor for the SpBTaskInfo class
        /// </summary>
        /// <param name="orig"></param>
        public FMRGTaskInfo(FMRGTaskInfo orig) : base(orig)
        {
            Block = orig.Block;
            NumOfQuads = orig.NumOfQuads;
            LeftSide = orig.LeftSide;
            ItemNums = new List<int>(orig.ItemNums);
            //poss = new List<Vector3>(orig.poss);
            //rots = new List<Quaternion>(orig.rots);
        }

        public override IMmSerializable Copy()
        {
            return new FMRGTaskInfo(this);
        }

        public override int Parse(string str)
        {
            //Parse base
            int index = base.Parse(str);

            List<string> words = new List<string>(str.Split(','));

            Block = int.Parse(words[index]);
            index += 1;
            NumOfQuads = int.Parse(words[index]);
            index += 1;
            LeftSide = int.Parse(words[index]) != 0;
            index += 1;
            ItemNums = new List<int>();
            //For each transformation (sub-task) within this task,
            //  allow the specialized transformation task info
            for (int i = 0; i < NumOfQuads; i++)
            {
                ItemNums.Add(int.Parse(words[index]));
                index += 1;

            }

            return index;
        }

        public override void Deserialize(NetworkReader reader)
        {

            base.Deserialize(reader);
            Block = reader.ReadInt32();
            NumOfQuads = reader.ReadInt32();
            LeftSide = reader.ReadBoolean();

            ItemNums = new List<int>();


            for (int i = 0; i < NumOfQuads; i++)
            {
                ItemNums.Add(reader.ReadInt32());
            }

        }

        public override void Serialize(NetworkWriter writer)
        {

            base.Serialize(writer);
            writer.Write(Block);
            writer.Write(NumOfQuads);
            writer.Write(LeftSide);

            //For each transformation (sub-task) within this task
            // invoke the object deserialization method

            for (int i = 0; i < ItemNums.Count; i++)
            {
                writer.Write(ItemNums[i]);
            }

        }

        public override string Headers()
        {
            string headerString = string.Format("{0},{1},{2},{3},",
                base.Headers(), "Block", "NumOfQuads", "LeftSide");

            //For each quad add pos and rot to header
            for (int i = 0; i < ItemNums.Count; i++)
            {
                headerString += "ItemNum" + ",";
            }
            int len = headerString.Length;
            return headerString.Substring(0, len - 1);
        }

        public override string ToString()
        {
            string newString = string.Format("{0},{1},{2},{3},",
                base.ToString(), Block, NumOfQuads, LeftSide ? 1 : 0);

            for (int i = 0; i < ItemNums.Count; i++)
            {
                newString += ItemNums[i].ToString() + ",";
            }

            int len = newString.Length;
            return newString.Substring(0, len - 1);
        }
    }
}
