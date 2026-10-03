// Created By   :   Isaac Bustad
// Created      :   6/24/2026
// Converted to FishNet for BugFreeProductions.VRClassroom - 10/3/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

namespace BugFreeProductions.VRClassroom
{
    /// <summary>
    /// Handles network spawning initialization and server-authoritative removal for placable items using FishNet.
    /// </summary>
    public class NetPlacableItemSpawnable : NetworkBehaviour
    {
        #region Methods

        /// <summary>
        /// Called automatically by FishNet on clients when the object finishes initializing.
        /// Finalizes the visual and structural placement of the item.
        /// </summary>
        public override void OnStartClient()
        {
            base.OnStartClient();

            NetPlacableItem netPlacableItem = GetComponent<NetPlacableItem>();

            if (netPlacableItem != null)
            {
                netPlacableItem.FinalizePlacement();
            }
        }

        /// <summary>
        /// Public helper to request item removal from the client.
        /// </summary>
        public virtual void RemoveItem()
        {
            ServerCmdRemoveItem();
        }
        
        /// <summary>
        /// ServerRpc executed on the server to handle item destruction/despawning.
        /// RequireOwnership is set to false to allow authorized guests (who may not own the object) to remove it.
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        public virtual void ServerCmdRemoveItem()
        {
            // Ensure execution occurs on the server side
            if (base.IsServer)
            {
                // Despawn and destroy the network object across all clients using FishNet's ServerManager
                base.ServerManager.Despawn(gameObject);
            }
        }

        #endregion Methods
    }
}