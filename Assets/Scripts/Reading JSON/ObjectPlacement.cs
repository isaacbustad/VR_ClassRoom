// Isaac Bustad
//11/6/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.Tools
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
        public string instanceID = "NA";
        #endregion

        #region constructors
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
            
        }


        #endregion


    }
}