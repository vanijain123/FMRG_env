using MercuryMessaging;
using Projects.FMRG.Scripts.UserStudy;
using UnityEngine;

namespace Projects.FMRG.Scripts
{
    public class FMRGSwitchResponder : MmSwitchResponder
    {

        public FMRGMmAppStateSwitchResponder FMRGMmAppState;
        /*
        void Awake()
        {
            MmLogger.LogApplication("SlSwitchResponder Awake");

            base.Awake();

            foreach (var routingTableItem in MmRelaySwitchNode.RoutingTable)
            {
                var avatarComboController = routingTableItem.Responder.GetComponent<SpBVisualizationController>();

                if (avatarComboController == null)
                    continue;

                //TODO: Set fields for the Visualization controller here           
            }
        }

        void Start()
        {
            MmLogger.LogFramework("SlSwitchResponder Start.");

            MmRelaySwitchNode.RespondersFSM.GlobalExit += delegate
            {
                MmRelaySwitchNode.Current.MmInvoke(MmMethod.SetActive, false,
                    new MmMetadataBlock(MmLevelFilterHelper.Default, MmActiveFilter.All,
                    default(MmSelectedFilter), MmNetworkFilter.Local));
            };

            MmRelaySwitchNode.RespondersFSM.GlobalEnter += delegate
            {
                //Debug.Log("Entering Switch for " + MmRelaySwitchNode.Current.name);
                MmRelaySwitchNode.Current.MmInvoke(MmMethod.SetActive, true,
                    new MmMetadataBlock(MmLevelFilterHelper.Default, MmActiveFilter.All,
                    default(MmSelectedFilter), MmNetworkFilter.Local));
            };

            MmOnStartComplete();
        }
        */

        protected override void Complete(bool active)
        {
            if (!active) return;
            ////

            if (FMRGMmAppState.CurrentState != FMRGMmAppStateSwitchResponder.AppState.Trial) return;

            //defaultSceneManager.logger.IsCompleteMode = true;
           // defaultSceneManager.logger.IsCompleteStart = Time.time;
           // defaultSceneManager.logger.CompletionEntranceCounter++;

        }
    }
}
