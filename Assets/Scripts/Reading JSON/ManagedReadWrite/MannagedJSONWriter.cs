// Isaac Busatd
// 4/3/2025


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.VRClassroom
{
    public class MannagedJSONWriter
    {
        // Vars


        // Methods
        // Write to specified location
        public virtual void WriteObjPlacementData(string aRoomFilePath, string aObjectFilePath)
        {
            if (JSONPlacementMannager.Instance.ABF_SCO != null)
            {
                // ObjectPlacementList roomPointLST = new ObjectPlacementList();
                // get the placable object info
                ObjectPlacementList opl = JSONPlacementMannager.Instance.ABF_SCO.GatherFactItemPosInfo(/*ref roomPointLST*/);

                // write object info
                ObjectPlacementReadWrite.Instance.WriteObjectPlacements(opl, aObjectFilePath);

                // Write room points
                // ObjectPlacementReadWrite.Instance.WriteObjectPlacements(opl, aRoomFilePath);
            }
        }

        //Accessors


    }
}