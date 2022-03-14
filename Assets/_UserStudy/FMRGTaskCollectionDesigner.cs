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
using System.Linq;
using MercuryMessaging;
using MercuryMessaging.Support.Extensions;
using MercuryMessaging.Support.Input;
using MercuryMessaging.Task;
using Projects.FMRG.Scripts.UserStudy;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Projects.FMRG.Scripts.UserStudy
{

    public class FMRGTaskCollectionDesigner : MmTaskCollectionDesigner
    {
        [Header("Mercury Routing Table")]
        [ReorderableList]
        public MmRoutingTable MmRoutingTable;

        public KeyCode GenerateTrigger = KeyCode.Ampersand;

        public string SequenceFilePath;

        public int NumberOfBlocksPerCondition = 1;

        //public bool InitialSetupGuide = true;

        // public List<int> TrackersInLocations;
        // public List<int> InitialTrackerPositions;

        int categoryCount = 6;
        int categoryNeeded = 3;
        int itemCount = 8;
        int itemNeeded = 3;


        public virtual void Start()
        {
            KeyboardHandler.AddEntry(GenerateTrigger,
                "Generate Task UserSequence File", Generate);

            SequenceFilePath = Application.dataPath + "/../";
            //Generate();
        }

        public override void Generate()
        {
            DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
            UnityEngine.Random.InitState(currentEpochTime);
            //NUM_OF_TASKS = sequenceMaxLength * NumberOfBlocksPerCondition;

            //Condition List shuffling 
            var conditionList = MmRoutingTable.Select(x => x.Name).ToArray();
            var numOfCond = 4;
            var conditionPerms = MmPermutation.GetPermutationsArray(new string[] { "noti", "anno", "close", "pivot" });
            Shuffle(ref conditionPerms);
            var sidePerms = MmPermutation.GetPermutationsArray(new string[] { "left1", "right1", "left2", "right2", "left3", "right3" });
            Shuffle(ref sidePerms);

            //RotationIndex List preparations
            //int[][] rotIndexPerms = MmPermutation.GetPermutationsArray(AcceptableRotationIndices);
            //Shuffle(ref rotIndexPerms);

            taskSequence = new List<MmTaskInfo>();

            var recId = 0;
            var sideId = 0;
            var condiId = 0;

            var sideIdx = 0;

            // For each user
            for (int u = 0; u < MAX_USER_COUNT; u++)
            {
                var seqId = 0;

                for (int b = 0; b < NumberOfBlocksPerCondition * numOfCond; b++)
                {
                    for (int t = 0; t < NUM_OF_TASKS; t++)
                    {
                        string TempTaskName = (((b / numOfCond) % 2) == 0) ? conditionPerms[u % conditionPerms.Length][b % conditionPerms[0].Length] : conditionPerms[u % conditionPerms.Length][conditionPerms[0].Length - 1 - (b % conditionPerms[0].Length)];
                        FMRGTaskInfo trial = new FMRGTaskInfo()
                        {
                            RecordId = recId,
                            UserId = u,
                            UserSequence = seqId,
                            TaskName = TempTaskName,// == "not" ? "noti" : "nonoti",
                            TaskId = t,
                            DoNotRecordData = t < NumberOfUnrecordedTasks,
                            Block = b,
                            LeftSide = sidePerms[sideIdx][sideId].Substring(0, 4) == "left" ? true : false,
                            NumOfQuads = categoryNeeded * itemNeeded,
                            ItemNums = randomlySelectDispItem()

                        };

                        taskSequence.Add(trial);
                        seqId++;
                        recId++;
                        sideId++;
                        if (((b / numOfCond) % 2) == 0)
                        {
                            condiId++;
                        }

                        if (sideId % sidePerms[0].Length == 0)
                        {
                            sideIdx++;
                            sideId = 0;
                        }
                        if (sideIdx == sidePerms.Length)
                        {
                            Shuffle(ref sidePerms);
                            sideId = 0;
                            sideIdx = 0;
                        }
                        // Need to reshuffle if num of user > 24
                        //if (condiId == conditionPerms.Length)
                        //{
                        //    Shuffle(ref conditionPerms);
                        //    condiId = 0;
                        //}
                    }
                }
            }

            Save();
        }

        public List<int> randomlySelectDispItem()
        {


            List<int> catList = new List<int>();
            List<List<int>> itmList = new List<List<int>>();
            List<int> flatItmList = new List<int>();
            while (catList.Count < categoryNeeded)
            {
                var randomInt = UnityEngine.Random.Range(0, categoryCount - 1);
                if (catList.Contains(randomInt))
                {
                    continue;
                }
                catList.Add(randomInt);
            }
            for (int i = 0; i < categoryNeeded; i++)
            {
                itmList.Add(new List<int>());
                while (itmList[i].Count < itemNeeded)
                {
                    var randomInt = UnityEngine.Random.Range(0, itemCount - 1);
                    if (itmList[i].Contains(randomInt))
                    {
                        continue;
                    }
                    itmList[i].Add(randomInt);
                    flatItmList.Add(itemCount * catList[i] + randomInt);
                }
            }
            return flatItmList;
        }


#if UNITY_EDITOR

        public void OnEditorUpdate()
        {
        }

        public void OnEnable()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        public void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }
#endif

    }
}
