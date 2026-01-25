// Created by   : Isaac Bustad
// Created      : 9/17/2025

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BugFreeProductions.Tools
{
    // manages recording sessions
    public class MementoSessionRecorder
    {
        #region Vars
        // hold the recording of all mementos
        protected List<ItemMemento> mementos = new List<ItemMemento>();
        
        #endregion

        #region Methods
        // start a recording session
        public virtual void StartRecordingSession()
        {
            mementos.Clear();

            List<Poolable> pooled = ItemMementoManager.Instance.PooledItems;

            //  loop through pooled items and record their mementos
            foreach (Poolable poolable in pooled)
            {
                ItemMementoRecorder recorder = poolable.GetComponent<ItemMementoRecorder>();
                if (recorder != null)
                {
                    ItemMemento memento = recorder.RecordMemento();
                    mementos.Add(memento);
                }
            }
        }

        #endregion

        #region Accessors

        #endregion
    }
}