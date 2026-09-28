// Created By   :   Isaac Bustad
// Created      :   6/15/2026
// Assisted By  :   Gemini


using UnityEngine;
using Mirror;
using UnityEngine.XR.Interaction.Toolkit;


namespace BugFreeProductions.VRClassroom
{
    
    [RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetworkGrabAdapter : NetworkBehaviour
    {
        protected UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
        protected Rigidbody rb;

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
                // You want to avoid processing this if a remote ghost avatar somehow triggers it
                CmdRequestAuthority();
            }
        }

        protected virtual void OnGrabExited(SelectExitEventArgs args)
        {
            if (isOwned)
            {
                // XRI's default Throw on Detach runs right before this event.
                // We capture that resulting velocity and tell the server to apply it for everyone.
                CmdReleaseObject(rb.linearVelocity, rb.angularVelocity);
            }
        }

        [Command]
        protected virtual void CmdRequestAuthority()
        {
            // Remove authority from current owner (if any) and give to the sender
            NetworkIdentity identity = GetComponent<NetworkIdentity>();
            identity.RemoveClientAuthority(); 
            identity.AssignClientAuthority(connectionToClient);
        }

        [Command]
        protected virtual void CmdReleaseObject(Vector3 velocity, Vector3 angularVelocity)
        {
            // Server forces synchronization of the final velocity vectors across all clients
            RpcApplyThrowPhysics(velocity, angularVelocity);
        }

        [ClientRpc]
        protected virtual void RpcApplyThrowPhysics(Vector3 velocity, Vector3 angularVelocity)
        {
            // Ensures physics engine resumes seamlessly on all remote instances
            rb.linearVelocity = velocity;
            rb.angularVelocity = angularVelocity;
        }
    }
}