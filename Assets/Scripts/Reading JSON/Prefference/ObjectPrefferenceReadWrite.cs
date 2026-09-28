// Isaac Bustad
// 1/27/2025


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace BugFreeProductions.VRClassroom
{
    public class ObjectPrefferenceReadWrite : MonoBehaviour
    {
        // Vars
        protected string placementPath = "/ObjectPrefferences.json";
        //protected string monstFilterName = "BtlShpTurt";
        //public MonsterStatList statList = new MonsterStatList();

        // singal instance
        private static ObjectPrefferenceReadWrite instance;





        // Methods

        public List<ObjectPrefference> ReadObjectPrefferences()
        {
            // hold a returnable list
            List<ObjectPrefference> retLST = new List<ObjectPrefference>();

            // hold String ref for json check
            string jsonSTR = CustomGatewayJSON.Instance.ReadJsonFile(placementPath);

            // if not default file value
            if (jsonSTR != CustomGatewayJSON.Instance.DefaultFileText)
            {
                retLST = JsonUtility.FromJson<ObjectPrefferenceList>(CustomGatewayJSON.Instance.ReadJsonFile(placementPath)).objectPrefferences.ToList();
            }

            return retLST;
        }

        public ObjectPlacement FindObjectPrefference(string aID)
        {
            ObjectPlacementList objLST = JsonUtility.FromJson<ObjectPlacementList>(CustomGatewayJSON.Instance.ReadJsonFile(placementPath));

            foreach (ObjectPlacement op in objLST.objectPlacements)
            {
                if (op.id == aID)
                {
                    return op;
                }
            }
            return null;
        }
        public void WriteObjectPrefferences(ObjectPrefferenceList aPrefferenceLst)
        {
            string JSONstr = JsonUtility.ToJson(aPrefferenceLst);

            CustomGatewayJSON.Instance.WriteJsonFile(placementPath, JSONstr);
        }

        // Constructors
        private ObjectPrefferenceReadWrite() { }


        // Accessors
        public static ObjectPrefferenceReadWrite Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ObjectPrefferenceReadWrite();
                }
                return instance;
            }
        }

    }
}