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

        // track unique memento IDs
        protected int nextMementoID = 0;

        // keep dictionary of factory items by a memento ID for quick access
        protected Dictionary<int, FactoryItem> factoryItemsByMementoID = new Dictionary<int, FactoryItem>();

        // recording flag
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
            // write mementos to file 
            

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

        public virtual int GetMementoID(int aID, FactoryItem aFI)
        {
            int retID = nextMementoID;

            if(factoryItemsByMementoID.ContainsKey(aID) == false)
            {
                factoryItemsByMementoID.Add(retID, aFI);
                retID = aID;
            }
            else
            {
                nextMementoID++;
            }
            
            return retID;
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