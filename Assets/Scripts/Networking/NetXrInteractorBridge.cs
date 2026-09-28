// Created By   :   Isaac Bustad
// Created      :   6/22/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace BugFreeProductions.VRClassroom
{    
    public class NetXrInteractorBridge : MonoBehaviour
    {
        #region Vars
        [SerializeField] protected UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor leftXrInteractor;
        [SerializeField] protected UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor rightXrInteractor;

        [SerializeField] protected XRInteractionManager xRInteractionManager;
        #endregion Vars



        #region Methods
        protected virtual void OnEnable()
        {
            //xRInteractionManager.interactor += OnSelect;
            //leftXrInteractor.

            if (leftXrInteractor == null) return;
            leftXrInteractor.selectEntered.AddListener(OnGrab);
            leftXrInteractor.selectExited.AddListener(OnRelease);

            if (rightXrInteractor == null) return;
            rightXrInteractor.selectEntered.AddListener(OnGrab);
            rightXrInteractor.selectExited.AddListener(OnRelease);
        }

        

        protected  void OnGrab(SelectEnterEventArgs args)
        {
            GameObject grabbed = args.interactableObject.transform.gameObject;
            Debug.Log($"Grabbed: {grabbed.name}");
            grabbed.GetComponent<NetXrInteractable>().OnGrab();
        }

        private void OnRelease(SelectExitEventArgs args)
        {
            GameObject dropped = args.interactableObject.transform.gameObject;
            Debug.Log($"Dropped: {dropped.name}");
            // Handle network release logic here
            dropped.GetComponent<NetXrInteractable>().OnRelease();
        }
        #endregion Methods
    }
}