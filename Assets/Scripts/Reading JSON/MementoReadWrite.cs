// Created By : Isaac Bustad
// Created On : 1/31/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BugFreeProductions.Tools
{
public class MementoReadWrite : MonoBehaviour
    {
        #region Vars
        // single instance
        private static MementoReadWrite instance;
        #endregion

        #region Methods        

        // reding objects based on passed file path
        public List<ItemMemento> ReadItemMementos(string aFilePath)
        {
            // hold a returnable list
            List<ItemMemento> retLST = new List<ItemMemento>();

            // hold String ref for json check
            string jsonSTR = CustomGatewayJSON.Instance.ReadJsonFile(aFilePath);

            // if not default file value
            if (jsonSTR != CustomGatewayJSON.Instance.DefaultFileText)
            {
                retLST = JsonUtility.FromJson<ItemMementoList>(CustomGatewayJSON.Instance.ReadJsonFile(aFilePath)).itemMementos.ToList();
            }

            return retLST;
        }

        // 
        public ItemMemento FindItemMemento(string aID, string aFilePath)
        {
            ItemMementoList objLST = JsonUtility.FromJson<ItemMementoList>(CustomGatewayJSON.Instance.ReadJsonFile(aFilePath));

            foreach (ItemMemento op in objLST.itemMementos)
            {
                if (op.id == aID)
                {
                    return op;
                }
            }
            return null;
        }

        

        // Writing placements based on a passed in file path
        public void WriteItemMementos(ItemMementoList aPlacementLst, string aSavePath)
        {
            string JSONstr = JsonUtility.ToJson(aPlacementLst);

            CustomGatewayJSON.Instance.WriteJsonFile(aSavePath, JSONstr);
        }
        
        #endregion

        // Constructors
        private MementoReadWrite() { }


        #region Accessors
        public static MementoReadWrite Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MementoReadWrite();
                }
                return instance;
            }
        }
        #endregion
    }

}