// Isaac Bustad
// 4/3/2025

//#define NotTesting

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;



namespace BugFreeProductions.VRClassroom
{
    public class JSONPlacementMannager : MonoBehaviour, Subscription
    {
        #region  Vars
        // Singelten instance
        protected static JSONPlacementMannager instance = null;

        // factory References
        [SerializeField] protected AbstractFactory_SCO abf_SCO = null;

        // list of all Factory Item objects in the scene kept for optimization and memento access
        protected List<FactoryItem> factoryItems  = new List<FactoryItem>();

        // pathing variables
        [SerializeField] protected string roomConfigPath = "N/A";
        protected string objectPlacementPath = "ObjectPlacements.json";
        protected string roomPlacementPath = "RoomPointPlacements.json";
        protected string roomNamePath = "RoomNames.json";

        // not a room reference
        protected string notRoom = "N/A";


        // Mannaged readers and writers
        protected MannagedJSONReader jsonReader = new MannagedJSONReader();
        protected MannagedJSONWriter jsonWriter = new MannagedJSONWriter();

        // Room object ID and Pool
        protected string roomID = "Room";
        [SerializeField] protected GenericPool pool = new GenericPool();
        
        #region New Observer System
        [SerializeField] protected List<Subscriber> subscribers = new List<Subscriber>();

        #endregion New Observer System
        
        
        #endregion Vars
        // Methods
        protected virtual void OnEnable()
        {
            Setup();
        }

        protected virtual void Setup()
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
        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ReadRoomConfig();
        }

        // remove from delegate on destroy
        protected virtual void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        #region Room Saving
        public virtual void ReadRoomConfig()
        {
            if (roomConfigPath != notRoom)
            {
                jsonReader.SpawnObjects("/" + roomConfigPath + objectPlacementPath);
                #if NotTesting
                FindObjectOfType<RoomGenerator>().LoadIntoRoom();
                FindObjectOfType<RoomGenerator>().HideFloorPoints();
                #endif
                //FindObjectOfType <VRInputMapManager>().SwitchToDefaultMode(false);
            }
            
        }

        public virtual void WriteRoomConfig()
        {
            jsonWriter.WriteObjPlacementData("/" + roomConfigPath + roomPlacementPath, "/" + roomConfigPath + objectPlacementPath);
        }
        

        #region Subscription Methods
        // add a subscriber to the Subsctition
        public virtual void AddSubscriber(Subscriber aSub)
        {
            subscribers.Add(aSub); 

            
        }

        // remove a subscriber from the Subscription
        public virtual void RemoveSubscriber(Subscriber aSub)
        {
            subscribers.Remove(aSub); 
        }

        // notify all subscribers
        public virtual void NotifySubscribers()
        {
            
        }
        #endregion

        

        #endregion

        

        // Accessors
        // Singelten Accessors
        public static JSONPlacementMannager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("JSONPlacementMannager").AddComponent<JSONPlacementMannager>();
                }
                return instance;
            }
        }

        // Accessors for managed read write
        public AbstractFactory_SCO ABF_SCO { get { return abf_SCO; } }

        public string RoomID { get { return roomID; } }

        public GenericPool Pool { get { return pool; } set { pool = value; } }

        public string RoomConfigPath { get { return roomConfigPath; } set {  roomConfigPath = value; } }

        public List<string> RoomList { get { return ReadRoomsInPath.FindRoomNames(); } }

        public string ObjectPlacementPath { get { return objectPlacementPath; }  }

        public string NotRoom { get { return notRoom; } }

        // Access list of factory items
        public List<FactoryItem> FactoryItems
        {
            get
            {
                return factoryItems;
            }
        }

        #region NewSubscription Accessors
        
        #endregion NewSubscription Accessors
        

    }
}