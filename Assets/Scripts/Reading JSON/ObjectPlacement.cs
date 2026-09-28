// Isaac Bustad
//11/6/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.VRClassroom
{
    [System.Serializable]
    public class ObjectPlacement
    {
        #region Vars
        // Vars
        // has vars that match JSON object fields
        #region transform info in world space
        // position
        public float tpX = 0;
        public float tpY = 0;
        public float tpZ = 0;
        
        // rotation
        public float trX = 0;
        public float trY = 0;
        public float trZ = 0;

        #endregion
        public string id = "NA";
        public int instanceID = 0;
        #endregion

        #region Constructors
        public ObjectPlacement() { }
        
        // full constructor for FactoryItem
        // yet to impliment
        // replace existing code to simplify
        public ObjectPlacement(float aTpX, float aTpY, float aTpZ, 
                               float aTrX, float aTrY, float aTrZ, 
                               string aID)
        {
            // set position
            this.tpX = aTpX;
            this.tpY = aTpY;
            this.tpZ = aTpZ;

            // set rotation
            this.trX = aTrX;
            this.trY = aTrY;
            this.trZ = aTrZ;

            this.id = aID;

        }

        // clone an existing 
        // chained to full constructor for stability
        public ObjectPlacement(ObjectPlacement aOP):this(aOP.tpX, aOP.tpY, aOP.tpZ, 
                                                         aOP.trX, aOP.trY, aOP.trZ,
                                                         aOP.id) 
        {
            this.instanceID = aOP.instanceID;
        }

        // convert NetPlacementData to ObjectPlacement
        // chain to full constructor
        public ObjectPlacement(NetPlacementData aNPD) : this(aNPD.position.x, aNPD.position.y, aNPD.position.z,
                                                            aNPD.rotation.x, aNPD.rotation.y, aNPD.rotation.z,
                                                            aNPD.itemID)
        {
            // addition future actions
        }

        #endregion Constructors

        #region Methods
        public virtual NetPlacementData ToNetPlacementData()
        {
            NetPlacementData npd = new NetPlacementData(this);
            return npd;
        }
        #endregion Methods


    }
}