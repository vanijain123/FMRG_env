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
using CGUI_Utilities.Audio;
using MercuryMessaging;
using MercuryMessaging.AppState;
using MercuryMessaging.Support.Data;
using MercuryMessaging.Support.FiniteStateMachine;
using MercuryMessaging.Support.Input;
using MercuryMessaging.Task;
using UnityEngine;

namespace Projects.FMRG.Scripts.UserStudy
{
    public class FMRGMmAppStateSwitchResponder : MmAppStateSwitchResponder
    {
        #region Member Variables

        public FMRGTaskManager StudyTaskManager;
        public FMRGSwitchResponder VisualizationSwitchResponder;

        //These should be assigned in the Unity Editor
        public MmDataCollector MmDataCollectorStats;
        public MmDataCollector MmDataCollectorHead;
        public MmDataCollector MmDataCollectorLeftHandController;
        public MmDataCollector MmDataCollectorRightHandController;

        public float ProceedPreventStart = 0;
        public float ProceedPreventTime = 8f;
        public bool AllowProceedPrevention = true;

        public FMRGDataContainer StudyDataContainer;
        public float CompletionTimeThreshold = 3;

        public MmRelayNode GuiRelayNode;

        public GameObject HMD, LeftHandController, RightHandController;



        #endregion

        public enum AppState
        {
            Off,
            Initialization,
            Waiting,
            Start,
            Trial,
            Break,
            End
        }

        public enum SlFunctions
        {
            ApplyThresholdInfo = 2001
        }

        /// <summary>
        /// Convert the specified stateName.
        /// 
        /// </summary>
        /// <param name="stateName">State name.</param>
        public static AppState Convert(string stateName)
        {
            if (!Enum.IsDefined(typeof(AppState), stateName))
            {
                throw new ArgumentException(string.Format("{0} isn't defined in AppState Enum", stateName));
            }
            return (AppState)Enum.Parse(typeof(AppState), stateName);
        }

        public AppState CurrentState
        {
            get { return MmRelaySwitchNode != null ? Convert(MmRelaySwitchNode.CurrentName) : AppState.Off; }
        }

        #region UserInstructions

        public static string UITextPreTrial =
            "<b>Condition: {0}</b>\n\n" +
            "{1}\n\n" + // Practice or Not
            "Trial: {2} of {3}";

        public static string UITextPreStudy =
            "<b>Study Start</b>\n\n";

        public static string UITextInitialization =
            "<b>Study Initialization</b>\n\n" +
            "User ID: {0}";

        public static string UITextPostStudy =
            "<b>End of Study</b>\n\n" +
            "Please complete evaluation.\n\n" +
            "Thank you!";

        public static string UITextPostCondition =
            "<b>End of Condition:\n" +
            "{0}</b>\n\n" +
            //"Please complete evaluation.\n\n" +
            "Continue when ready.";

        public static string UITextFreeMode =
            "<b>Condition: {0}</b>\n\n" +
            "Free Mode";


        public static string UITextPractice = "Practice";
        public static string UITextTimedTrial = "Timed";

        [SerializeField] private bool demoMode;

        #endregion




        public override void Start()
        {
            base.Start();

            KeyboardHandler.AddEntry(KeyCode.UpArrow, "Next Study Step", delegate
            {
                if (CurrentState != AppState.Trial)
                    Proceed();
            });

            KeyboardHandler.AddEntry(KeyCode.DownArrow, "Next Study Step", delegate
            {
                if (CurrentState != AppState.Trial)
                    Proceed();
            });

            KeyboardHandler.AddEntry(KeyCode.O, "Opt Out", delegate
            {
                if (CurrentState == AppState.Trial)
                {
                    StudyDataContainer.IsOptOut = true;
                    Proceed();
                }
            });

            KeyboardHandler.AddEntry(KeyCode.B, "Bad Data", delegate
            {
                if (CurrentState == AppState.Trial)
                {
                    StudyDataContainer.IsBadData = true;
                    Proceed();
                }
            });
        }

