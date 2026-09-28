// Created by   :   Isaac Bustad
// Created      :   6/10/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// this neeeds to be de coupled but there is no time in the forseeable future to accomidate
namespace BugFreeProductions.VRClassroom
{

    public class XrInputActionBridge : MonoBehaviour
    {
        #region Vars
        [Header("Input Action References")]
        [SerializeField] protected InputActionReference rightIndexTriggerAction = null;
        [SerializeField] protected InputActionReference rightMiddleTriggerAction = null;
        [SerializeField] protected InputActionReference leftIndexTriggerAction = null;
        [SerializeField] protected InputActionReference leftMiddleTriggerAction = null;

        [Header("Player Action References")]
        [SerializeField] protected VR_PlacableItemPlacerGun pipg = null;
        [SerializeField] protected VR_PlacableItemRemoverGun pirg = null;

        

        #endregion Vars

        #region Methods
        protected virtual void OnEnable()
        {
            Setup();
        }

        // setup the hand input references
        protected virtual void Setup()
        {
            // Right Hand Direct Setup
            // right index
            if (rightIndexTriggerAction != null)
            {
                rightIndexTriggerAction.action.Enable();
                rightIndexTriggerAction.action.started += OnRightIndexPress;
                rightIndexTriggerAction.action.canceled += OnRightIndexRelease;
            }

            // right middle
            if (rightMiddleTriggerAction != null)
            {
                rightMiddleTriggerAction.action.Enable();
                rightMiddleTriggerAction.action.started += OnRightMiddlePress;
                rightMiddleTriggerAction.action.canceled += OnRightMiddleRelease;
            }

            // Left Hand Direct Setup
            // left index
            if (leftIndexTriggerAction != null)
            {
                leftIndexTriggerAction.action.Enable();
                leftIndexTriggerAction.action.started += OnLeftIndexPress;
                leftIndexTriggerAction.action.canceled += OnLeftIndexRelease;
            }

            // left middle
            if (leftMiddleTriggerAction != null)
            {
                leftMiddleTriggerAction.action.Enable();
                leftMiddleTriggerAction.action.started += OnLeftMiddlePress;
                leftMiddleTriggerAction.action.canceled += OnLeftMiddleRelease;
            }

            // // additional temporary references
            // if(pipg != null)
            // {
            //     pipg = GetComponentInChildren<VR_PlacableItemPlacerGun>();
            // }
            
            // if(pirg != null)
            // {
            //     pirg = GetComponentInChildren<VR_PlacableItemRemoverGun>();
            // }
            
        }

        protected virtual void OnDisable()
        {
            // Right Hand Clean Up
            if (rightIndexTriggerAction != null)
            {
                rightIndexTriggerAction.action.started -= OnRightIndexPress;
                rightIndexTriggerAction.action.canceled -= OnRightIndexRelease;
                rightIndexTriggerAction.action.Disable();
            }

            if (rightMiddleTriggerAction != null)
            {
                rightMiddleTriggerAction.action.started -= OnRightMiddlePress;
                rightMiddleTriggerAction.action.canceled -= OnRightMiddleRelease;
                rightMiddleTriggerAction.action.Disable();
            }

            // Left Hand Clean Up
            if (leftIndexTriggerAction != null)
            {
                leftIndexTriggerAction.action.started -= OnLeftIndexPress;
                leftIndexTriggerAction.action.canceled -= OnLeftIndexRelease;
                leftIndexTriggerAction.action.Disable();
            }

            if (leftMiddleTriggerAction != null)
            {
                leftMiddleTriggerAction.action.started -= OnLeftMiddlePress;
                leftMiddleTriggerAction.action.canceled -= OnLeftMiddleRelease;
                leftMiddleTriggerAction.action.Disable();
            }
        }

        #region Action Methods
        // --- Simple Methods to Handle the Input ---

        // right index trigger actions
        protected virtual void OnRightIndexPress(InputAction.CallbackContext ctx)
        {
            if (pirg != null)
            {
                pirg.UseRemover(true);
            }


        }


        protected virtual void OnRightIndexRelease(InputAction.CallbackContext ctx)
        {
            if (pirg != null)
            {
                pirg.UseRemover(false);
            }
        }

        // right middle trigger actions
        protected virtual void OnRightMiddlePress(InputAction.CallbackContext ctx)
        {
            Debug.Log(">>> BRIDGE CAUGHT GRIP PRESS <<<"); // Put this BEFORE the if statement
            if (pipg != null)
            {
                
                pipg.UsePlacer(true);
            }
        }
        protected virtual void OnRightMiddleRelease(InputAction.CallbackContext ctx)
        {
            Debug.Log(">>> BRIDGE CAUGHT GRIP Release <<<"); // Put this BEFORE the if statement
            if (pipg != null)
            {
                
                pipg.UsePlacer(false);
            }
        }

        // left index trigger actions
        protected virtual void OnLeftIndexPress(InputAction.CallbackContext ctx) {Debug.Log("Left Trigger Squeezed");}
        protected virtual void OnLeftIndexRelease(InputAction.CallbackContext ctx) {Debug.Log("Left Trigger Released");}

        // left index middle actions
        protected virtual void OnLeftMiddlePress(InputAction.CallbackContext ctx) {Debug.Log("Left Grip Squeezed");}
        protected virtual void OnLeftMiddleRelease(InputAction.CallbackContext ctx) {Debug.Log("Left Grip Released");}

        #endregion Action Methods

        #endregion Methods
    }
}