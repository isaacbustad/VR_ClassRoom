// Isaac Bustad
// 1/27/25



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace BugFreeProductions.VRClassroom
{
    public class ObjectPrefferenceMannager
    {
        // Vars
        // hold instance
        private static ObjectPrefferenceMannager instance = null;

        #region Propertys of an instance
        private List<ObjectPrefference> objectPrefferences = new List<ObjectPrefference>();

        #endregion







        // Methods
        // Read JSON objectPrefferences file into List
        #region Methods for instance
        public void RefreshJSONToList()
        {
            ReadJSONToList();
        }

        // Read JSON only alow Manager direct access
        private void ReadJSONToList()
        {
            objectPrefferences = ObjectPrefferenceReadWrite.Instance.ReadObjectPrefferences();

        }

        // allow outside class to save JSON
        public void SavePrefferences()
        {
            WritJSONToList();
        }

        // Hamdle actual writing to Json
        protected void WritJSONToList()
        {
            ObjectPrefferenceList nObjectPrefferences = new ObjectPrefferenceList();

            nObjectPrefferences.objectPrefferences = objectPrefferences.ToArray();

            ObjectPrefferenceReadWrite.Instance.WriteObjectPrefferences(nObjectPrefferences);
        }


        #endregion

        // Constructors
        private ObjectPrefferenceMannager()
        {

        }




        // Accessors
        // create and access Singalton instance
        #region Static Accessors
        public static ObjectPrefferenceMannager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ObjectPrefferenceMannager();
                }
                 instance.ReadJSONToList();
                return instance;
            }
        }

        #endregion

        #region Accessors of instance
        public List<ObjectPrefference> ObjectPrefferences 
        { 
            get 
            {
                if (objectPrefferences.Count == 0)
                {
                    ReadJSONToList();
                }
                return 
                    objectPrefferences; 
            } 
        }
        

        #endregion



    }
}