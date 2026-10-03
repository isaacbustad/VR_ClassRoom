// Created By   :   Isaac Bustad
// Created      :   6/22/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;


namespace BugFreeProductions.VRClassroom
{    
    public class NetXrInteractable : NetworkBehaviour
    {
        #region Vars
        protected UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable = null;
        #endregion Vars

        #region Methods

        public override void OnStartClient()
        {
            base.OnStartClient();
            Debug.Log($"[FishNet] Client started for object: {base.ObjectId}");
        }

        protected virtual void OnEnable()
        {
            Setup();
        }

        protected virtual void Setup()
        {
            grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            
            // CRITICAL STEP: Prevent XRI from trying to track the object across the network
            // until FishNet says we officially have the authority to move it.
            if (grabInteractable != null && !base.IsOwner)
            {
                grabInteractable.trackPosition = false;
                grabInteractable.trackRotation = false;
            }
        }

        public virtual void OnGrab()
        {
            RequestAuthority();
        }

        public virtual void OnRelease()
        {
            RemoveAuthority();
        }

        #endregion

       #region FishNet Authority Hooks

        // This fires automatically on clients when ownership changes.
        public override void OnOwnershipClient(NetworkConnection prevOwner)
        {
            base.OnOwnershipClient(prevOwner);

            if (base.IsOwner)
            {
                Debug.Log($"[NetXrInteractable] Authority secured for ID: {base.ObjectId}. Activating XRI tracking.");

                if (grabInteractable != null)
                {
                    grabInteractable.trackPosition = true;
                    grabInteractable.trackRotation = true;
                }
            }
            else
            {
                Debug.Log($"[NetXrInteractable] Authority lost for ID: {base.ObjectId}. Deactivating XRI tracking.");

                if (grabInteractable != null)
                {
                    grabInteractable.trackPosition = false;
                    grabInteractable.trackRotation = false;
                }
            }
        }

        #endregion

        #region Authority Routing

        protected virtual void RequestAuthority()
        {
            if (base.IsServerOnly) return; 

            if (base.IsServer)
            {
                base.NetworkObject.GiveOwnership(base.LocalConnection);
                Debug.Log($"[NetXrInteractable] Host grabbed object. Assigned local authority.");
            }
            else if (base.IsClient && !base.IsOwner)
            {
                CmdRequestAuthority();
            }
        }

        protected virtual void RemoveAuthority()
        {
            if (base.IsServerOnly) return;

            if (base.IsServer)
            {
                base.NetworkObject.RemoveOwnership();
                Debug.Log($"[NetXrInteractable] Host released object. Removed authority.");
                return;
            }

            if (base.IsClient && base.IsOwner)
            {
                CmdRemoveAuthority();
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        protected virtual void CmdRequestAuthority(NetworkConnection sender = null)
        {
            NetworkConnection currentOwner = base.Owner;
            NetworkConnection requester = sender;

            if (currentOwner == null)
            {
                base.NetworkObject.GiveOwnership(requester);
                Debug.Log($"[NetXrInteractable] Assigned unowned object {gameObject.name} to connection: {requester}");
            }
            else if (currentOwner != requester)
            {
                base.NetworkObject.GiveOwnership(requester);
                Debug.Log($"[NetXrInteractable] Stole authority of {gameObject.name} from connection {currentOwner} and gave to: {requester}");
            }
        }

        [ServerRpc]
        protected virtual void CmdRemoveAuthority(NetworkConnection sender = null)
        {
            NetworkConnection currentOwner = base.Owner;
            NetworkConnection requester = sender;

            if (currentOwner == null) return;

            if (currentOwner != requester)
            {
                Debug.LogWarning($"[NetXrInteractable] Rejected delayed drop command from client {requester}.");
                return;
            }

            base.NetworkObject.RemoveOwnership();
            Debug.Log($"[NetXrInteractable] Reclaimed authority over {gameObject.name} from client {requester}.");
        }

        #endregion Authority 
    }
}