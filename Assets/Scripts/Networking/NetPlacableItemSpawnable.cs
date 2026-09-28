// Created By   :   Isaac Bustad
// Created      :   6/24/2026


using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


namespace BugFreeProductions.VRClassroom
{
    public class NetPlacableItemSpawnable : NetworkBehaviour
    {
        public override void OnStartClient()
        {
            base.OnStartClient();

            NetPlacableItem netPlacableItem = GetComponent<NetPlacableItem>();

            if (netPlacableItem != null)
            {
                netPlacableItem.FinalizePlacement();
            }
        }

        public virtual void RemoveItem()
        {
            CmdRemoveItem();
        }

        
        [Command]
        public virtual void CmdRemoveItem()
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}