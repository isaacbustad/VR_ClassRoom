// Isaac Bustad
// 9/17/2025


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ToDo: create list of all pooled items stored here or in ObjectPlacementManager
// ToDo: calculate time between memento records based on current pooled item count and a desired number of rounds per second

namespace BugFreeProductions.Tools
{
    // manages playback of item mementos
    public class ItemMementoManager : MonoBehaviour
    {
        #region Vars
        #region Test Keys
        // for testing start/stop recording
        protected KeyCode startRecordingKey = KeyCode.J;
        protected KeyCode stopRecordingKey = KeyCode.K;
        protected KeyCode printMementosKey = KeyCode.L;
        #endregion

        #region Test Path
        // for testing path
        protected string testRecordPath = "/record.json";
        #endregion

        // Singelten instance
        private static ItemMementoManager instance = null;

        // hold the recording of all mementos
        protected List<ItemMemento> mementos = new List<ItemMemento>();
        protected MementoSessionRecorder sessionRecorder;
        protected MementoSessionWriter sessionWriter;

        // reference to Abstract Factory
        [SerializeField] private AbstractFactory_SCO abf_SCO = null;

        // time between memento records
        protected float timeBetweenMementoRecords = 0.1f;

        
        
        
        // hold path
        protected string recordPath = "/record/test";


        #endregion

        #region Methods
        // OnEnable collect variables
        protected virtual void SetDefaults()
        {
            // get session recorder
            sessionRecorder = MementoSessionRecorder.Instance;
            // get session writer
            sessionWriter = MementoSessionWriter.Instance;
        }

        #region Test Methods
        // for testing start/stop recording
        protected virtual void Update()
        {
            if (Input.GetKeyDown(startRecordingKey))
            {
                sessionRecorder.StartRecordingSession();
                Debug.Log("Started Recording Session.");
            }

            if (Input.GetKeyDown(stopRecordingKey))
            {
                // stop recording
                sessionRecorder.StopRecordingSession();
                Debug.Log("Stopped Recording Session with " + sessionRecorder.Mementos.Count + " Mementos recorded.");
            }

            if (Input.GetKeyDown(printMementosKey))
            {
                int itemCount = 0;
                // print mementos
                foreach (ItemMemento memento in sessionRecorder.Mementos)
                {
                    itemCount++;
                    Debug.Log(itemCount + ": Memento ID: " + memento.id + " at position: " + memento.tpX + " , " + memento.tpY + " , " + memento.tpZ);
                }
            }
        }
        #endregion

        // make Singelten on enable
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            if (instance != null)
            {
                if (instance != this)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
                ReadRoomsInPath.FindRoomNames();
            }
        }

        public void AddMemento(ItemMemento aMemento)
        {
            sessionRecorder.AddMemento(aMemento);
        }

        // onScene change
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // do nothing for now
        }

        public int GetNextMementoID(int aID, FactoryItem aFI)
        {
            return sessionRecorder.GetMementoID(aID, aFI);
        }

        #endregion

        #region Constructors
        // make Singelten
        private ItemMementoManager()
        {
            // set defaults
            SetDefaults();
        } 
        
        #endregion


        #region Accessors
        // Singelten Accessors
        public static ItemMementoManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("ItemMementoManager").AddComponent<ItemMementoManager>();
                }
                return instance;
            }
        }

        // Access for all pooled items
        public List<Poolable> PooledItems
        {
            get
            {
                return abf_SCO.PooledItems;
            }

        }

        // Access for test record path
        public string TestRecordPath
        {
            get
            {
                return testRecordPath;
            }
        }
        
        // Access for time between memento records
        public float TimeBetweenMementoRecords
        {
            // time Memento Records = TimeBetweenMementoRecords divided by current pooled  item count
            get
            {
                return timeBetweenMementoRecords;
            }
        }   
        #endregion
    }
}