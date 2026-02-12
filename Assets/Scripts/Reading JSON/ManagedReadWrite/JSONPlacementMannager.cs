// Isaac Bustad
// 4/3/2025


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;


namespace BugFreeProductions.Tools
{
    public class JSONPlacementMannager : MonoBehaviour
    {
        // Vars
        // Singelten instance
        private static JSONPlacementMannager instance = null;

        // factory References
        [SerializeField] private AbstractFactory_SCO abf_SCO = null;

        // list of all Factory Item objects in the scene kept for optimization and memento access
        private List<FactoryItem> factoryItems  = new List<FactoryItem>();

        // pathing variables
        [SerializeField] private string roomConfigPath = "N/A";
        private string objectPlacementPath = "ObjectPlacements.json";
        private string roomPlacementPath = "RoomPointPlacements.json";
        private string roomNamePath = "RoomNames.json";

        // not a room reference
        private string notRoom = "N/A";


        // Mannaged readers and writers
        private MannagedJSONReader jsonReader = new MannagedJSONReader();
        private MannagedJSONWriter jsonWriter = new MannagedJSONWriter();

        // Room object ID and Pool
        private string roomID = "Room";
        [SerializeField] private GenericPool pool = new GenericPool();

        // Methods
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
            ReadRoomConfig();
        }

        // remove from delegate on destroy
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        #region Room Saving
        public void ReadRoomConfig()
        {
            if (roomConfigPath != notRoom)
            {
                jsonReader.SpawnObjects("/" + roomConfigPath + objectPlacementPath);
                FindObjectOfType<RoomGenerator>().LoadIntoRoom();
                FindObjectOfType<RoomGenerator>().HideFloorPoints();
                //FindObjectOfType <VRInputMapManager>().SwitchToDefaultMode(false);
            }
            
        }

        public void WriteRoomConfig()
        {
            jsonWriter.WriteObjPlacementData("/" + roomConfigPath + roomPlacementPath, "/" + roomConfigPath + objectPlacementPath);
        }
        

        

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

    }
}