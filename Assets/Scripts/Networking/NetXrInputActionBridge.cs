// Created By   :   Isaac Bustad
// Created      :   6/10/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BugFreeProductions.VRClassroom
{
    public class NetXrInputActionBridge : XrInputActionBridge
    {
        #region Vars
        //protected NetPlacableObjectPlacer npop = null;
        #endregion

        #region Methods

        #region Unity Methods
        protected override void Setup()
        {
            // base.Setup();
            // npop = GetComponentInChildren<NetPlacableObjectPlacer>();
        }

        #endregion Unity Methods

        #region Action Methods
        // --- Simple Methods to Handle the Input ---

        // right index trigger actions
        // protected override void OnRightIndexPress(InputAction.CallbackContext ctx)
        // {
        //     // if (ctx.started)
        //     // {
        //     //     npop.UsePlacer(true);
        //     // }

        //     // else if (ctx.canceled)
        //     // {
        //     //     npop.UsePlacer(false);
        //     // }
        // }


        // protected override void OnRightIndexRelease(InputAction.CallbackContext ctx)  {Debug.Log("Right Trigger Released");}

        // // right middle trigger actions
        // protected override void OnRightMiddlePress(InputAction.CallbackContext ctx) {Debug.Log("Right Grip Squeezed");}
        // protected override void OnRightMiddleRelease(InputAction.CallbackContext ctx) {Debug.Log("Right Grip Released");}

        // // left index trigger actions
        // protected override void OnLeftIndexPress(InputAction.CallbackContext ctx) {Debug.Log("Left Trigger Squeezed");}
        // protected override void OnLeftIndexRelease(InputAction.CallbackContext ctx) {Debug.Log("Left Trigger Released");}

        // // left index middle actions
        // protected override void OnLeftMiddlePress(InputAction.CallbackContext ctx) {Debug.Log("Left Grip Squeezed");}
        // protected override void OnLeftMiddleRelease(InputAction.CallbackContext ctx) {Debug.Log("Left Grip Released");}

        #endregion Action Methods

        #endregion Methods
    }
}