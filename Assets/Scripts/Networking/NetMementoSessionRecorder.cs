// Created by   :   Isaac Busatd
// Created      :   6/28/2026


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BugFreeProductions.VRClassroom
{
    
    public class NetMementoSessionRecorder : MementoSessionRecorder
    {
        #region Vars

        #endregion Vars


        #region Methods
        public override void StartRecordingSession()
        {
            if (NetGuestPermissionManager.Instance.isServer)
            {
                base.StartRecordingSession();
                return;
            }
            
            if (NetGuestPermissionManager.GuestCanRecord)
            {
                base.StartRecordingSession();
                return;
            }
        }

        protected virtual void OnPermissionsChanged(NetGuestPermission aNetGuestPermission)
        {
            if(NetGuestPermissionManager.Instance.isServer)
            {
                return;
            }

            if (!NetGuestPermissionManager.GuestCanRecord)
            {
                StopRecordingSession();
            }
        }

        #region Subscriber Methods
        // // method to recieve update from subscrition
        // public void OnNotify()
        // {

        // }

        // // method to subscribe to SubscriptionService
        // public void Subscribe()
        // {
        //     NetGuestPermissionManager.
        // }

        // // method to unsubscribe from SubscriptionService
        // public void Unsubscribe()
        // {

        // }

        #endregion Subscriber Methods

        #endregion Methods


        #region Constructors
        protected NetMementoSessionRecorder():base()
        {
            NetGuestPermissionManager.Instance.OnPermissionsChanged += OnPermissionsChanged;
        }

        #endregion Constructors


        #region Accessors
        
        #endregion Accessors
    }

}