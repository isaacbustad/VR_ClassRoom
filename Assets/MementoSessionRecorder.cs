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
        // singleton instance
        private static MementoSessionRecorder instance = null;

        // hold the recording of all mementos
        protected List<ItemMemento> mementos = new List<ItemMemento>();

        // track unique memento IDs
        protected int nextMementoID = 0;

        // keep dictionary of factory items by a memento ID for quick access
        protected Dictionary<int, FactoryItem> factoryItemsByMementoID = new Dictionary<int, FactoryItem>();

        // recording flag
        protected bool isRecording = false;

        // time since last record
        protected float timeSinceLastRecord = 0;
        
        #endregion // Vars

        #region Methods

        
        #region  Mement Recording Methods
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
        
        #endregion // Recording Methods

        #region Session Recording Methods
        // record mementos durring a session over time
        // this will be called by ItemMementoManager in update
        public virtual void RecordSession(float aDeltaTime)
        {
            

             // if recording and time to record
            if (isRecording == true)
            {
                timeSinceLastRecord += aDeltaTime;
                if (timeSinceLastRecord > ItemMementoManager.Instance.TimeBetweenMementoRecords)
                {
                    // time Memento Records = TimeBetweenMementoRecords divided by current pooled  item count
                }
            }
        }

        #endregion // Session Recording Methods

        #endregion // Methods

        #region Constructors
        // make Singelten
        private MementoSessionRecorder()
        {
            
        }
        #endregion // Constructors

        #region Accessors
        // access to mementos recorded
        public List<ItemMemento> Mementos
        {
            get { return mementos; }
        }

        // access to singleton instance
        public static MementoSessionRecorder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MementoSessionRecorder();
                }
                return instance;
            }
        }

        #endregion // Accessors
    }
}