// Created By   :   Isaac Bustad
// Created      :   6/18/2026
// Converted to FishNet for BugFreeProductions.VRClassroom - 10/3/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

namespace BugFreeProductions.VRClassroom
{
    /// <summary>
    /// Manages item memento recording permissions, integrating with FishNet network states
    /// and guest permission rules to control user access to the memento and recording system.
    /// </summary>
    public class NetItemMementoManager : ItemMementoManager
    {
        #region Vars
        // FishNet equivalent of Mirror's NetworkIdentity
        protected NetworkObject networkObj = null;
        #endregion Vars

        #region Methods

        protected virtual void Awake()
        {
            // Cache the FishNet NetworkObject component reference
            networkObj = GetComponent<NetworkObject>();
        }

        // protected override void OnEnable()
        // {
        //     base.OnEnable();
        // }

        /// <summary>
        /// Tests whether the user is permitted to use the memento system and recording features based on server status or guest permissions.
        /// </summary>
        protected override void TestByKey()
        {
            // Check if the permission manager instance is on the server using FishNet's PascalCase IsServer property
            if (NetGuestPermissionManager.Instance != null && NetGuestPermissionManager.Instance.IsServer)
            {
                base.TestByKey();
                return;
            }

            // Check if guests are explicitly allowed to record
            if (NetGuestPermissionManager.GuestCanRecord)
            {
                base.TestByKey();
                return;
            }
        }

        #endregion Methods
    }
}