        public override void Update()
        {
            if (CurrentState == AppState.Trial
                && !StudyTaskManager.CurrentTaskInfo.DoNotRecordData)
            {
                StudyDataContainer.Update();
            }

            //if (RightHandControllerTrigger.GetComponent<RightControllerButton>().ControllerTriggered && (!alreadytriggered))
            //{
            //    alreadytriggered = true;
            //    if (CurrentState != AppState.Trial)
            //        Proceed();
            //}



        }

        private void SendInfo()
        {
            MmLogger.LogApplication("Sending client initialization information.");

            //GetRelayNode().MmInvoke((MmMethod)SlFunctions.ApplyThresholdInfo,
            //    StudyTaskManager.TaskThreshold,
            //    new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));

            //TODO: Send user ID
        }

        public override void SetupAppStates()
        {
            base.SetupAppStates();

            MmLogger.LogApplication("OrTaskManager: Initialized Study States.");

            // =====================================
            // Trial
            // =====================================

            MmRelaySwitchNode["Trial"] = new StateEvents
            {
                Enter = delegate
                {
                    MmLogger.LogApplication("Entering trial");

                    StudyDataContainer = new FMRGDataContainer()
                    {
                        CurrentTask = StudyTaskManager.CurrentTaskInfo,
                        MmDataCollectorHead = MmDataCollectorHead,
                        MmDataCollectorStats = MmDataCollectorStats,



                        UserInfo = StudyTaskManager.MmTaskUserData,
                        IsCompleteTimeThreshold = this.CompletionTimeThreshold,
                    };

                    StudyDataContainer.CurrentResponder =
                        StudyTaskManager.FMRGSwitchResponder.
                            MmRelaySwitchNode.Current.GetComponent<FMRGTaskResponder>();

                    if (!StudyTaskManager.CurrentTaskInfo.DoNotRecordData)
                    {
                        StudyDataContainer.RegisterDataCollectors();
                        MmDataCollectorHead.gameObject.SetActive(true);

                        MmDataCollectorLeftHandController.gameObject.SetActive(true);
                        MmDataCollectorRightHandController.gameObject.SetActive(true);

                    }

                    //TrackedObject.transform.GetChild(0).gameObject.SetActive(true);

                    SoundEffectManager.PlayEffect(SoundItem.Next);

                },
                Exit = delegate
                {
                    MmLogger.LogApplication("Exiting trial.");
                    //  Debug.Log("Exiting trial. Exiting trial. Exiting trial. Exiting trial. Exiting trial.");
                    //TrackedObject.transform.GetChild(0).gameObject.SetActive(false);
                    StudyDataContainer.DeRegisterFMRGDataContainer();
                    MmDataCollectorHead.gameObject.SetActive(false);

                    MmDataCollectorLeftHandController.gameObject.SetActive(false);
                    MmDataCollectorRightHandController.gameObject.SetActive(false);

                    //if (StudyTaskManager.PrevTaskInfo != null
                    //&& StudyTaskManager.PrevTaskInfo.DoNotRecordData
                    //&& !StudyTaskManager.CurrentTaskInfo.DoNotRecordData)
                    //    SoundEffectManager.PlayEffect(SoundItem.StartRound);
                    //else
                    //    SoundEffectManager.PlayEffect(SoundItem.Correct);

                    //WIM.SetActive(false);
                }
            };

            // =====================================
            // Waiting
            // =====================================

            MmRelaySwitchNode["Waiting"] = new StateEvents
            {
                Enter = delegate
                {
                    MmLogger.LogApplication("Entering Waiting.");
                    //                    Debug.Log("VisualizationSwitchResponder.MmRelaySwitchNode.RespondersFSM.Current.Name" + VisualizationSwitchResponder.MmRelaySwitchNode.RespondersFSM.Current.Name);
                    if (VisualizationSwitchResponder.MmRelaySwitchNode.RespondersFSM.Current != null)
                    {
                        VisualizationSwitchResponder.MmRelaySwitchNode.MmInvoke(MmMethod.SetActive, false,
                              new MmMetadataBlock(MmLevelFilter.Child, MmActiveFilter.All));
                    }

                    if (StudyTaskManager.StudyMode.Current == FMRGTaskManager.TaskType.Recorded)
                    {
                        string PanelTaskName;


                        PanelTaskName = "0";

                        GuiRelayNode.MmInvoke(MmMethod.MessageString, string.Format(
                                UITextPreTrial,
                                PanelTaskName,
                                (StudyTaskManager.IsPractice ? UITextPractice : UITextTimedTrial),
                                (StudyTaskManager.CurrentTaskInfo.TaskId + 1),
                                3), //StudyTaskManager.TotalTasksWithCurrentName
                            MmMetadataBlockHelper.SelfDefaultTagAll);

                    }
                    else
                    {
                        string PanelTaskName;
                        PanelTaskName = "0";


                        GuiRelayNode.MmInvoke(MmMethod.MessageString, string.Format(
                                UITextFreeMode, PanelTaskName), MmMetadataBlockHelper.SelfDefaultTagAll);
                        //StudyTaskManager.CurrentTaskInfo.TaskName
                    }

                    GuiRelayNode.MmInvoke(MmMethod.SetActive, true, MmMetadataBlockHelper.SelfDefaultTagAll);



                    GetRelayNode().MmInvoke(MmMethod.TaskInfo,
                        StudyTaskManager.CurrentTaskInfo,
                        new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));
                    //Todo: Also trigger current free mode.

                    //BaseObject.transform.Rotate(axis, angle, Space.World);
                    //MmLogger.LogApplication("******Enter Waiting Base: " + BaseObject.transform.rotation);

                    ProceedPreventStart = Time.time;


                    if (demoMode)
                        MmRelaySwitchNode.JumpTo("Trial");

                },
                Exit = delegate
                {
                    MmLogger.LogApplication("Exiting Waiting.");

                    //TODO: Is this redundant with TaskManager.ProceedToNextTask()?
                    if (VisualizationSwitchResponder.MmRelaySwitchNode.RespondersFSM.Current.Responder !=
                        VisualizationSwitchResponder.MmRelaySwitchNode.RoutingTable[StudyTaskManager.CurrentTaskInfo.TaskName].Responder)
                    {
                        VisualizationSwitchResponder.MmRelaySwitchNode.MmInvoke(MmMethod.Switch, StudyTaskManager.CurrentTaskInfo.TaskName,
                            new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));
                    }
                    else
                    {
                        //Todo: Refresh and SetActive are called here because the Switch above (which also has a
                        // Refresh/SetActive will not get called if Switch is called on the same state, which it usually is.
                        List<MmTransform> transformList = new List<MmTransform>();
                        //transformList.Add(new MmTransform(TrackedObject.transform, true));
                        //transformList.Add(new MmTransform(BaseObject.transform, true));
                        VisualizationSwitchResponder.MmRelaySwitchNode.MmInvoke(MmMethod.Refresh, transformList,
                            new MmMetadataBlock(MmLevelFilter.Child, MmActiveFilter.All, MmSelectedFilter.Selected));

                        VisualizationSwitchResponder.MmRelaySwitchNode.MmInvoke(MmMethod.SetActive, true,
                            new MmMetadataBlock(MmLevelFilter.Child, MmActiveFilter.All, MmSelectedFilter.Selected));

                    }

                    GuiRelayNode.MmInvoke(MmMethod.SetActive, false, MmMetadataBlockHelper.SelfDefaultTagAll);
                }
            };

