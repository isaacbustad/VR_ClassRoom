// Created By   :   Isaac Bustad
// Created      :   6/11/2026
// Converted To :   FishNet on 10/3/2026

using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BugFreeProductions.VRClassroom
{
    public class NetPlacableObjectPlacer : VR_PlacableItemPlacerGun
    {
        #region Vars
        [Tooltip("The FishNet NetworkObject component found on a parent object.")]
        [SerializeField] protected NetworkObject netObj = null;

        [Tooltip("Reference for the networked item spawner component.")]
        protected NetPlacableItemSpawner netItemSpawner = null;
        #endregion Vars

        #region Methods
        protected override void OnEnable()
        {
            // Ensure we have a reference to the parent FishNet NetworkObject
            if (netObj == null)
            {
                netObj = gameObject.GetComponentInParent<NetworkObject>();
            }

            base.OnEnable();
        }

        protected override void FixedUpdate()
        {
            bool isServer = netObj != null && netObj.IsServer;

            if (isServer || NetGuestPermissionManager.GuestCanEdit)
            {
                base.FixedUpdate();
            }
        }

        protected override void CollectVars()
        {
            // Get the NetPlacableItemSpawner reference
            netItemSpawner = GetComponent<NetPlacableItemSpawner>();
            netObj = GetComponentInParent<NetworkObject>();

            base.CollectVars();
        }

        public override void UsePlacer(bool aCon)
        {
            Debug.Log("Use Placer");
            
            bool isServer = netObj != null && netObj.IsServer;
            bool isClient = netObj != null && netObj.IsClient;

            if (isServer)
            {
                base.UsePlacer(aCon);
                return;
            }

            if (isClient && NetGuestPermissionManager.GuestCanEdit)
            {
                base.UsePlacer(aCon);
                return;
            }
        }

        protected override void PlaceItem()
        {
            bool isServer = netObj != null && netObj.IsServer;

            if (isServer)
            {
                base.PlaceItem();

                if (placableFactoryItem != null)
                {
                    // Spawn the placed item across the network using FishNet's ServerManager
                    InstanceFinder.ServerManager.Spawn(placableFactoryItem.gameObject);
                }
                return;
            }

            // For players/guests to request item placement
            if (NetGuestPermissionManager.GuestCanEdit)
            {
                if (netItemSpawner != null)
                {
                    Debug.Log("requesting spawn");
                    netItemSpawner.RequestNetworkItemSpawn(placableFactoryItem.ObjectPlacement().ToNetPlacementData());
                }
                Destroy(placableFactoryItem.gameObject);
                return;
            }
        }

        protected override ObjectPlacement CalcObjectPlacementData()
        {
            return base.CalcObjectPlacementData();
        }

        protected override void CastAndCheckforPlacement()
        {
            base.CastAndCheckforPlacement();
        }

        protected override void DrawPlacementLine()
        {
            base.DrawPlacementLine();
        }

        public override void SaveRoomConfig()
        {
            if (netItemSpawner != null && netItemSpawner.IsServer)
            {
                JSONPlacementMannager.Instance.WriteRoomConfig();
                return;
            }

            if (netItemSpawner != null && netItemSpawner.IsClient && NetGuestPermissionManager.GuestCanSave)
            {
                JSONPlacementMannager.Instance.WriteRoomConfig();
                return;
            }
        }

        #endregion Methods
    }
}