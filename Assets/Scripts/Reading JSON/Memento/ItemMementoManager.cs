// Isaac Bustad
// 9/17/2025


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// ToDo: create list of all pooled items stored here or in ObjectPlacementManager
// ToDo: calculate time between memento records based on current pooled item count and a desired number of rounds per second

namespace BugFreeProductions.VRClassroom
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


        protected KeyCode startReplayKey = KeyCode.N;
        protected KeyCode endReplayKey = KeyCode.M;
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
        protected virtual void TestByKey()
        {
            if (Input.GetKeyDown(startRecordingKey))
            {
                Debug.Log("ItemMementoManager: Started Recording Session.");
                sessionRecorder.StartRecordingSession();
                Debug.Log("ItemMementoManager: Started Recording Session.");
            }

            // for testing the ending of a recording session and writes to
            // Linux Flatpack: /home/user/.var/app/com.unity.UnityHub/config/unity3d/BugFreeProductions
            // Windows: 
            if (Input.GetKeyDown(stopRecordingKey))
            {
                // stop recording
                sessionRecorder.StopRecordingSession();
                Debug.Log($"Copilot ItemMementoManager: Stopped Recording Session with {sessionRecorder.Mementos.Count} Mementos recorded.");
            }

            if (Input.GetKeyDown(printMementosKey))
            {
                int itemCount = 0;
                // print mementos
                foreach (ItemMemento memento in sessionRecorder.Mementos)
                {
                    itemCount++;
                    Debug.Log($"Copilot ItemMementoManager: {itemCount}: Memento ID={memento.id} memID={memento.memID} instanceID={memento.instanceID} pos=({memento.tpX}, {memento.tpY}, {memento.tpZ})");
                }
            }

            // Test case to call replay
            // Should replay saved mementos 
            // Moved to MementoSessionReplay
            // if (Input.GetKeyDown(startReplayKey))
            // {
            //     MementoSessionReplay.Instance.BeginPlayback("test");
            // }            

        }
        #endregion Test Methods

        #region Unity Methods
        protected virtual void Update()
        {
            TestByKey();

            sessionRecorder.RecordSession(Time.deltaTime);
        }

        // make Singelten on enable
        protected virtual void OnEnable()
        {
            SetDefaults();
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

        #endregion Unity Methods


        // onScene change
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // do nothing for now
        }

        #endregion Methods




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
        public virtual List<Poolable> PooledItems
        {
            get
            {
                return abf_SCO.PooledItems;
            }

        }

        // Access for test record path
        public virtual string TestRecordPath
        {
            get
            {
                return testRecordPath;
            }
        }

        // Access te abstract factory for spawning new objects
        public virtual AbstractFactory_SCO AbstractFactory_SCO
        {
            get
            {
                return abf_SCO;
            }
        }

        #endregion Accessors
    }
}