// Created By   :   Isaac Bustad
// Created      :   5/30/2026
// Converted To :   FishNet on 10/3/2026

using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using UnityEngine;

namespace BugFreeProductions.VRClassroom
{
    public class NetPlacableItem : PlacableFactoryItem
    {
        #region Vars
        [Tooltip("The FishNet NetworkObject component attached to this item.")]
        protected NetworkObject netObj = null;

        protected NetPlacableItemSpawnable netSpawnable = null;
        #endregion Vars

        #region Methods
        public override void RemoveItem()
        {
            Unsubscribe();

            // Despawn the object across the network using FishNet's NetworkObject
            if (netObj != null && netObj.IsServer)
            {
                netObj.Despawn();
            }
            else if (InstanceFinder.IsServer)
            {
                InstanceFinder.ServerManager.Despawn(gameObject);
            }
        }

        protected override void CollectVars()
        {
            // Collect FishNet's NetworkObject instead of Mirror's NetworkIdentity
            netObj = GetComponent<NetworkObject>();
            netSpawnable = GetComponent<NetPlacableItemSpawnable>();

            base.CollectVars();
        }

        #endregion Methods
    }
}