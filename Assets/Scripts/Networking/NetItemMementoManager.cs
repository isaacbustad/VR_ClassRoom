// Created By   :   Isaac Bustad
// Created      :   6/18/2026


using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;


namespace BugFreeProductions.VRClassroom
{

    public class NetItemMementoManager : ItemMementoManager
    {
        #region Vars
        NetworkIdentity ni = null;

        #endregion Vars

        #region Methods
        // protected override void OnEnable()
        // {
        //     base.OnEnable();
        // }


        // summary: test able code that prevents users from usig the memento system
        // and recording
        protected override void TestByKey()
        {
            if (NetGuestPermissionManager.Instance.isServer)
            {
                base.TestByKey();
                return;
            }

            if (NetGuestPermissionManager.GuestCanRecord)
            {
                base.TestByKey();
                return;
            }
        }

        
        #endregion Methods
    }
}