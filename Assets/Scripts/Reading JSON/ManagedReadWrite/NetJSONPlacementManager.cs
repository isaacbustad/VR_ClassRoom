// Created By   :   Isaac Bustad
// Created      :   6/26/2026
// Converted To :   FishNet on 10/3/2026

using System.Collections;
using System.Collections.Generic;
using FishNet;
using FishNet.Object;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BugFreeProductions.VRClassroom
{
    [RequireComponent(typeof(NetworkObject))]
    public class NetJSONPlacementManager : JSONPlacementMannager
    {
        #region Vars
        [Tooltip("Reference to the FishNet NetworkObject component.")]
        protected NetworkObject netObj = null;
        #endregion Vars

        #region Methods
        protected override void Setup()
        {
            // Do the basic setup from base class
            base.Setup();

            // Get NetworkObject to use FishNet attributes and checks
            netObj = GetComponent<NetworkObject>();
        }

        protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            base.OnSceneLoaded(scene, mode);
            
            // Only the server should handle spawning networked items upon scene load
            if (InstanceFinder.IsServer)
            {
                // Loop through subscribers and spawn any placable items across the network
                foreach (Subscriber aSub in subscribers)
                {
                    if (aSub is NetPlacableItem anItem)
                    {
                        InstanceFinder.ServerManager.Spawn(anItem.gameObject);
                    }
                }
            }
        }

        /*
        public override void WriteRoomConfig()
        {
            if (netObj != null && netObj.IsServer)
            {
                base.WriteRoomConfig();
            }
        }
        */
        
        #endregion Methods
    }
}