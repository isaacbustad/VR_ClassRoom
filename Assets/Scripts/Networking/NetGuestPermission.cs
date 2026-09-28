// Created By   :   Isaac Busatd
// Created      :   6/16/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BugFreeProductions.VRClassroom
{
    [System.Serializable]
    public struct NetGuestPermission
    {
        public bool guestCanEdit;
        public bool guestCanPermit;
        public bool guestCanSave;
        public bool guestCanRecord;
        public bool guestCanReplay;



        public NetGuestPermission(bool aGuestCanEdit, bool aGuestCanRecord, bool aGuestCanPermit, bool aGuestCanSave, bool aGuestCanReplay)
        {
            guestCanEdit = aGuestCanEdit;
            guestCanRecord = aGuestCanRecord;
            guestCanPermit = aGuestCanPermit;
            guestCanSave = aGuestCanSave;
            guestCanReplay = aGuestCanReplay;

        }

        
    }
}