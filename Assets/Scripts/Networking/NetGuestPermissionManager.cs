// Created By   :   Isaac Bustad
// Created      :   6/8/2026
// Converted to FishNet for BugFreeProductions.VRClassroom - 10/3/2026

using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

namespace BugFreeProductions.VRClassroom
{
    /// <summary>
    /// Manages guest permissions across the network using FishNet SyncVars,
    /// allowing the server to control and broadcast guest capabilities (editing, recording, saving, replaying).
    /// </summary>
    public class NetGuestPermissionManager : NetworkBehaviour
    {
        #region Vars
        protected static NetGuestPermissionManager instance = null;

        // Actions triggered when permission values update
        public event Action<NetGuestPermission> OnPermissionsChanged;

        #region Synced and Network Vars
        // Synchronized guest permission struct managed via FishNet SyncVar
        public readonly SyncVar<NetGuestPermission> netGuestPermission = new SyncVar<NetGuestPermission>();
        #endregion Synced and Network Vars
        #endregion Vars

        #region Methods

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            // Subscribe to FishNet SyncVar value change events
            netGuestPermission.OnChange += OnPermissionDataChanged;
        }

        // Hook method triggered when the SyncVar value changes on clients or server
        protected virtual void OnPermissionDataChanged(NetGuestPermission oldData, NetGuestPermission newData, bool asServer)
        {
            // Protect against unnecessary execution if the data didn't actually change
            if (oldData.Equals(newData)) return;

            // Broadcast the update to local UI subscribers
            OnPermissionsChanged?.Invoke(newData);
            Debug.Log("Data = bool can edit : " + newData.guestCanEdit);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            // Ensure local subscribers get the initial synchronized state upon starting the client
            OnPermissionsChanged?.Invoke(netGuestPermission.Value);
        }

        #region Toggles

        // Toggle whether guests are permitted to edit the environment (Server-authoritative)
        public virtual void ToggleGuestCanEdit()
        {
            if (base.IsServer)
            {
                NetGuestPermission nNetGuestPermission = netGuestPermission.Value;
                nNetGuestPermission.guestCanEdit = !nNetGuestPermission.guestCanEdit;
                netGuestPermission.Value = nNetGuestPermission;
            }
        }

        // Toggle whether guests are permitted to record sessions (Server-authoritative)
        public virtual void ToggleGuestCanRecord()
        {
            if (base.IsServer)
            {
                NetGuestPermission nNetGuestPermission = netGuestPermission.Value;
                nNetGuestPermission.guestCanRecord = !nNetGuestPermission.guestCanRecord;
                netGuestPermission.Value = nNetGuestPermission;
            }
        }

        // Toggle whether guests are permitted to save room configurations (Server-authoritative)
        public virtual void ToggleGuestCanSave()
        {
            if (base.IsServer)
            {
                NetGuestPermission nNetGuestPermission = netGuestPermission.Value;
                nNetGuestPermission.guestCanSave = !nNetGuestPermission.guestCanSave;
                netGuestPermission.Value = nNetGuestPermission;
            }
        }

        // Toggle whether guests are permitted to replay sessions (Server-authoritative)
        public virtual void ToggleGuestCanReplay()
        {
            if (base.IsServer)
            {
                NetGuestPermission nNetGuestPermission = netGuestPermission.Value;
                nNetGuestPermission.guestCanReplay = !nNetGuestPermission.guestCanReplay;
                netGuestPermission.Value = nNetGuestPermission;
            }
        }

        #endregion Toggles

        #region Functional Methods
        
        // Static helper for external classes to trigger room saving
        public static void SaveRoom()
        {
            if (instance != null)
            {
                instance.OnSaveRoom();
            }
        }

        // Overridable method handling the room saving logic based on server status or permissions
        protected virtual void OnSaveRoom()
        {
            if (base.IsServer)
            {
                JSONPlacementMannager.Instance.WriteRoomConfig();
                return;
            }

            if (netGuestPermission.Value.guestCanSave)
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
                return instance != null ? instance.netGuestPermission.Value : default;
            }
        }

        public static bool GuestCanEdit
        {
            get
            {
                return instance != null && instance.netGuestPermission.Value.guestCanEdit;
            }
        }

        public static bool GuestCanRecord
        {
            get
            {
                return instance != null && instance.netGuestPermission.Value.guestCanRecord;
            }
        }

        public static bool GuestCanSave
        {
            get
            {
                return instance != null && instance.netGuestPermission.Value.guestCanSave;
            }
        }

        public static bool GuestCanReplay
        {
            get
            {
                return instance != null && instance.netGuestPermission.Value.guestCanReplay;
            }
        }

        #endregion Accessors
    }
}