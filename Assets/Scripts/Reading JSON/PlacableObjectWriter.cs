// Isaac Busatd
// 12/11/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace BugFreeProductions.VRClassroom
{
    public class PlacableObjectWriter : MonoBehaviour
    {
        // Vars
        [SerializeField] protected AbstractFactory_SCO abf_SCO = null;

        

        // Methods
        // test Purpose only
        public virtual void WriteTest(InputAction.CallbackContext aCall)
        {
            WriteObjPlacementData();
        }

        public virtual void WriteObjPlacementData()
        {
            if (abf_SCO != null)
            {
                ObjectPlacementList opl = abf_SCO.GatherFactItemPosInfo();

                ObjectPlacementReadWrite.Instance.WriteObjectPlacements(opl);
            }
        }

        // Write to specified location
        public virtual void WriteTest(InputAction.CallbackContext aCall, string aFilePath)
        {
            WriteObjPlacementData(aFilePath);
        }

        public virtual void WriteObjPlacementData(string aFilePath)
        {
            if (abf_SCO != null)
            {
                ObjectPlacementList opl = abf_SCO.GatherFactItemPosInfo();

                ObjectPlacementReadWrite.Instance.WriteObjectPlacements(opl, aFilePath);
            }
        }

        //Accessors



    }
}