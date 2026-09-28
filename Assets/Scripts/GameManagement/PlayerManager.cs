// Isaac Bustad
// 10/22/2024


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BugFreeProductions.VRClassroom
{
    public class PlayerManager : MonoBehaviour
    {
        // Vars        
        protected int playerCount = 0;
        protected Dictionary<int,Transform> players = new Dictionary<int,Transform>();

        // for camera viewport changes
        protected CameraViewportManager cvm;


        // Methods
        protected virtual void OnEnable()
        {
            // set up singalton // possibly move to awake later
            SingleRacePlayerManager();

            // Collect refferences for later uses
            CollectVars();
        }

        protected virtual void CollectVars()
        {
            cvm = GetComponent<CameraViewportManager>();
        }

        // test methods for ref
        public virtual void OnPlayerJoin(PlayerInput aPI)
        {
            playerCount++;
            players.Add(playerCount, aPI.gameObject.transform);

            // Change cam viewports
            ChangeCamViewport(aPI.camera ,true);
            //Debug.Log(aPI.camera.name);

        }

        

        protected virtual void ChangeCamViewport(Camera aCam, bool addPlayer)
        {
            if (cvm != null)
            {
                cvm.ChangeCamViewport( aCam, addPlayer);
            }
        }


        // Make a singalton
        protected void SingleRacePlayerManager()
        {
             
            List<PlayerManager> pms = FindObjectsOfType<PlayerManager>().ToList();
            pms.Remove(this);

            foreach (PlayerManager p in pms)
            {
                Destroy(p.gameObject);
            }

            DontDestroyOnLoad(gameObject);
            

        }


        // Accessors
    }
}