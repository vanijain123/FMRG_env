using System;
using System.Collections.Generic;
using UnityEngine;
using MercuryMessaging;
using MercuryMessaging.Support.Data;
using MercuryMessaging.Task;
using MercuryMessaging.Support.Extensions;

namespace Projects.FMRG.Scripts.UserStudy
{
    public enum FMRGMethods
    {

        AssignFMRGTaskInfo = 4000,
        AssignTaskIndex
    }

    public class FMRGTaskResponder : MmTaskResponder<FMRGTaskInfo>
    {


        /// <summary>
        /// Handle to the scene's HMD GameObject
        /// </summary>
        public GameObject HMD;

        /// <summary>
        /// Handle to the scene's HMD camera RIG
        /// </summary>
        //public DefaultSceneManagerMR defaultSceneManager;

        public float ZDistRequired;


        public override void Awake()
        {
            base.Awake();
        }

        /// <summary>
        /// Task completion check determines completion based on 
        /// TransformationTypeMode.
        /// Supports checking multiple types of transformations simultaneously.
        /// </summary>
        /// <returns>True on task complete.</returns>
        public override bool TaskCompleteCheck()
        {
            return true;
            //return defaultSceneManager.cart.Count == defaultSceneManager.TargetItemCount && HMD.transform.position.z > ZDistRequired;
        }





        /// <summary>
        /// Override of the MmBaseResponder's Initialization function
        /// Use with the MmMethod.Initialize command.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }



        /// <summary>
        /// Reset calculation values to identity or zero.
        /// </summary>
        public virtual void Reset()
        {
        }

        public override void MmInvoke(MmMessageType msgType, MmMessage message)
        {
            var type = message.MmMethod;

            switch (type)
            {
                case (MmMethod)FMRGMethods.AssignFMRGTaskInfo:
                    break;
                case (MmMethod)FMRGMethods.AssignTaskIndex:
                    break;
                default:
                    base.MmInvoke(msgType, message);
                    break;
            }
        }

        public override void Update()
        {
            base.Update();


        }
    }
}
