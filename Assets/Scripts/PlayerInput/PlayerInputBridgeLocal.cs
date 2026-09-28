// Isaac Bustad
// 1/17/2025


using BugFreeProductions.VRClassroom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerInputBridgeLocal : PlayerInputBridge
{
    // Vars
    #region Movement Vars

    #endregion

    #region Camera Vars

    #endregion

    #region
    // for additional rotation testing
    [SerializeField] protected PlacableItemPlacer placableItemPlacer = null;
    #endregion

    // Methods
    #region Keyboard Player Controls
    public override void MouseCamControles(InputAction.CallbackContext aCon)
    {
        Vector2 nDir = aCon.ReadValue<Vector2>();
        //Debug.Log("In Direction" + nDir);
        camRotDir += new Vector3(-nDir.y, nDir.x, 0);
        //Debug.Log("Mouse Direction" + camRotDir);
    }

    public override void KeyBoardMove(InputAction.CallbackContext aCon)
    {
        Vector2 nDir = aCon.ReadValue<Vector2>();

        moveDir = new Vector3(nDir.x, 0, nDir.y);
    }

    public override void Primary_Action_Left_Q(InputAction.CallbackContext aCon)
    {

    }

    public virtual void Primary_Action_Left_Click(InputAction.CallbackContext aCon)
    {
        placableItemPlacer.PlayerInputBridge = this;
        placableItemPlacer.UsePlacer(aCon);
    }

    public override void Primary_Action_Right_E(InputAction.CallbackContext aCon)
    {

    }
    public virtual void MouseScroll(InputAction.CallbackContext aCon)
    {
        /*float nVall = ((float)aCon.ReadValue<double>());
        additionalBodyRotation.y += Mathf.Clamp(nVall, -1f, 1f);*/
        //additionalBodyRotation.y += ((float)aCon.ReadValue<Axis>());
        additionalBodyRotation += aCon.ReadValue<Vector3>();
    }

    protected virtual void Update()
    {
        LerpCamToZero();
        LerpAdditionalRotationToZero();
    }

    protected virtual void LerpCamToZero()
    {
        if (camRotDir.magnitude > playerCameraContext.PlayerCameraParam_SCO.ReZeroBuff)
        {
            camRotDir = Vector3.Lerp(camRotDir, Vector3.zero, playerCameraContext.PlayerCameraParam_SCO.TimeToTween);
        }
        else camRotDir = Vector3.zero;
        
    }


    #endregion
    protected virtual void LerpAdditionalRotationToZero()
    {
        if (additionalBodyRotation.magnitude > playerCameraContext.PlayerCameraParam_SCO.ReZeroBuff)
        {
            additionalBodyRotation = Vector3.Lerp(additionalBodyRotation, Vector3.zero, playerCameraContext.PlayerCameraParam_SCO.TimeToTween);
        }
        else additionalBodyRotation = Vector3.zero;

    }

    #region UI
    // Move to local input bridge
    public void HandleToggleRadialMenu(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            UIUtils.HandleToggleRadialMenuKBM();
        }
    }
    #endregion

    // Accessors



}
