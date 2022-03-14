using CGUI_Utilities.Math;
using MercuryMessaging;
using MercuryMessaging.Support.Data;
using MercuryMessaging.Support.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Projects.FMRG.Scripts.UserStudy
{
    public class FMRGDataContainer : MonoBehaviour
    {
        private float trialTime;

        public float IsCompleteStart;

        public float IsCompleteTimeThreshold = 3;

        public bool IsCompleteMode;
        public int CompletionEntranceCounter;
        public float CompletionTotalTime;

        private Vector3 prevHeadPosition = Vector3.zero;
        private Quaternion prevHeadOrientation = Quaternion.identity;
        private bool prevHeadPositionandOrientationnotupdated = true;

        #region Incremental Motion

        private Vector3 HMDIncrementalTranslationDirection;
        private float HMDIncrementalTranslationDistance;

        private Vector3 HMDIncrementalRotationAxis;
        private float HMDIncrementalRotationAngle;

        #endregion

        #region Cumulative Stats

        private float HMDCumulAngleTraveled;
        private float HMDCumulDistanceTraveled;

        #endregion

        [SerializeField] private float badTrackingAngleThreshold = 8f;
        [SerializeField] private float badTrackingDistanceThreshold = 8f;

        private bool badTrackingDataObserved;

        public bool IsBadData;
        public bool IsOptOut;

        public MmTaskUserConfigurator UserInfo;

        #region Finetuning Members

        private enum interactionType { Localization = 0, Finetuning };

        [SerializeField] private float finetuningThreshold = 10;

        private int finetuningEntranceCounter;

        private float finetuningTotalTime;

        private FiniteStateMachine<interactionType> interactionMode;

        #endregion

        #region Data Collectors

        public MmDataCollector MmDataCollectorStats;
        public MmDataCollector MmDataCollectorHead;
       

        [SerializeField] private string nameSummaryStats = "Summary_Stats";
        [SerializeField] private string nameHead = "Head";
        [SerializeField] private string nameEvents = "Event";

        #endregion

        #region References to Study Objects

        // These refs are passed at construction time

        public FMRGTaskInfo CurrentTask;

        private FMRGTaskResponder currentResponder;

        public FMRGTaskResponder CurrentResponder
        {
            get { return currentResponder; }
            set
            {
                /*
                //De-register from the previous task responder
                if (currentResponder &&
                    currentResponder.HandleLoadSubTasks != null)
                    currentResponder.HandleLoadSubTasks -= LoadSubtasks;

                if (currentResponder &&
                    currentResponder.HandleStartSubTask != null)
                    currentResponder.HandleStartSubTask -= StartOfSubtask;

                if (currentResponder &&
                    currentResponder.HandleEndSubTask != null)
                    currentResponder.HandleEndSubTask -= EndOfSubtask;

                currentResponder = value;

                //Register to the new task responder
                currentResponder.HandleLoadSubTasks += LoadSubtasks;
                currentResponder.HandleStartSubTask += StartOfSubtask;
                currentResponder.HandleEndSubTask += EndOfSubtask;
                */
            }
        }

        ////public SLSubTaskInfo CurrentSubTask;

        #endregion

        public FMRGDataContainer()
        {
            interactionMode = new FiniteStateMachine<interactionType>("InteractionModes")
            {
                LogMessage = MmLogger.LogApplication
            };

            interactionMode[interactionType.Finetuning] = new StateEvents
            {
                Enter = delegate
                {
                    finetuningEntranceCounter++;
                },
            };

            interactionMode.JumpTo(interactionType.Localization);
        }

        /// <summary>
        /// Register the variables to be written every frame in tracker file
        ///  & register the variables to be written at end of trial in stats file
        /// </summary>
        public void RegisterDataCollectors()
        {


            SetupDataCollectorSummaryStats();
            SetupDataCollectorHead();




        }
        /*
        public void SetupDataCollectorTrackers(MmDataCollector dc, string name)
        {
            dc.Clear();

            if (currentResponder == null ||
                currentResponder.CurrentTarget == null ||
                currentResponder.CurrentTracker == null)
                return;

            dc.Add("TrialTime", () => trialTime.ToString());
            dc.Add("DeltaTime", () => Time.deltaTime.ToString());

            //dc.Add("Tracker.Position", () => currentResponder.CurrentTracker.transform.localPosition.ToCSV('|'));
            //dc.Add("Tracker.Rotation", () => currentResponder.CurrentTracker.transform.localRotation.ToCSV('|'));

            dc.Add("Tracker.Position.x", () => currentResponder.CurrentTracker.transform.parent.parent.localPosition.x.ToString());
            dc.Add("Tracker.Position.y", () => currentResponder.CurrentTracker.transform.parent.parent.localPosition.y.ToString());
            dc.Add("Tracker.Position.z", () => currentResponder.CurrentTracker.transform.parent.parent.localPosition.z.ToString());

            dc.Add("Tracker.Rotation.x", () => currentResponder.CurrentTracker.transform.parent.parent.localRotation.x.ToString());
            dc.Add("Tracker.Rotation.y", () => currentResponder.CurrentTracker.transform.parent.parent.localRotation.y.ToString());
            dc.Add("Tracker.Rotation.z", () => currentResponder.CurrentTracker.transform.parent.parent.localRotation.z.ToString());

            //TODO: The extension method for V2 seems to get confused with V3. Does V3 extend V2? Need to look into it
            //dc.Add("TouchPad.Axis", () => ctrl.GetTouchpadAxis().ToCSV('|'));

            if (!Directory.Exists(Path.Combine(UserInfo.DirPath, UserInfo.SequenceId.ToString())))
                Directory.CreateDirectory(Path.Combine(UserInfo.DirPath, UserInfo.SequenceId.ToString()));

            dc.CreateDataHandler(Path.Combine(UserInfo.DirPath, UserInfo.SequenceId.ToString()),
                CurrentTask.UserSequence + "_" + name);
            dc.OpenTag();
        }
        */
        public void SetupDataCollectorHead()
        {
            MmDataCollectorHead.Clear();

            MmDataCollectorHead.Add("TrialTime", () => trialTime.ToString());
            MmDataCollectorHead.Add("DeltaTime", () => Time.deltaTime.ToString());

            //MmDataCollectorHead.Add("Head.Position", () => currentResponder.HMD.transform.localPosition.ToCSV('|'));
            //MmDataCollectorHead.Add("Head.Rotation", () => currentResponder.HMD.transform.localRotation.ToCSV('|'));

            MmDataCollectorHead.Add("Head.Position.x", () => currentResponder.HMD.transform.localPosition.x.ToString());
            MmDataCollectorHead.Add("Head.Position.y", () => currentResponder.HMD.transform.localPosition.y.ToString());
            MmDataCollectorHead.Add("Head.Position.z", () => currentResponder.HMD.transform.localPosition.z.ToString());
            MmDataCollectorHead.Add("Head.Rotation.x", () => currentResponder.HMD.transform.localRotation.x.ToString());
            MmDataCollectorHead.Add("Head.Rotation.y", () => currentResponder.HMD.transform.localRotation.y.ToString());
            MmDataCollectorHead.Add("Head.Rotation.z", () => currentResponder.HMD.transform.localRotation.z.ToString());

            MmDataCollectorHead.Add("Head.Incremental.Translation.Distance", () => HMDIncrementalTranslationDistance.ToString());
            //MmDataCollectorHead.Add("Incremental.Translation.Direction", () => incrementalTranslationDirection.ToCSV('|'));

            MmDataCollectorHead.Add("Head.Incremental.Translation.Direction.x", () => HMDIncrementalTranslationDirection.x.ToString());
            MmDataCollectorHead.Add("Head.Incremental.Translation.Direction.y", () => HMDIncrementalTranslationDirection.y.ToString());
            MmDataCollectorHead.Add("Head.Incremental.Translation.Direction.z", () => HMDIncrementalTranslationDirection.z.ToString());


            MmDataCollectorHead.Add("Head.Incremental.Rotation.Angle", () => HMDIncrementalRotationAngle.ToString());
            //MmDataCollectorHead.Add("Incremental.Rotation.Axis", () => incrementalRotationAxis.ToCSV('|'));
            MmDataCollectorHead.Add("Head.Incremental.Rotation.Axis.x", () => HMDIncrementalRotationAxis.x.ToString());
            MmDataCollectorHead.Add("Head.Incremental.Rotation.Axis.y", () => HMDIncrementalRotationAxis.y.ToString());
            MmDataCollectorHead.Add("Head.Incremental.Rotation.Axis.z", () => HMDIncrementalRotationAxis.z.ToString());

            MmDataCollectorHead.Add("Head.Cumul.Angle.Traveled", () => HMDCumulAngleTraveled.ToString());
            MmDataCollectorHead.Add("Head.Cumul.Distance.Traveled", () => HMDCumulDistanceTraveled.ToString());

            MmDataCollectorHead.Add("BadTracking", () => badTrackingDataObserved.ToString().ToUpper());

            if (!Directory.Exists(Path.Combine(UserInfo.DirPath, UserInfo.SequenceId.ToString())))
                Directory.CreateDirectory(Path.Combine(UserInfo.DirPath, UserInfo.SequenceId.ToString()));

            MmDataCollectorHead.CreateDataHandler(Path.Combine(UserInfo.DirPath, UserInfo.SequenceId.ToString()),
                CurrentTask.UserSequence + "_" + nameHead);
            MmDataCollectorHead.OpenTag();
        }

        public void SetupDataCollectorSummaryStats()
        {
            //TODO: Variables to be written at the end of a trial
            MmDataCollectorStats.Clear();

            //Task Information
            MmDataCollectorStats.Add("RecId", () => CurrentTask.RecordId.ToString());
            MmDataCollectorStats.Add("UserId", () => CurrentTask.UserId.ToString());
            MmDataCollectorStats.Add("UserSequence", () => CurrentTask.UserSequence.ToString());
            MmDataCollectorStats.Add("TrialId", () => CurrentTask.TaskId.ToString());
            MmDataCollectorStats.Add("Condition", () => CurrentTask.TaskName.ToString());
            MmDataCollectorStats.Add("NumOfQuads", () => CurrentTask.NumOfQuads.ToString());
            MmDataCollectorStats.Add("IsLeftSide", () => CurrentTask.LeftSide.ToString());

            MmDataCollectorStats.Add("Duration", () => trialTime.ToString("0.000"));

            //MmDataCollectorStats.Add("Position.Offset.x", () => positionOffset.x.ToString("0.000"));
            //MmDataCollectorStats.Add("Position.Offset.y", () => positionOffset.y.ToString("0.000"));
            //MmDataCollectorStats.Add("Position.Offset.z", () => positionOffset.z.ToString("0.000"));

            //MmDataCollectorStats.Add("Orientation.Offset.Axis.x", () => orientationOffset.x.ToString("0.000"));
            //MmDataCollectorStats.Add("Orientation.Offset.Axis.y", () => orientationOffset.y.ToString("0.000"));
            //MmDataCollectorStats.Add("Orientation.Offset.Axis.z", () => orientationOffset.z.ToString("0.000"));
            //MmDataCollectorStats.Add("Orientation.Offset.Axis.w", () => orientationOffset.w.ToString("0.000"));

            MmDataCollectorStats.Add("Is.Bad.Data", () => IsBadData.ToString().ToUpper());
            MmDataCollectorStats.Add("Is.Optout", () => IsOptOut.ToString().ToUpper());

            MmDataCollectorStats.Add("Completion.Entrance.Counter", () => CompletionEntranceCounter.ToString());
            MmDataCollectorStats.Add("Completion.Total.Time", () => CompletionTotalTime.ToString());

            MmDataCollectorStats.Add("Finetuning.Entrance.Counter", () => finetuningEntranceCounter.ToString());
            MmDataCollectorStats.Add("Finetuning.Total.Time", () => finetuningTotalTime.ToString());

            MmDataCollectorStats.Add("Cumul.Angle.Traveled", () => HMDCumulAngleTraveled.ToString());
            MmDataCollectorStats.Add("Cumul.Distance.Traveled", () => HMDCumulDistanceTraveled.ToString());

            int newItemCreated = MmDataCollectorStats.CreateDataHandler(UserInfo.DirPath, nameSummaryStats);

            //If file is empty, then set header to top
            if (newItemCreated == 1)
            {
                MmDataCollectorStats.OpenTag();
            }
        }





        public void Update()
        {
            trialTime += Time.deltaTime;

            if (prevHeadPositionandOrientationnotupdated)
            {
                prevHeadPositionandOrientationnotupdated = false;
                prevHeadPosition = currentResponder.HMD.transform.position;
                prevHeadOrientation = currentResponder.HMD.transform.rotation;

            }

            if (currentResponder == null ||
                currentResponder.HMD == null
                )
                return;
            // Incremental positional/orientational offset relative to prev frame

            HMDIncrementalTranslationDirection = currentResponder.HMD.transform.position - prevHeadPosition;
            HMDIncrementalTranslationDistance = HMDIncrementalTranslationDirection.magnitude;

            HMDIncrementalRotationAxis = Vector3.zero;
            HMDIncrementalRotationAngle = Math3d.CalcOptimalRotation(
                prevHeadOrientation,
                currentResponder.HMD.transform.rotation, out HMDIncrementalRotationAxis);

            badTrackingDataObserved = HMDIncrementalRotationAngle > badTrackingAngleThreshold
                                      || HMDIncrementalTranslationDistance > badTrackingDistanceThreshold;

            if (!badTrackingDataObserved)
            {
                prevHeadOrientation = currentResponder.HMD.transform.rotation;
                prevHeadPosition = currentResponder.HMD.transform.position;

                HMDCumulAngleTraveled += HMDIncrementalRotationAngle;
                HMDCumulDistanceTraveled += HMDIncrementalTranslationDistance;


            }

            //TODO: This should come from TaskInfo's Complete State that checks position and orientation
            // Calculate FineTuning vs. Localization
            /*
            if (orientationOffsetAngle < finetuningThreshold)
            {
                if (interactionMode.Current == interactionType.Localization)
                {
                    interactionMode.JumpTo(interactionType.Finetuning);
                }
                else
                {
                    finetuningTotalTime += Time.deltaTime;
                }
            }
            else
            {
                interactionMode.JumpTo(interactionType.Localization);
            }
            */

        }



        public void EndOfTrial()
        {
            MmDataCollectorStats.Write();
            //MmDataCollectorStats.CloseTag();

            MmDataCollectorStats.CloseDataHandler();
            MmDataCollectorHead.CloseDataHandler();


            MmDataCollectorStats.Clear();
            MmDataCollectorHead.Clear();

        }

        public void Clear()
        {
            trialTime = 0;

            CompletionEntranceCounter = 0;
            CompletionTotalTime = 0;

            prevHeadPosition = Vector3.zero;
            prevHeadOrientation = Quaternion.identity;
            prevHeadPositionandOrientationnotupdated = true;


            // Incremental Motion

            HMDIncrementalTranslationDirection = Vector3.zero;
            HMDIncrementalTranslationDistance = 0;

            HMDIncrementalRotationAxis = Vector3.zero;
            HMDIncrementalRotationAngle = 0;


            // Cumulative Stats

            HMDCumulAngleTraveled = 0;
            HMDCumulDistanceTraveled = 0;




            // Offsets from target

            //orientationOffset = Quaternion.identity;

            //positionOffset = Vector3.zero;

            badTrackingDataObserved = false;

            IsBadData = false;
            IsOptOut = false;

        }

        public void DeRegisterFMRGDataContainer()
        {

        }

        ~FMRGDataContainer()
        {
            DeRegisterFMRGDataContainer();
        }
    }
}


