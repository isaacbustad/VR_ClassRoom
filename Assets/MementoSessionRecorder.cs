// Created by   : Isaac Bustad
// Created      : 9/17/2025


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.Tools
{
    // manages recording sessions
    public class MementoSessionRecorder : Subscription
    {
        #region Vars
        // singleton instance
        protected static MementoSessionRecorder instance = null;

        // hold a list of all Memento Recorder Objects
        protected List<ItemMementoRecorder> itemMementoRecorders = new List<ItemMementoRecorder>();

        // hold the recording of all mementos
        protected List<ItemMemento> mementos = new List<ItemMemento>();

        // track unique memento IDs
        protected int nextMementoID = 0;

        // keep dictionary of factory items by a memento ID for quick access
        protected Dictionary<int, ItemMementoRecorder> mementoItemsByMementoID = new Dictionary<int, ItemMementoRecorder>();

        // recording flag
        protected bool isRecording = false;

        // recording test path
        protected string recordingTestPath = "RecordTest";

        #region Record Frame Distribution Vars

        // time since last record
        protected float timeSinceLastRecord = 0;

        // time between memento records
        protected float timeBetweenMementoRecords = 0.1f;

        // time between memento recordings
        protected float timeBetweenMementoRecordings = 0.5f;
        
        // current recorder index
        protected int currentRecorderIdx = 0;

        #endregion // Record Frame Distribution Vars

        #endregion // Vars

        #region Methods

        
        #region  Memento Recording Methods
        // start a recording session
        public virtual void StartRecordingSession()
        {
            mementos.Clear();

            // set recording flag
            isRecording = true;
            Debug.Log("Memento Session Recorder: Started Recording Session with " + mementos.Count + " initial Mementos.");
        }

        // stop a recording session
        public virtual void StopRecordingSession()
        {
            // write mementos to file

            // convert mementos to a serializable memento list
            // create the serializable object
            ItemMementoList itemMementoList = new ItemMementoList();

            // assign the created mementos value to the array in the new object
            itemMementoList.itemMementos = mementos.ToArray();

            // call the memento session writer
            MementoReadWrite.Instance.WriteItemMementos(itemMementoList, "/" + recordingTestPath + ".json");

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

        // ToDo: Use Linq reuse existing memento players
        // ToDo: Remove Dictionary usage
        public virtual int GetMementoID(ItemMementoRecorder aIMR)
        {
            int retID = nextMementoID;

            if(mementoItemsByMementoID.ContainsKey(aIMR.InstanceID) == true)
            {
                // setting the returned id to Instance ID if ID exist in Dictionary
                retID = aIMR.InstanceID;
            }

            else
            {
                
                nextMementoID++;
                retID = nextMementoID;

                // add if not found in dictionary
                mementoItemsByMementoID.Add(retID, aIMR);
            }
            
            return retID;
        }
        
        #endregion // Recording Methods

        #region Subscription Methods
        // add a subscriber to the Subsctition
        public void AddSubscriber(Subscriber aSub)
        {
            if (aSub is ItemMementoRecorder aIMR)
            {
                itemMementoRecorders.Add(aIMR);
            }
        }

        // remove a subscriber from the Subscription
        public void RemoveSubscriber(Subscriber aSub)
        {
            if (aSub is ItemMementoRecorder aIMR)
            {
                itemMementoRecorders.Remove(aIMR);
            }
                
        }

        

        // notify
        public void NotifySubscribers()
        {
            
        }

        #endregion // Subscription Methods

        #region Session Recording Methods
        // record mementos during a session over time
        // this will be called by ItemMementoManager in update
        public virtual void RecordSession(float aDeltaTime)
        {
            // if recording and time to record
            if (isRecording == true && itemMementoRecorders.Count > 0)
            {
                timeSinceLastRecord += aDeltaTime;
                while (timeSinceLastRecord > timeBetweenMementoRecords)
                {
                    // increment the index
                    currentRecorderIdx ++;

                    // create memento
                    if (currentRecorderIdx >= itemMementoRecorders.Count)
                    {
                        currentRecorderIdx = 0;
                    }

                    // add the memento at the current Index
                    itemMementoRecorders[currentRecorderIdx].RecordMemento(false);

                    timeSinceLastRecord -= timeBetweenMementoRecords;

                }
            }
        }

        #endregion // Session Recording Methods

        #endregion // Methods

        #region Constructors
        // make Singelten
        protected MementoSessionRecorder()
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
