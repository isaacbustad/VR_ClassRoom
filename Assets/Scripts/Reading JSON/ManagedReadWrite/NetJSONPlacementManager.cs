// Created by   :   Isaac Bustad
// Created      :   6/26/2026


using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace BugFreeProductions.VRClassroom
{
    [RequireComponent(typeof(NetworkIdentity))]
    public class NetJSONPlacementManager : JSONPlacementMannager
    {
        #region Vars
        NetworkIdentity ni = null;
        #endregion Vars

        #region Methods
        protected override void Setup()
        {
            // do the basic setup
            base.Setup();

            // get NetworkIdentity to use network atributes
            ni = GetComponent<NetworkIdentity>();
        }


        protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            base.OnSceneLoaded(scene, mode);
            
            // loop through subscribers
            foreach(Subscriber aSub in subscribers)
            {
                if (aSub is NetPlacableItem anItem)
                {
                    NetworkServer.Spawn(anItem.gameObject);

                }
            }
        }

        // public override void WriteRoomConfig()
        // {
        //     if (ni.isServer)
        //     {
        //         base.WriteRoomConfig();
        //     }
        // }
        
        #endregion Methods
    }
}