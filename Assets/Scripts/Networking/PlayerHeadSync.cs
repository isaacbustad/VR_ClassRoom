// Created By   :   Isaac Bustad
// Created      :   5/14/2026
// Converted To :   FishNet on 10/3/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

namespace BugFreeProductions.VRClassroom
{
    /// <summary>
    /// Synchronizes the local VR headset camera transform to a head proxy object 
    /// on the client that owns this network player.
    /// </summary>
    public class VRPlayerSync : NetworkBehaviour
    {
        #region Vars
        [Tooltip("Target proxy transform (e.g., visual head representation) to sync with the camera.")]
        [SerializeField] private Transform headProxy; 
        
        private Transform mainCameraTransform;
        #endregion Vars

        #region Methods
        /// <summary>
        /// FishNet lifecycle method called on all clients when this network object starts.
        /// </summary>
        public override void OnStartClient()
        {
            base.OnStartClient();

            // Find the camera on the local machine only if this client owns the player object
            if (base.IsOwner)
            {
                if (Camera.main != null)
                {
                    mainCameraTransform = Camera.main.transform;
                }
            }
        }

        private void Update()
        {
            // Only the owner of this player should update the head proxy position and rotation
            if (base.IsOwner && mainCameraTransform != null && headProxy != null)
            {
                headProxy.position = mainCameraTransform.position;
                headProxy.rotation = mainCameraTransform.rotation;
            }
        }
        #endregion Methods
    }
}