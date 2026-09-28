// Created By   :   Isaac Bustad
// Created      :   6/5/2026

using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


namespace BugFreeProductions.VRClassroom
{
    public class NetPlacableItemSpawner : NetworkBehaviour
    {
        [SerializeField] protected AbstractFactory_SCO itemFactory = null;

        #region Methods

        public virtual void RequestNetworkItemSpawn(NetPlacementData netPlacementData)
        {
            // call the network spawn command
            Debug.Log("request placed");
            CmdSpawnNetworkItem(netPlacementData);
        }
        public virtual void RequestNetworkItemDeSpawn(uint aNI)
        {
            // call the network spawn command
            CmdRequestNetworkItemDeSpawn(aNI);
        }

        [Command] protected virtual void CmdSpawnNetworkItem(NetPlacementData netPlacementData)
        {
            if (itemFactory != null)
            {
                Debug.Log("requested Commang run");
                // create reference
                FactoryItem factoryItem = null;

                // create an item and instantiate locally
                itemFactory.CreateItem(ref factoryItem, netPlacementData.ToObjectPlacement());

                // get reference of game object for spawning on network
                GameObject instance = factoryItem.gameObject;

                // spawn on the network
                NetworkServer.Spawn(instance);
            }

            else
            {
                Debug.Log("Abstract Factory reference is not assigned");
            }
            //NetworkServer.Spawn();
        }

        [Command] protected virtual void CmdRequestNetworkItemDeSpawn(uint aNI)
        {
            if (NetworkServer.spawned.TryGetValue(aNI, out NetworkIdentity identity))
            {
                GameObject obj = identity.gameObject;
                // Handle your server-side logic here

                NetworkServer.Destroy(obj);
            }
        }
        #endregion Methods
    }
}