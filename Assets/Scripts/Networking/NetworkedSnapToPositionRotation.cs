// Created By   :   Isaac Bustad
// Converted To :   FishNet on 10/3/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

namespace BugFreeProductions.VRClassroom
{
    /// <summary>
    /// Snaps the network transform position and rotation to a tracked target 
    /// on the client that owns this object during LateUpdate.
    /// </summary>
    public class NetworkedSnapToPositionRotation : NetworkBehaviour
    {
        #region Vars
        [Tooltip("The target transform being tracked (e.g., VR headset or controller tracking node).")]
        [SerializeField] protected Transform trackedTf = null;
        #endregion Vars

        #region Methods
        protected virtual void LateUpdate()
        {
            // Only the owner updates and syncs its local tracking data
            if (base.IsOwner)
            {
                if (trackedTf != null)
                {
                    transform.position = trackedTf.position;
                    transform.rotation = trackedTf.rotation;
                }
            }
        }
        #endregion Methods
    }
}