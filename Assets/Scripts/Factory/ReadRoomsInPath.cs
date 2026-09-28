// Isaac Bustad
// 4/17/2025


using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using BugFreeProductions.VRClassroom;

public class ReadRoomsInPath
{
    // Vars



    // Methods
    static public List<string> FindRoomNames()
    {
        // collect found rooms
        List<string> roomsFound = new List<string>();

        // paths in directory
        List<string> roomPaths = new List<string>();
        roomPaths = Directory.GetFiles(Application.persistentDataPath).ToList();

        foreach (string name in roomPaths)
        {
            string fileName = Path.GetFileName(name);

            if (fileName.EndsWith(JSONPlacementMannager.Instance.ObjectPlacementPath))
            {
                string roomName = fileName.Substring(0, fileName.Length - JSONPlacementMannager.Instance.ObjectPlacementPath.Length);

                roomName = roomName.Trim();

                if (!string.IsNullOrEmpty(roomName))
                {
                    roomsFound.Add(roomName);
                    //Debug.Log($"Found room: '{roomName}'");
                }
            }
        }

        foreach (string r in roomsFound)
        {
            //Debug.Log("room = " + r);
        }
        
        return roomsFound;
    }


    // Accessors
}
