// Isaac Bustad
// 1/17/2025


using BugFreeProductions.VRClassroom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMS_LockToPoint : PlayerMoveState
{
    // Vars




    // Methods

    // do not move when locked to point
    public override void FUActions(PlayerMoveContext aPMC)
    {
        base.FUActions(aPMC);
    }
    protected override void Move(Rigidbody aRB, Vector3 aMovDir, MoveStateParam_SCO aMSP_SCO)
    {

    }




    // Accessors
    public override bool IsLockToPoint { get { return true; } }



}
