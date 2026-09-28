// Created By   :   Isaac Bustad
// Created      :   5/30/2026

using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;



namespace BugFreeProductions.VRClassroom
{

    public class NetPlacableItem : PlacableFactoryItem
    {
        #region Vars
        protected NetworkIdentity ni = null;
        protected NetPlacableItemSpawnable netSpawnable = null;
        
        #endregion Vars

        #region Methods
        public override void RemoveItem()
        {
            // if (ni.isServer)
            // {
            //     NetworkServer.Destroy(gameObject);
            //     return;
            // }

            // if (ni.isClient && NetGuestPermissionManager.GuestCanEdit)
            // {
            //     netSpawnable.RemoveItem();
            //     return;
            // }
            Unsubscribe();
            NetworkServer.Destroy(gameObject);
            
        }

        protected override void CollectVars()
        {
            ni = GetComponent<NetworkIdentity>();
            netSpawnable = GetComponent<NetPlacableItemSpawnable>();

            base.CollectVars();
            // // get and default Rigidbody
            // rb = GetComponent<Rigidbody>();
            // rb.freezeRotation = true;

            // // collect bodyScript
            // body = GetComponent<PlacableFactoryItemBody>();
        }

         

        
        #endregion Methods
    }
}