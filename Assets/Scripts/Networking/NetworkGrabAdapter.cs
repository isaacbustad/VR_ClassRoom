// Created By   :   Isaac Bustad
// Created      :   6/15/2026
// Assisted By  :   Gemini
// Converted To :   FishNet on 10/3/2026

using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using UnityEngine.XR.Interaction.Toolkit;

namespace BugFreeProductions.VRClassroom
{
    [RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkGrabAdapter : NetworkBehaviour
    {
        #region Vars
        protected UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
        protected Rigidbody rb;
        #endregion Vars

        #region Methods
        protected virtual void Awake()
        {
            grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
            rb = GetComponent<Rigidbody>();
        }

        protected virtual void OnEnable()
        {
            // Hook into XRI's local interaction events
            grabInteractable.selectEntered.AddListener(OnGrabEntered);
            grabInteractable.selectExited.AddListener(OnGrabExited);
        }

        protected virtual void OnDisable()
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabEntered);
            grabInteractable.selectExited.RemoveListener(OnGrabExited);
        }

        protected virtual void OnGrabEntered(SelectEnterEventArgs args)
        {
            // Only the local player initiating the grab needs to ask for authority
            if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor)
            {
                // Verify if this is the local avatar's hand controller
                // Request ownership from the server so we can manipulate the object locally
                CmdRequestAuthority();
            }
        }

        protected virtual void OnGrabExited(SelectExitEventArgs args)
        {
            // Verify if the local client currently owns this object before sending release physics
            if (base.IsOwner)
            {
                // XRI's default Throw on Detach runs right before this event.
                // We capture that resulting velocity and tell the server to apply it for everyone.
                CmdReleaseObject(rb.linearVelocity, rb.angularVelocity);
            }
        }

        /// <summary>
        /// ServerRpc to request object ownership when a client grabs it.
        /// RequireOwnership = false allows clients who do not yet own the object to request control.
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        protected virtual void CmdRequestAuthority(NetworkConnection conn = null)
        {
            if (base.NetworkObject != null && conn != null)
            {
                // Give ownership to the requesting client connection (automatically replaces any previous owner)
                base.NetworkObject.GiveOwnership(conn);
            }
        }

        /// <summary>
        /// ServerRpc to send the release throw velocities to the server.
        /// </summary>
        [ServerRpc]
        protected virtual void CmdReleaseObject(Vector3 velocity, Vector3 angularVelocity)
        {
            // Server forces synchronization of the final velocity vectors across all observing clients
            RpcApplyThrowPhysics(velocity, angularVelocity);
        }

        /// <summary>
        /// ObserversRpc executed on all observing clients to synchronize throw physics.
        /// </summary>
        [ObserversRpc]
        protected virtual void RpcApplyThrowPhysics(Vector3 velocity, Vector3 angularVelocity)
        {
            // Ensures physics engine resumes seamlessly on all remote instances
            rb.linearVelocity = velocity;
            rb.angularVelocity = angularVelocity;
        }
        #endregion Methods
    }
}