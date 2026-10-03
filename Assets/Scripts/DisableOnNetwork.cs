// Created By   :   Isaac Bustad
// Converted to FishNet for BugFreeProductions.VRClassroom

using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

namespace BugFreeProductions.VRClassroom
{
    public class DisableOnNetwork : NetworkBehaviour
    {
        #region Vars
        [Tooltip("GameObjects that should only be active/enabled when controlled by the local client.")]
        [SerializeField] protected List<GameObject> toDisableLst = new List<GameObject>();
        
        [Tooltip("MonoBehaviour components that should only run when controlled by the local client.")]
        [SerializeField] protected MonoBehaviour[] localOnlyComponents;
        #endregion Vars

        #region Methods

        /// <summary>
        /// Automatically called by FishNet on clients when this network object finishes initializing.
        /// </summary>
        public override void OnStartClient()
        {
            base.OnStartClient();

            // Evaluate ownership status upon client startup
            NetEnable();
        }

        /// <summary>
        /// Enables or disables local-only game objects and components based on network ownership.
        /// </summary>
        protected virtual void NetEnable()
        {
            // Check if this specific client instance owns this network object (FishNet equivalent to Mirror's isOwned)
            if (base.IsOwner)
            {
                // Ensure the main GameObject is active for the owner
                gameObject.SetActive(true);

                // Enable all companion GameObjects in the list
                foreach (GameObject go in toDisableLst)
                {
                    if (go != null)
                    {
                        go.SetActive(true);
                    }
                }

                // Enable all local-only components (e.g., local cameras, input handlers)
                foreach (MonoBehaviour comp in localOnlyComponents)
                {
                    if (comp != null)
                    {
                        comp.enabled = true;
                    }
                }
            }
            else
            {
                // If this client does not own the object, you can optionally 
                // ensure local-only features remain disabled or hidden.
            }
        }

        #endregion Methods
    }
}