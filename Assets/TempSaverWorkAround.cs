using BugFreeProductions.VRClassroom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSaverWorkAround : MonoBehaviour
{
    public void SaveRoom()
    {
        JSONPlacementMannager.Instance.WriteRoomConfig();
    }
}
