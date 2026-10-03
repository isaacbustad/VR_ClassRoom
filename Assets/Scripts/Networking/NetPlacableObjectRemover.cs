// Created By   :   Isaac Bustad
// Created      :   6/11/2026
// Converted To :   FishNet on 10/3/2026

using System.Collections;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine;

namespace BugFreeProductions.VRClassroom
{
    public class NetPlacableObjectRemover : VR_PlacableItemRemoverGun
    {
        #region Vars
        [Tooltip("The FishNet NetworkObject component found on the parent object.")]
        [SerializeField] protected NetworkObject netObj = null;

        [Tooltip("Reference to the NetPlacableItemSpawner component on this object.")]
        protected NetPlacableItemSpawner npis = null;
        #endregion Vars

        #region Methods
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void CollectVars()
        {
            // Collect FishNet's NetworkObject instead of Mirror's NetworkIdentity
            netObj = GetComponentInParent<NetworkObject>();
            npis = GetComponent<NetPlacableItemSpawner>();

            base.CollectVars();
        }

        protected override void CastAndCheckforPlacement()
        {
            base.CastAndCheckforPlacement();
        }

        public override void UseRemover(bool aCon)
        {
            // Check server status using FishNet's NetworkObject property
            bool isServer = netObj != null && netObj.IsServer;

            if (isServer || NetGuestPermissionManager.GuestCanEdit)
            {
                base.UseRemover(aCon);
            }
        }

        protected override void RemoveObject()
        {
            bool isServer = netObj != null && netObj.IsServer;

            // Server-side direct item removal
            if (isServer && placableItemHighlighter != null)
            {
                var factoryItem = placableItemHighlighter.GetComponent<PlacableFactoryItem>();
                if (factoryItem != null)
                {
                    factoryItem.RemoveItem();
                }
                return;
            }

            // Client-side network request: pass the target NetworkObject reference directly 
            // to the spawner, avoiding all manual ID lookups and indexer adaptations.
            bool isClient = npis != null && npis.IsClient;
            if (isClient && NetGuestPermissionManager.GuestCanEdit && placableItemHighlighter != null)
            {
                NetworkObject targetNetObj = placableItemHighlighter.GetComponent<NetworkObject>();
                if (targetNetObj != null)
                {
                    npis.RequestNetworkItemDeSpawn(targetNetObj);
                }
                return;
            }
        }

        protected override void DrawRemovalLine()
        {
            base.DrawRemovalLine();
        }

        #endregion Methods
    }
}