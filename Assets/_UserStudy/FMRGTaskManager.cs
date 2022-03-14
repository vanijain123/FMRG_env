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
using System.Linq;
//using CGUI_Utilities.Math;
using MercuryMessaging;
using MercuryMessaging.Support;
using MercuryMessaging.Support.FiniteStateMachine;
using MercuryMessaging.Support.Input;
using MercuryMessaging.Task;
using UnityEngine;



namespace Projects.FMRG.Scripts.UserStudy
{
    public class FMRGTaskManager : MmTaskManager<FMRGTaskInfo>
    {
        public enum TaskType { Free = 0, Recorded }

        public MmRepeatingTask<FMRGTaskInfo> FreeMode;
        public System.Random repeatingTaskRndGen = new System.Random();
        public int RepeatingTaskRepeats = 3;

        #region Member fields

        //public MmTransformTaskThreshold TaskThreshold; ////Need to be changed


        public FMRGSwitchResponder FMRGSwitchResponder;
        public FMRGMmAppStateSwitchResponder FMRGMmAppState;

        public bool FMRGAppStateReady, FMRGSwitchResponderReady;

        public bool LoadStudyImmediately = true;

        //Computational Variables
        public FiniteStateMachine<TaskType> StudyMode;
        public bool IsStudyFinished { get; set; }
        public bool IsPractice
        {
            get { return CurrentTaskInfo.DoNotRecordData; }
        }

        //public DefaultSceneManagerMR sceneManagerMR;

        /*public List<Quaternion> Rotations = new List<Quaternion>
        {
            Quaternion.Euler(0,40,0),
            Quaternion.Euler(0,100,0),
            Quaternion.Euler(0,160,0),
            Quaternion.Euler(0,-40,0),
            Quaternion.Euler(0,-100,0),
            Quaternion.Euler(0,-160,0),
        };*/

        #endregion

        #region Methods

        public override void Awake()
        {
            MmLogger.LogApplication("SpBTaskManager Awake");

            base.Awake();

            StudyMode = new FiniteStateMachine<TaskType>("SpBStudyModes")
            {
                LogMessage = MmLogger.LogApplication
            };

            FMRGMmAppState.MmRegisterStartCompleteCallback(AppStateReadyCallback);

            FMRGSwitchResponder.MmRegisterStartCompleteCallback(SwitchResponderReadyCallback);

            InitializeStudyModes();
        }

        public override void Start()
        {
            MmLogger.LogApplication("OrTaskManager Start");

            base.Start();

            KeyboardHandler.AddEntry(KeyCode.Q, "End Free Mode / Start Study", delegate
            {
                if (StudyMode.Current == TaskType.Free)
                {
                    StudyMode.JumpTo(TaskType.Recorded);
                    ProceedToNextTask();
                    //                  OrMmAppState.GetRelayNode().MmInvoke(MmMethod.Switch, "Start",
                    //                     new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));
                }
            });

            #region Task Loading

            //Todo: Iterate over each task responder and assign the completion delegate into it
            foreach (var routingTableItem in TasksNode.RespondersFSM.Keys)
            {
                FMRGTaskResponder taskResponder = (FMRGTaskResponder)
                    GetTaskResponder(routingTableItem.Responder.GetRelayNode());

                //Debug.Log("Assigning TaskThreshold to " + taskResponder.name);

                ////taskResponder.Thresholds = TaskThreshold;
                //taskResponder.ValidRotations = Rotations;
            }

            if (currentTaskInfo != TaskInfos.First)
            {
                StudyMode.JumpTo(TaskType.Recorded);
            }

            else
            {
                //Generate a repeating task that will power the study's free mode.

                FreeMode = new MmRepeatingTask<FMRGTaskInfo>(
                        FMRGSwitchResponder.MmRelaySwitchNode.
                        RespondersFSM.Keys.Select(k => k.Name).ToList());
                FreeMode.FreeModeVizRepeats = RepeatingTaskRepeats;
                var curTaskName = FreeMode.Task.TaskName;
                var randomTaskId = repeatingTaskRndGen.Next(TaskInfos.Count);
                Debug.LogFormat("randomTaskId: {0}", randomTaskId);
                FMRGTaskInfo taskInfo = new FMRGTaskInfo(TaskInfos.ElementAt(randomTaskId));
                taskInfo.DoNotRecordData = true;
                taskInfo.TaskName = curTaskName;
                FreeMode.Task = taskInfo;


                TaskInfos.AddFirst(FreeMode.Task);

                //GenerateRandomTrialInfo();

                ProceedToFirstTask();
            }


            #endregion

            //PrepareCompletionInfoData();

            InitializeStudyModes();
        }

        public void SwitchResponderReadyCallback()
        {
            MmLogger.LogApplication("FMRGTaskManager: SwitchResponderReadyCallback Called");

            FMRGSwitchResponderReady = true;

            BeginStudy();
        }

        public void AppStateReadyCallback()
        {
            MmLogger.LogApplication("FMRGTaskManager: AppStateReadyCallback Called");

            FMRGAppStateReady = true;

            BeginStudy();
        }

        public void BeginStudy()
        {
            if (FMRGSwitchResponderReady && FMRGAppStateReady)
            {
                MmLogger.LogApplication("FMRGTaskManager: BeginStudy Called");

                if (LoadStudyImmediately)
                    FMRGMmAppState.GetRelayNode().MmInvoke(MmMethod.Switch, "Start",
                        new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));
            }
        }

