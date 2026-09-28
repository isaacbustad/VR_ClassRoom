// Isaac Bustad
// 8/1/2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.VRClassroom
{
    public class GameMode : MonoBehaviour
    {
        // Vars
        [SerializeField] private GameMode gMode;

        #region Vars For Player Configs
        protected Dictionary<int, RacerSetting> racerSettings = new Dictionary<int, RacerSetting>();

        #endregion

        // Methods
        private void Awake()
        {
            SingleRaceManager();

        }

        #region Racer Selection Methods
        // method to call to set Racer Settings for players
        protected void SellectRacer(int aID)
        {

        }

        protected bool CheckIfSelected(int aID)
        {


            return false;
        }

        // Fill Remaining slots for NonRacers


        #endregion
        // Make sure there is one GameMode Mannager
        private void SingleRaceManager()
        {
            GameMode[] gModes = FindObjectsOfType<GameMode>();
            if (gModes.Length > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }

        }

        // Accessors
        public Dictionary<int, RacerSetting> RacerSettings { get { return racerSettings; } }
    }
}
