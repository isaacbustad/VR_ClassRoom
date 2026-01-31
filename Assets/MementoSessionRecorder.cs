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
        protected bool isRecording = false;


       

        #endregion

        #region Methods

        

        // start a recording session
        public virtual void StartRecordingSession()
        {
            mementos.Clear();

            // set recording flag
            isRecording = true;

            List<Poolable> pooled = ItemMementoManager.Instance.PooledItems;

            //  loop through pooled items and record their mementos
            foreach (Poolable poolable in pooled)
            {
                ItemMementoRecorder recorder = poolable.GetComponent<ItemMementoRecorder>();
                if (recorder != null)
                {
                    // record memento
                    recorder.RecordMemento();
                    //mementos.Add(memento);
                }
            }
           

            Debug.Log("Memento Session Recorder: Started Recording Session with " + mementos.Count + " initial Mementos.");
        }

        // stop a recording session
        public virtual void StopRecordingSession()
        {
            // set recording flag
            isRecording = false;
        }

        // add a memento to the session
        public virtual void AddMemento(ItemMemento aMemento)
        {
            // only add if recording
            if (isRecording == true)
            {
                // add memento to list
                mementos.Add(aMemento);
            }
            
        }
        #endregion

        #region Accessors
        // access to mementos recorded
        public List<ItemMemento> Mementos
        {
            get { return mementos; }
        }
        #endregion
    }
}