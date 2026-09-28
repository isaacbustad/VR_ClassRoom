// Created By   :   Isaac Bustad
// Created      :   6/3/2023


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.Input; // Meta Interaction SDK namespace


namespace BugFreeProductions.VRClassroom
{
    public class VR_RigPosSnap : MonoBehaviour
    {
        [SerializeField] protected OVRCameraRig cameraRig = null;
        private void OnEnable()
        {
            // // Explicit type is OVRCameraRig
            // if(cameraRig = null)
            // {
            //     cameraRig = Object.FindAnyObjectByType<OVRCameraRig>();
            // }


            // if (cameraRig != null)
            // {
            //     // cameraRig.rightHandAnchor is explicitly a Transform
            //     transform.SetPositionAndRotation(cameraRig.rightHandAnchor.position, cameraRig.rightHandAnchor.rotation);
            //     transform.SetParent(cameraRig.rightHandAnchor, true);
            // }


            // Start a coroutine so we can wait safely for the VR tracking to warm up
            StartCoroutine(WaitForRigAndSnap());
        }

        private IEnumerator WaitForRigAndSnap()
        {
            OVRCameraRig cameraRig = null;

            // 1. Loop until we successfully find the rig in the scene
            while (cameraRig == null)
            {
                cameraRig = Object.FindAnyObjectByType<OVRCameraRig>();
                if (cameraRig == null)
                {
                    yield return null; // Wait for the next frame and try again
                }
            }

            // 2. Extra safety buffer: Wait until Meta's rig confirms it has received tracking data.
            // This directly bypasses those 'TrackingOrigin' console warnings.
            while (cameraRig.rightHandAnchor == null)
            {
                yield return null;
            }

            // 3. Optional: Give the OpenXR compositor one single frame to settle its tracking origin
            yield return new WaitForEndOfFrame();

            // 4. Now that tracking is 100% active, snap and parent safely
            transform.SetPositionAndRotation(cameraRig.rightHandAnchor.position, cameraRig.rightHandAnchor.rotation);
            transform.SetParent(cameraRig.rightHandAnchor, true);
        }
    }
}