// Created By   :   Isaac Bustad
// Created      :   5/30/2026 (Updated 6/5/2026)
// Converted to FishNet for BugFreeProductions.VRClassroom - 10/3/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

namespace BugFreeProductions.VRClassroom
{
    /// <summary>
    /// Manages network spawning and despawning requests for placable items 
    /// using FishNet ServerRpcs and server-authoritative management.
    /// </summary>
    public class NetPlacableItemSpawner : NetworkBehaviour
    {
        [SerializeField] protected AbstractFactory_SCO itemFactory = null;

        #region Methods

        /// <summary>
        /// Requests the server to spawn a network item using serialized placement data.
        /// </summary>
        public virtual void RequestNetworkItemSpawn(NetPlacementData netPlacementData)
        {
            Debug.Log("request placed");
            CmdSpawnNetworkItem(netPlacementData);
        }

        /// <summary>
        /// Requests the server to despawn a network item using its NetworkObject reference.
        /// </summary>
        public virtual void RequestNetworkItemDeSpawn(NetworkObject targetNetObj)
        {
            CmdRequestNetworkItemDeSpawn(targetNetObj);
        }

        /// <summary>
        /// ServerRpc executed on the server to instantiate and spawn the requested item.
        /// RequireOwnership = false allows clients to initiate spawns without owning this spawner object.
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        protected virtual void CmdSpawnNetworkItem(NetPlacementData netPlacementData)
        {
            if (!base.IsServerStarted) return;

            if (itemFactory != null)
            {
                Debug.Log("requested Command run");
                
                // Create reference for the factory item
                FactoryItem factoryItem = null;

                // Create an item and instantiate it locally on the server
                itemFactory.CreateItem(ref factoryItem, netPlacementData.ToObjectPlacement());

                if (factoryItem != null)
                {
                    // Get reference of the game object for spawning on the network
                    GameObject instance = factoryItem.gameObject;

                    // Spawn on the network using FishNet's ServerManager
                    base.ServerManager.Spawn(instance);
                }
            }
            else
            {
                Debug.Log("Abstract Factory reference is not assigned");
            }
        }

        /// <summary>
        /// ServerRpc executed on the server to despawn an item via its NetworkObject reference.
        /// RequireOwnership = false allows authorized clients to request despawns.
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        protected virtual void CmdRequestNetworkItemDeSpawn(NetworkObject targetNetObj)
        {
            if (!base.IsServerStarted) return;

            if (targetNetObj != null)
            {
                // Despawn and clean up the object across the network using FishNet's ServerManager
                base.ServerManager.Despawn(targetNetObj.gameObject);
            }
        }

        #endregion Methods
    }
}