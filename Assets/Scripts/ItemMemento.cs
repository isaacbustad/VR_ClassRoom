// Isaac Bustad
// 9/17/2025

using BugFreeProductions.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace BugFreeProductions.Tools
{
    // class that is for the recording of serialized information only
    [Serializable]
    public class ItemMemento : ObjectPlacement
    {
        #region Vars
        
        // list of actions conducted this memento via strings
        protected string[] actions = new string[0];

        // time stamp as a float
        protected double timestamp = 0;

        #endregion

        #region Methods

        #endregion

        #region Constructors
        public ItemMemento()
        {

        }

        // construct based on a Objects current placement
        public ItemMemento(ObjectPlacement aOP) : base(aOP)
        {
            timestamp = Time.realtimeSinceStartupAsDouble;
        }

        #endregion

        #region Accessors

        #endregion
    }
}