            // =====================================
            // Start
            // =====================================

            MmRelaySwitchNode["Start"] = new StateEvents
            {
                Enter = delegate
                {
                    MmLogger.LogApplication("Starting Study for user: " + StudyTaskManager.MmTaskUserData.UserId);

                    GuiRelayNode.MmInvoke(MmMethod.MessageString, UITextPreStudy, MmMetadataBlockHelper.SelfDefaultTagAll);
                    GuiRelayNode.MmInvoke(MmMethod.SetActive, true, MmMetadataBlockHelper.SelfDefaultTagAll);


                    VisualizationSwitchResponder.MmRelaySwitchNode.MmInvoke(MmMethod.SetActive, false,
                        new MmMetadataBlock(MmLevelFilter.Child, MmActiveFilter.All, MmSelectedFilter.Selected));
                    // Debug.Log("VisualizationSwitchResponder.MmRelaySwitchNode.RespondersFSM.Current.Name" + VisualizationSwitchResponder.MmRelaySwitchNode.RespondersFSM.Current.Name);
                },
                Exit = delegate
                {

                    GuiRelayNode.MmInvoke(MmMethod.SetActive, false, MmMetadataBlockHelper.SelfDefaultTagAll);

                    //SoundEffectManager.PlayEffect(SoundItem.Pling);
                }
            };

