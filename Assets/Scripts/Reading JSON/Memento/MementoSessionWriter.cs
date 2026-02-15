// Created By: Isaac Bustad
// Created On: 2/5/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.Tools
{
    // writes memento sessions to disk
    public class MementoSessionWriter
    {
        #region Vars
        // singleton instance
        protected static MementoSessionWriter instance = null;

        #endregion

        #region Methods
        // write memento session to disk
        public virtual void WriteMementoSession(ItemMementoList mementoList, string path)
        {
            CustomGatewayJSON.Instance.WriteJsonFile(path, JsonUtility.ToJson(mementoList));
        }
        #endregion

        #region Constructors
        // make Singelten
        protected MementoSessionWriter()
        {

        }
        #endregion

        #region Accessors
        // singleton instance accessor
        public static MementoSessionWriter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MementoSessionWriter();
                }
                return instance;
            }
        }

        #endregion
        
    }
}