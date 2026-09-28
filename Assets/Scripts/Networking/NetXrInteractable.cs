// Created By   :   Isaac Bustad
// Created      :   6/22/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


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
            Debug.Log($"[Mirror] Client started for object: {netId}");
        }

        protected virtual void OnEnable()
        {
            Setup();
        }

        protected virtual void Setup()
        {
            grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            
            // CRITICAL STEP: Prevent XRI from trying to track the object across the network
            // until Mirror says we officially have the authority to move it.
            if (grabInteractable != null && !isOwned)
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

        #region Mirror Authority Hooks

        // This fires automatically the exact frame Mirror registers that 
        // this client successfully obtained network ownership.
        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            Debug.Log($"[NetXrInteractable] Authority secured for NetID: {netId}. Activating XRI tracking.");

            // Turn XRI tracking back on so the object smoothly follows your hand
            if (grabInteractable != null)
            {
                grabInteractable.trackPosition = true;
                grabInteractable.trackRotation = true;
            }
        }

        // This fires automatically when ownership is stripped or dropped.
        public override void OnStopAuthority()
        {
            base.OnStopAuthority();
            Debug.Log($"[NetXrInteractable] Authority lost for NetID: {netId}. Deactivating XRI tracking.");

            // Turn off tracking so remote physics/transforms don't fight local interactions
            if (grabInteractable != null)
            {
                grabInteractable.trackPosition = false;
                grabInteractable.trackRotation = false;
            }
        }

        #endregion

        #region Authority Routing

        protected virtual void RequestAuthority()
        {
            if (isServerOnly) return; 

            if (isServer)
            {
                if (netIdentity.connectionToClient != null)
                {
                    netIdentity.RemoveClientAuthority();
                }
                netIdentity.AssignClientAuthority(NetworkServer.localConnection);
                Debug.Log($"[NetXrInteractable] Host grabbed object. Assigned local authority.");
            }
            else if (isClient && !isOwned)
            {
                CmdRequestAuthority();
            }
        }

        protected virtual void RemoveAuthority()
        {
            if (isServer) return;

            if (isClient && isOwned)
            {
                CmdRemoveAuthority();
            }
        }
        
        [Command(requiresAuthority = false)]        
        protected virtual void CmdRequestAuthority(NetworkConnectionToClient sender = null)
        {
            NetworkConnectionToClient currentOwner = netIdentity.connectionToClient;
            NetworkConnectionToClient requester = sender; // Use the auto-injected sender parameter

            if (currentOwner == null)
            {
                netIdentity.AssignClientAuthority(requester);
                Debug.Log($"[NetXrInteractable] Assigned unowned object {gameObject.name} to connection: {requester}");
            }
            else if (currentOwner != requester)
            {
                netIdentity.RemoveClientAuthority();
                netIdentity.AssignClientAuthority(requester);
                Debug.Log($"[NetXrInteractable] Stole authority of {gameObject.name} from connection {currentOwner} and gave to: {requester}");
            }
        }

        [Command]
        protected virtual void CmdRemoveAuthority(NetworkConnectionToClient sender = null)
        {
            NetworkConnectionToClient currentOwner = netIdentity.connectionToClient;
            NetworkConnectionToClient requester = sender;

            if (currentOwner == null) return;

            if (currentOwner != requester)
            {
                Debug.LogWarning($"[NetXrInteractable] Rejected delayed drop command from client {requester}.");
                return;
            }

            netIdentity.RemoveClientAuthority();
            Debug.Log($"[NetXrInteractable] Reclaimed authority over {gameObject.name} from client {requester}.");
        }

        #endregion Authority 
    }
}