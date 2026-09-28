// Isaac Bustad
// 11/6/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;


namespace BugFreeProductions.VRClassroom
{
    public class ObjectPlacementReadWrite
    {
        protected string placementPath = "/ObjectPlacements.json";
        //protected string monstFilterName = "BtlShpTurt";
        //public MonsterStatList statList = new MonsterStatList();

        // singal instance
        private static ObjectPlacementReadWrite instance;






        public List<ObjectPlacement> ReadObjectPlacements()
        {
            // hold a returnable list
            List<ObjectPlacement> retLST = new List<ObjectPlacement>();

            // hold String ref for json check
            string jsonSTR = CustomGatewayJSON.Instance.ReadJsonFile(placementPath);

            // if not default file value
            if (jsonSTR != CustomGatewayJSON.Instance.DefaultFileText)
            {
                retLST = JsonUtility.FromJson<ObjectPlacementList>(CustomGatewayJSON.Instance.ReadJsonFile(placementPath)).objectPlacements.ToList();
            }
           
            return retLST;
        }

        public ObjectPlacement FindObjectPlacement(string aID)
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

        // reding objects based on passed file path
        public List<ObjectPlacement> ReadObjectPlacements(string aFilePath)
        {
            // hold a returnable list
            List<ObjectPlacement> retLST = new List<ObjectPlacement>();

            // hold String ref for json check
            string jsonSTR = CustomGatewayJSON.Instance.ReadJsonFile(aFilePath);

            // if not default file value
            if (jsonSTR != CustomGatewayJSON.Instance.DefaultFileText)
            {
                retLST = JsonUtility.FromJson<ObjectPlacementList>(CustomGatewayJSON.Instance.ReadJsonFile(aFilePath)).objectPlacements.ToList();
            }

            return retLST;
        }

        // 
        public ObjectPlacement FindObjectPlacement(string aID, string aFilePath)
        {
            ObjectPlacementList objLST = JsonUtility.FromJson<ObjectPlacementList>(CustomGatewayJSON.Instance.ReadJsonFile(aFilePath));

            foreach (ObjectPlacement op in objLST.objectPlacements)
            {
                if (op.id == aID)
                {
                    return op;
                }
            }
            return null;
        }

        // Writing placements
        public void WriteObjectPlacements(ObjectPlacementList aPlacementLst)
        {
            string JSONstr = JsonUtility.ToJson(aPlacementLst);

            CustomGatewayJSON.Instance.WriteJsonFile(placementPath, JSONstr);
        }

        // Writing placements based on a passed in file path
        public void WriteObjectPlacements(ObjectPlacementList aPlacementLst, string aSavePath)
        {
            string JSONstr = JsonUtility.ToJson(aPlacementLst);

            CustomGatewayJSON.Instance.WriteJsonFile(aSavePath, JSONstr);
        }


        // Constructors
        private ObjectPlacementReadWrite() { }


        // Accessors
        public static ObjectPlacementReadWrite Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ObjectPlacementReadWrite();
                }
                return instance;
            }
        }
    }
}