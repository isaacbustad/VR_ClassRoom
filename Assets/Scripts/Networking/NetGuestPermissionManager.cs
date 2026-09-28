// Created By   :   Isaac Bustad
// Created      :   6/8/2026

using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace BugFreeProductions.VRClassroom
{
    public class NetGuestPermissionManager : NetworkBehaviour
    {
        #region Vars
        protected static NetGuestPermissionManager instance = null;

        // actions that need to be run on variable updates
        public event Action<NetGuestPermission> OnPermissionsChanged;

        #region Synced and Network Vars
        // network and synced variables
        [SyncVar(hook = nameof(OnPermissionDataChanged))] protected NetGuestPermission netGuestPermission = new NetGuestPermission();


        #endregion Synced and Network Vars
        #endregion Vars

        #region Methods
        // Write the matching hook method
        protected virtual void OnPermissionDataChanged(NetGuestPermission oldData, NetGuestPermission newData)
        {
            // Optional: Protect against unnecessary execution if the data didn't actually change
            // (Note: Structs compare all fields automatically when using Equals or == if implemented)
            if (oldData.Equals(newData)) return;

            // Broadcast the update to your local UI subscribers
            OnPermissionsChanged?.Invoke(netGuestPermission);
            Debug.Log("Data = bool can edit : " + netGuestPermission.guestCanEdit);
        }
        protected virtual void OnEnable()
        {
            if (instance == null)
            {
                instance = this;
            }

            // else
            // {
            //     Destroy(gameObject);
            // }


        }
        public override void OnStartClient()
        {
            OnPermissionsChanged?.Invoke(netGuestPermission);
        }

        #region Toggles

        // Only the owner should be able to request 
        // the guest placing permission changing
        public virtual void ToggleGuestCanEdit()
        {
            // check if is owned by the local object 
            if (isServer)
            {
                NetGuestPermission nNetGuestPermission = netGuestPermission;

                nNetGuestPermission.guestCanEdit = !nNetGuestPermission.guestCanEdit;

                netGuestPermission = nNetGuestPermission;
                // request the permission be toggled via command
                //CmdToggleGuestCanEdit();
            }


        }

        public virtual void ToggleGuestCanRecord()
        {
            // check if is owned by the local object 
            if (isServer)
            {
                NetGuestPermission nNetGuestPermission = netGuestPermission;

                nNetGuestPermission.guestCanRecord = !nNetGuestPermission.guestCanRecord;

                netGuestPermission = nNetGuestPermission;
                // request the permission be toggled via command
                //CmdToggleGuestCanEdit();
            }


        }

        public virtual void ToggleGuestCanSave()
        {
            // check if is owned by the local object 
            if (isServer)
            {
                // hold current permission value
                NetGuestPermission nNetGuestPermission = netGuestPermission;

                // edit current permissions
                nNetGuestPermission.guestCanSave = !nNetGuestPermission.guestCanSave;

                // reassign permissions to take effect
                netGuestPermission = nNetGuestPermission;
                
            }
        }

        public virtual void ToggleGuestCanReplay()
        {
            // check if is owned by the local object 
            if (isServer)
            {
                // hold current permission value
                NetGuestPermission nNetGuestPermission = netGuestPermission;

                // edit current permissions
                nNetGuestPermission.guestCanReplay = !nNetGuestPermission.guestCanReplay;

                // reassign permissions to take effect
                netGuestPermission = nNetGuestPermission;
                
            }
        }

        // command to request server toggle permission 
        // [Command] protected virtual void CmdToggleGuestCanEdit()
        // {
        //     if (isServer)
        //     {
        //         guestCanEdit = !guestCanEdit;
        //     }
        // }
        #endregion Toggles

        #region Functional Methods
        // static for easy call
        public static void SaveRoom()
        {
            instance.OnSaveRoom();
        }

        // overridable for easy change in childeren
        protected virtual void OnSaveRoom()
        {
            if (isServer)
            {
                JSONPlacementMannager.Instance.WriteRoomConfig();
                return;
            }

            if (netGuestPermission.guestCanSave)
            {
                JSONPlacementMannager.Instance.WriteRoomConfig();
                return;
            }
            
        }

        #endregion Functional Methods

        #endregion Methods

        #region Accessors


        public static NetGuestPermissionManager Instance
        {
            get
            {
                return instance;
            }
        }

        public static NetGuestPermission NetGuestPermission
        {
            get
            {
                return instance.netGuestPermission;
            }
        }

        public static bool GuestCanEdit
        {
            get
            {
                return instance.netGuestPermission.guestCanEdit;
            }

        }

        public static bool GuestCanRecord
        {
            get
            {
                return instance.netGuestPermission.guestCanRecord;
            }
        }

        public static bool GuestCanSave
        {
            get
            {
                return instance.netGuestPermission.guestCanSave;
            }
        }

        public static bool GuestCanReplay
        {
            get
            {
                return instance.netGuestPermission.guestCanReplay;
            }
        }
        #endregion Accessors
    }


}