            // =====================================
            // Break
            // =====================================

            MmRelaySwitchNode["Break"] = new StateEvents
            {
                Enter = delegate
                {
                    MmLogger.LogApplication("Entering Break.");

                    if (VisualizationSwitchResponder.MmRelaySwitchNode.RespondersFSM.Current != null)
                    {
                        VisualizationSwitchResponder.MmRelaySwitchNode.MmInvoke(MmMethod.SetActive, false,
                            new MmMetadataBlock(MmLevelFilter.Child, MmActiveFilter.All));

                    }

                    string PanelTaskName;
                    string PanelTaskName1;
                    string PanelTaskName2;





                    if (StudyTaskManager.CurrentTaskInfo.TaskName == "Cue")
                    {
                        PanelTaskName2 = "0";
                    }
                    else if (StudyTaskManager.CurrentTaskInfo.TaskName == "PreCue")
                    {
                        PanelTaskName2 = "1";
                    }
                    else if (StudyTaskManager.CurrentTaskInfo.TaskName == "PreCue2")
                    {
                        PanelTaskName2 = "2";
                    }
                    else if (StudyTaskManager.CurrentTaskInfo.TaskName == "PreCue3")
                    {
                        PanelTaskName2 = "3";
                    }
                    else if (StudyTaskManager.CurrentTaskInfo.TaskName == "PreCue4")
                    {
                        PanelTaskName2 = "4";
                    }
                    else
                    {
                        PanelTaskName2 = "Sand Box";
                    }



                    //StudyTaskManager.PrevTaskInfo.TaskName
                    GuiRelayNode.MmInvoke(MmMethod.MessageString, string.Format(
                        UITextPostCondition,
                        "0"), MmMetadataBlockHelper.SelfDefaultTagAll);
                    GuiRelayNode.MmInvoke(MmMethod.SetActive, true, MmMetadataBlockHelper.SelfDefaultTagAll);

                    //SoundEffectManager.PlayEffect(SoundItem.GoodJob);
                },
                Exit = delegate
                {
                    MmLogger.LogApplication("Exiting Break.");

                    GuiRelayNode.MmInvoke(MmMethod.SetActive, false, MmMetadataBlockHelper.SelfDefaultTagAll);
                }
            };

            // =====================================
            // Break
            // =====================================

            MmRelaySwitchNode["End"] = new StateEvents
            {
                Enter = delegate
                {

                    GuiRelayNode.MmInvoke(MmMethod.MessageString, UITextPostStudy, MmMetadataBlockHelper.SelfDefaultTagAll);
                    GuiRelayNode.MmInvoke(MmMethod.SetActive, true, MmMetadataBlockHelper.SelfDefaultTagAll);

                    //SoundEffectManager.PlayEffect(SoundItem.Victory);

                    VisualizationSwitchResponder.MmRelaySwitchNode.MmInvoke(MmMethod.SetActive, false,
                        new MmMetadataBlock(MmLevelFilter.Child, MmActiveFilter.All));
                },
                Exit = delegate
                {
                    MmLogger.LogApplication("Study has ended.");
                }
            };

            // =====================================
            // Initialization
            // =====================================

