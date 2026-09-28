// Created By   :   Isaac Bustad
// Created      :   6/5/2026

using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;


namespace BugFreeProductions.VRClassroom
{
    public struct NetPlacementData
    {
        #region Vars
        // denotes what object should be spawned
        public string itemID;

        // denotes the position it should be spawned
        public Vector3 position;

        // denotes the rotation of the object to be spawned
        public Vector3 rotation;
        #endregion Vars


        #region Methods
        public ObjectPlacement ToObjectPlacement()
        {
            return new ObjectPlacement(this);
        }
        #endregion Methods

        #region Constuctors
        public NetPlacementData(ObjectPlacement aOP)
        {
            itemID = aOP.id;
            position = new Vector3(aOP.tpX,aOP.tpY,aOP.tpZ);
            rotation = new Vector3(aOP.trX,aOP.trY,aOP.trZ);
        }
        #endregion Constructors
    }
}