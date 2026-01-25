// Isaac Bustad
// 9/17/2025


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace BugFreeProductions.Tools
{
    // manages playback of item mementos
    public class ItemMementoManager : MonoBehaviour
    {
        #region Vars

        // Singelten instance
        private static ItemMementoManager instance = null;

        // hold the recording of all mementos
        protected List<ItemMemento> mementos = new List<ItemMemento>();
        protected MementoSessionRecorder sessionRecorder = new MementoSessionRecorder();

        // reference to Abstract Factory
        [SerializeField] private AbstractFactory_SCO abf_SCO = null;
        
        // hold path
        protected string recordPath = "/record/test";


        #endregion

        #region Methods
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

        // onScene change
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // do nothing for now
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
        #endregion
    }
}