            MmRelaySwitchNode["Initialization"] = new StateEvents
            {
                Enter = delegate
                {
                    SendInfo();

                    GuiRelayNode.MmInvoke(MmMethod.MessageString, string.Format(UITextInitialization,
                                                StudyTaskManager.MmTaskUserData.UserId), MmMetadataBlockHelper.SelfDefaultTagAll);
                    GuiRelayNode.MmInvoke(MmMethod.SetActive, true, MmMetadataBlockHelper.SelfDefaultTagAll);
                },
                Exit = delegate
                {
                    GuiRelayNode.MmInvoke(MmMethod.SetActive, false, MmMetadataBlockHelper.SelfDefaultTagAll);

                    //SoundEffectManager.PlayEffect(SoundItem.LetsGo);
                }
            };
        }

        /// <summary>
        /// Move the study to the next state.
        /// </summary>
        public virtual void Proceed()
        {
            if (StudyTaskManager.IsStudyFinished)
                return;

            switch (CurrentState)
            {
                // =====================================
                // Start
                // =====================================
                case AppState.Start:

                    MmRelaySwitchNode.MmInvoke(MmMethod.Switch, "Initialization",
                        new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));


                    break;
                // =====================================
                // Waiting
                // =====================================
                case AppState.Waiting:
                    if (!AllowProceedPrevention
                        || (Time.time - ProceedPreventStart)
                        >= ProceedPreventTime)

                        MmRelaySwitchNode.MmInvoke(MmMethod.Switch, "Trial",
                            new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));

                    break;
                // =====================================
                // Break
                // =====================================
                case AppState.Break:

                    MmRelaySwitchNode.MmInvoke(MmMethod.Switch, "Waiting",
                        new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));

                    break;
                // =====================================
                // Trial
                // =====================================
                case AppState.Trial:
                    if (StudyTaskManager.StudyMode.Current == FMRGTaskManager.TaskType.Recorded)
                    {
                        //Check is complete, then proceed to waiting
                        SaveStudyData();
                        StudyTaskManager.ProceedToNextTask();
                    }
                    else
                    {
                        StudyTaskManager.GenerateRandomTrialInfo();

                        StudyTaskManager.FreeMode.IterateRepeatingTask();

                        MmRelaySwitchNode.MmInvoke(MmMethod.Switch, "Waiting",
                            new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));

                    }
                    break;
                // =====================================
                // Initialization
                // =====================================
                case AppState.Initialization:

                    MmRelaySwitchNode.MmInvoke(MmMethod.Switch, "Waiting",
                        new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));


                    break;
                // =====================================
                // Off
                // =====================================
                case AppState.Off:

                    MmRelaySwitchNode.MmInvoke(MmMethod.Switch, "Start",
                        new MmMetadataBlock(MmLevelFilter.Self, MmActiveFilter.All));

                    break;
            }
        }

        protected override void ApplyTaskInfo(IMmSerializable serializableValue)
        {
            ApplyTaskValue((FMRGTaskInfo)serializableValue);
        }

        protected virtual void ApplyThresholdInfo(IMmSerializable serializableValue)
        {
            //VisualizationSwitchResponder.ApplyCompletionInfo((MmTransformTaskThreshold) serializableValue);
        }

        public virtual void SaveStudyData()
        {
            if (!StudyTaskManager.CurrentTaskInfo.DoNotRecordData)
                StudyDataContainer.EndOfTrial();
        }

        public override void MmInvoke(MmMessageType msgType, MmMessage message)
        {
            var type = message.MmMethod;
            MmMessage copyMsg = message;

            switch ((SlFunctions)type)
            {
                case SlFunctions.ApplyThresholdInfo:
                    var messageSerializable = (MmMessageSerializable)copyMsg;
                    ApplyThresholdInfo(messageSerializable.value);
                    return;
            }

            base.MmInvoke(msgType, message);
        }

        public void ApplyTaskValue(FMRGTaskInfo taskInfo)
        {
            if (StudyTaskManager.TaskInfos == null)
            {
                StudyTaskManager.TaskInfos = new LinkedList<FMRGTaskInfo>();
                StudyTaskManager.TaskInfos.AddFirst(new LinkedListNode<FMRGTaskInfo>(taskInfo));
            }
            StudyTaskManager.CurrentTaskInfo = taskInfo;

            //GUIHandler.Instance.ToggleFullScreenText();
        }
    }



}
