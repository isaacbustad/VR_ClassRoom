// Created By   :   Isaac Bustad
// Created      :   6/11/2026
// Assisted By  :   Gemini
// Converted To :   FishNet on 10/3/2026

using System.Collections.Generic;
using FishNet.Object;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace BugFreeProductions.VRClassroom
{
    public class XrNetworkPlayerSetup : NetworkBehaviour
    {
        protected Camera localCamera;
        protected AudioListener localAudioListener;
        protected TrackedPoseDriver headPoseDriver;

        protected ContinuousMoveProviderBase moveProvider;
        protected SnapTurnProviderBase turnProvider;
        protected CharacterController characterController;

        protected TrackedPoseDriver leftHandPose;
        protected TrackedPoseDriver rightHandPose;
        protected XRBaseController leftController;
        protected XRBaseController rightController;

        // input components
        protected XrInputActionBridge inputActionBridge = null;

        [SerializeField] private XROrigin xrOrigin;

        [Header("list of objects that should be enabled if this belongs to the player")]
        [SerializeField] protected List<GameObject> localPlayerOnlyGOs = new List<GameObject>(); // Exposed to Inspector

        /// <summary>
        /// FishNet native callback: Fires on ALL clients when this object is network-initialized.
        /// Replaces Mirror's OnStartClient and OnStartLocalPlayer by evaluating base.IsOwner.
        /// </summary>
        public override void OnStartClient()
        {
            base.OnStartClient();

            // 1. Gather all references right here as the client starts up
            GatherComponentReferences();

            // 2. Configure components based on whether this client owns the player object
            ToggleLocalComponents(base.IsOwner);
        }

        protected void GatherComponentReferences()
        {
            // Find root locomotion and physics components
            characterController = GetComponent<CharacterController>();
            moveProvider = GetComponent<ContinuousMoveProviderBase>();
            turnProvider = GetComponent<SnapTurnProviderBase>();

            // Find Camera-related components by drilling down the standard XR Origin path
            Transform cameraTransform = transform.Find("Camera Offset/Main Camera");
            if (cameraTransform != null)
            {
                localCamera = cameraTransform.GetComponent<Camera>();
                localAudioListener = cameraTransform.GetComponent<AudioListener>();
                headPoseDriver = cameraTransform.GetComponent<TrackedPoseDriver>();
            }

            // Find Left Hand tracking components
            Transform leftHandTransform = transform.Find("Camera Offset/Left Controller");
            if (leftHandTransform != null)
            {
                leftController = leftHandTransform.GetComponent<XRBaseController>();
                leftHandPose = leftHandTransform.GetComponent<TrackedPoseDriver>();
            }

            // Find Right Hand tracking components
            Transform rightHandTransform = transform.Find("Camera Offset/Right Controller");
            if (rightHandTransform != null)
            {
                rightController = rightHandTransform.GetComponent<XRBaseController>();
                rightHandPose = rightHandTransform.GetComponent<TrackedPoseDriver>();
            }

            // Get reference for the input bridge
            inputActionBridge = GetComponentInChildren<XrInputActionBridge>();
        }

        protected void ToggleLocalComponents(bool isLocal)
        {
            if (localCamera != null) localCamera.enabled = isLocal;
            if (localAudioListener != null) localAudioListener.enabled = isLocal;

            // CRITICAL: CharacterController must remain ENABLED on remote proxies to let NetworkTransform sync their positions!
            if (characterController != null) characterController.enabled = isLocal; 
            
            if (moveProvider != null)        moveProvider.enabled = isLocal;
            if (turnProvider != null)        turnProvider.enabled = isLocal;

            if (headPoseDriver != null)   headPoseDriver.enabled = isLocal;
            if (leftHandPose != null)     leftHandPose.enabled = isLocal;
            if (rightHandPose != null)    rightHandPose.enabled = isLocal;
            
            if (leftController != null)   leftController.enabled = isLocal;
            if (rightController != null)  rightController.enabled = isLocal;

            if (inputActionBridge != null) inputActionBridge.enabled = isLocal;

            // loop through added objects
            foreach (GameObject go in localPlayerOnlyGOs)
            {
                if (go != null) go.SetActive(isLocal);
            }
        }
    }
}