        protected virtual void InitializeStudyModes()
        {
            StudyMode[TaskType.Free] = new StateEvents
            {
                Enter = delegate
                {
                    if (FMRGSwitchResponder.MmRelaySwitchNode.RespondersFSM.Current != null)
                        FMRGSwitchResponder.MmRelaySwitchNode.MmInvoke(MmMethod.SetActive, false,
                            new MmMetadataBlock(MmLevelFilter.Child, MmActiveFilter.All));
                },
                Exit = delegate
                {
                    //Free mode does not interact with the standard trial progression
                    //logic. Upon exiting free mode, the study will progress into 
                    //a trial waiting state. The active MmRelayNodes are normally
                    //SetActive'd(false) when exiting a trial. However, the trial exit delegate 
                    //is not necessarily invoked when a exit FreeMode command is triggered.
                    //Thus, we need to manually invoke SetActive here.
                    if (FMRGSwitchResponder.MmRelaySwitchNode.RespondersFSM.Current != null)
                        FMRGSwitchResponder.MmRelaySwitchNode.MmInvoke(MmMethod.SetActive, false,
                            new MmMetadataBlock(MmLevelFilter.Child, MmActiveFilter.All));
                }
            };
        }

        public void ExitFreeMode()
        {
            base.ProceedToNextTask();
            FMRGMmAppState.GetRelayNode().MmInvoke(MmMethod.Switch, "Break",
                    new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));
            //sceneManagerMR.ResetScene();
            TasksNode.MmInvoke((MmMethod)FMRGMethods.AssignFMRGTaskInfo,
                (FMRGTaskInfo)currentTaskInfo.Value,
                new MmMetadataBlock(MmLevelFilter.Child, MmActiveFilter.All)
            );
            //reset
           // resetScene();
        }



        public override void ProceedToNextTask()
        {


            if (currentTaskInfo.Next == null)
            {
                IsStudyFinished = true;

                FMRGMmAppState.GetRelayNode().MmInvoke(MmMethod.Switch, "End",
                    new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));
            }
            else
            {
                base.ProceedToNextTask();
                var switchTo = (CurrentTaskInfo.DoNotRecordData && !PrevTaskInfo.DoNotRecordData)
                    ? "Break"
                    : "Waiting";

                FMRGMmAppState.GetRelayNode().MmInvoke(MmMethod.Switch, switchTo,
                    new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));
                //sceneManagerMR.ResetScene();
                TasksNode.MmInvoke((MmMethod)FMRGMethods.AssignFMRGTaskInfo,
                    (FMRGTaskInfo)currentTaskInfo.Value,
                    new MmMetadataBlock(MmLevelFilter.Child, MmActiveFilter.All)
                );
                //reset
                //resetScene();
            }

            if (CurrentTaskInfo != null)
            {
                //TODO: Deal with Task Loading for study Data container
                //SlMmAppState.StudyDataContainer.CurrentSubTask = CurrentTaskInfo;

                //Load next billboard/target info

                //Deal with loading of TaskInfo into task responder


            }
        }

        public override void ProceedToFirstTask()
        {
            base.ProceedToFirstTask();
            //resetScene();
            TasksNode.MmInvoke((MmMethod)FMRGMethods.AssignFMRGTaskInfo,
                currentTaskInfo.Value,
                new MmMetadataBlock(MmLevelFilter.Child, MmActiveFilter.All)
            );
            //SpbMmAppState.MmRelaySwitchNode.MmInvoke(MmMethod.Switch, "Trial",
            //                new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));
        }

        public void GenerateRandomTrialInfo()
        {
            var randomTaskId = repeatingTaskRndGen.Next(TaskInfos.Count);
            Debug.LogFormat("randomTaskId: {0}", randomTaskId);
            FMRGTaskInfo taskInfo = new FMRGTaskInfo(TaskInfos.ElementAt(randomTaskId));
            FreeMode.Task.ItemNums = taskInfo.ItemNums;
            FreeMode.Task.LeftSide = taskInfo.LeftSide;
            var r = Random.value;
            if (r > 0.75)
            {
                FreeMode.Task.TaskName = "noti";
            }
            else if (r > 0.5)
            {
                FreeMode.Task.TaskName = "anno";
            }
            else if (r > 0.25)
            {
                FreeMode.Task.TaskName = "close";
            }
            else
            {
                FreeMode.Task.TaskName = "pivot";
            }

        }

        public override void Update()
        {
            ////
            ////

            if (FMRGMmAppState.CurrentState == FMRGMmAppStateSwitchResponder.AppState.Trial
                )//&& FMRGMmAppState.Logger.IsCompleteMode)
            {
                if (Time.time>1) //- FMRGMmAppState.Logger.IsCompleteStart
                    //> FMRGMmAppState.Logger.IsCompleteTimeThreshold)
                {
                    FMRGMmAppState.Proceed();
                }
                else
                {
                    //FMRGMmAppState.Logger.CompletionTotalTime += Time.deltaTime;
                }
            }


            base.Update();
        }

        protected override void Complete(bool active)
        {
            //if (!active) return;
            //////

            //if (SpbMmAppState.CurrentState != SpbMmAppStateSwitchResponder.AppState.Trial) return;

            //var emptyList = new List<MmTransform>();
            //SpbSwitchResponder.MmRelaySwitchNode.RespondersFSM.Current.Responder.GetRelayNode().MmInvoke(
            //    MmMethod.Refresh,
            //    emptyList,
            //    MmMetadataBlockHelper.SelfDefaultTagAll);

            //SpbMmAppState.Proceed();

        }

        public override bool ShouldTriggerSwitch()
        {
            return (currentTaskInfo != null
                    && currentTaskInfo.Value.TaskName != PrevTaskInfo.TaskName);
        }

        #endregion
    }
}