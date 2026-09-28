// Isaac Busatd
// 11/6/2024

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BugFreeProductions.VRClassroom
{
    public class CustomGatewayJSON
    {
        // Vars
        //[SerializeField] private TextAsset skillFile;
        private static CustomGatewayJSON instance;

        private string defaultFileText = "NA";



        // Methods
        public string ReadJsonFile(string aPath)
        {
            StreamReader streamReader = null;
            string jsonTxt = null;

            if (File.Exists(Application.persistentDataPath + aPath))
            {
                streamReader = new StreamReader(Application.persistentDataPath + aPath);
                jsonTxt = streamReader.ReadToEnd();
                streamReader.Close();
                Debug.Log(Application.persistentDataPath + aPath);
                return jsonTxt;

            }

            else
            {
                WriteJsonFile(aPath, DefaultFileText);

                streamReader = new StreamReader(Application.persistentDataPath + aPath);
                jsonTxt = streamReader.ReadToEnd();
                streamReader.Close();
                Debug.Log(Application.persistentDataPath + aPath);
                return jsonTxt;
            }
            

            //StreamReader streamReader = new StreamReader(Application.persistentDataPath + aPath);
            /*if (streamReader == null)
            {
                // Close current writer
                streamReader.Close();

                // write a file to path
                WriteJsonFile(Application.persistentDataPath + aPath, " ");

                // re-define streamReader
                streamReader = new StreamReader(Application.persistentDataPath + aPath);

            }*/
            //string jsonTxt = streamReader.ReadToEnd();
            //Debug.Log(jsonTxt);
            //streamReader.Close();
            //return jsonTxt;

        }

        public void WriteJsonFile(string aPath, string anObjStr)
        {
            StreamWriter streamWriter = new StreamWriter(Application.persistentDataPath + aPath);
            streamWriter.Write(anObjStr);
            streamWriter.Close();
            Debug.Log(Application.persistentDataPath + aPath);
        }




        // Constructors
        private CustomGatewayJSON() { }


        // Accessors
        public static CustomGatewayJSON Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomGatewayJSON();
                }
                return instance;
            }
        }
        public string DefaultFileText { get { return defaultFileText; } }
    }
}