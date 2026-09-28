// Isaac Bustad
// 1/17/2025


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BugFreeProductions.VRClassroom;

public class PlayerMoveState 
{
    // Vars


    // Methods
    #region State Change Methods
    public virtual PlayerMoveState LockToPoint(PlayerMoveContext aPMC)
    {
        return aPMC.LockToPointPMS;
    }

    public virtual PlayerMoveState FreeWalk(PlayerMoveContext aPMC)
    {
        return aPMC.FreeWalkPMS;
    }
    #endregion

    #region Fixed Update Actions
    // Fixed Update Actions
    public virtual void FUActions(PlayerMoveContext aPMC)
    {
        Move(aPMC.RB,aPMC.IB.MoveDir,aPMC.FreeWalkMSP, aPMC);
        AlignBod(aPMC.PlayerCameraContext, aPMC);
        BugFreeTool.LimitToWorldVelocity(aPMC.RB.linearVelocity);
    }

    // Update Actions
    public virtual void UActions(PlayerMoveContext aPMC)
    {
        //Move(aPMC.RB,aPMC.IB,aPMC.);
    }

    protected virtual void Move(Rigidbody aRB, Vector3 aMovDir, MoveStateParam_SCO aMSP_SCO)
    {
        if (aRB != null)
        {
            if (aRB.linearVelocity.magnitude < aMSP_SCO.MaxSpeed)
            {
                aRB.AddForce(aMovDir.normalized * aMSP_SCO.Accelleration, ForceMode.Force);
            }

        }
        //aRB.AddForce(aMovDir.normalized *);
    }

    protected virtual void Move(Rigidbody aRB, Vector3 aMovDir, MoveStateParam_SCO aMSP_SCO, PlayerMoveContext aPCM)
    {
        if (aRB != null)
        {
            if (aRB.linearVelocity.magnitude < aMSP_SCO.MaxSpeed)
            {
                // set forward force
                Vector3 nF = aPCM.RotBodTF.forward * aMovDir.z;


                // set side force
                Vector3 nS = aPCM.RotBodTF.right * aMovDir.x;


                // aRB.AddForce(aMovDir.normalized * aMSP_SCO.Accelleration, ForceMode.Force);

                aRB.AddForce((nF + nS).normalized * aMSP_SCO.Accelleration, ForceMode.Force);
            }

        }
        //aRB.AddForce(aMovDir.normalized *);
    }

    protected virtual void AlignBod(PlayerCameraContext aPCC, PlayerMoveContext aPMC)
    {
        // hold initial target rot
        Vector3 nRot = aPCC.CamRot;


        // calc correct bod alignment
        nRot.x = 0;
        nRot.z =0;


        // set new rotation
        aPMC.RotBodTF.rotation = Quaternion.Lerp(aPMC.RotBodTF.rotation, Quaternion.Euler(nRot), aPMC.FreeWalkMSP.TimeToTween) ;
    }
    #endregion

    // Accessors
    #region Bool Check for States
    public virtual bool IsLockToPoint { get { return false; } }
    public virtual bool IsFreeWalk { get { return false; } }
    #endregion


}
