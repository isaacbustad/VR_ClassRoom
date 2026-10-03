// Created By   :   Isaac
// Created      :   5/14/2026
// Gemini Assisted
// Converted To :   FishNet on 10/3/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

namespace BugFreeProductions.VRClassroom
{
    /// <summary>
    /// Manages the activation of the local VR rig and remote avatar visuals 
    /// based on FishNet object ownership.
    /// </summary>
    public class VRPlayerSetup : NetworkBehaviour
    {
        #region Vars
        [Tooltip("Local VR rig containing the camera, tracking nodes, and local controllers.")]
        [SerializeField] private GameObject localVRRig; 

        [Tooltip("Remote avatar visuals displayed to other connected clients.")]
        [SerializeField] private GameObject remoteAvatar; 
        #endregion Vars

        #region Methods
        /// <summary>
        /// FishNet lifecycle method called on all clients when this network object initializes.
        /// </summary>
        public override void OnStartClient()
        {
            base.OnStartClient();
            
            // Check if the local client owns this player instance (FishNet equivalent to Mirror's isLocalPlayer)
            if (base.IsOwner)
            {
                SetupLocalPlayer();
            }
            else
            {
                SetupRemotePlayer();
            }
        }

        /// <summary>
        /// Enables local tracking/camera rig and hides the remote avatar for the local player.
        /// </summary>
        private void SetupLocalPlayer()
        {
            if (localVRRig != null) localVRRig.SetActive(true);
            if (remoteAvatar != null) remoteAvatar.SetActive(false);
        }

        /// <summary>
        /// Disables the local tracking rig and enables remote avatar visuals for other players.
        /// </summary>
        private void SetupRemotePlayer()
        {
            if (localVRRig != null) localVRRig.SetActive(false);
            if (remoteAvatar != null) remoteAvatar.SetActive(true);
        }
        #endregion Methods
    }
}