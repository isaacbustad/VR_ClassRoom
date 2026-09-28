// Created By   : Isaac Bustad
// Created      : 2/15/2026


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace BugFreeProductions.VRClassroom
{
    public class MementoSessionReplay : MonoBehaviour, Subscription 
    {
        #region Vars
        // instance for singelton
        protected static MementoSessionReplay instance = null;

        // list of mementos in recorded file
        protected List<ItemMemento> playbackMementos = new List<ItemMemento>();

        // list of MementoPlayers
        protected List<MementoPlayer> mementoPlayers = new List<MementoPlayer>();

        // list to track objects created by replay for cleanup
        protected List<GameObject> replayCreatedObjects = new List<GameObject>();

        // bool to tell to play
        protected bool isPlaying = false;

        // bool to tell if paused
        protected bool isPaused = false;

        // batch size
        protected int maxBatchSize = 5;

        // current playback time
        protected double playbackTime = 0.00;

        // current playback index
        protected int playbackIDX = 0;

        // replay keys
        protected KeyCode startReplayKey = KeyCode.N;
        protected KeyCode endReplayKey = KeyCode.M;
        protected KeyCode clearReplayObjectsKey = KeyCode.C;

        

        #endregion // Vars

        #region Methods

        #region Unity Methods
        protected virtual void Update()
        {
            // Check for replay key presses
            CheckReplayKeys();

            if (isPlaying == true && !isPaused)
            {
                ContinuePlayback(Time.deltaTime);
            }
        }

        protected virtual void CheckReplayKeys()
        {
            if (Input.GetKeyDown(startReplayKey))
            {
                if (!isPlaying)
                {
                    BeginPlayback("RecordTest");
                    Debug.Log("MementoSessionReplay: Started replay.");
                }
            }

            if (Input.GetKeyDown(endReplayKey))
            {
                if (isPlaying)
                {
                    EndPlayback();
                    Debug.Log("MementoSessionReplay: Stopped replay.");
                }
            }

            if (Input.GetKeyDown(clearReplayObjectsKey))
            {
                ClearReplayObjects();
                Debug.Log("MementoSessionReplay: Cleared replay objects.");
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ResumePlayback();
                Debug.Log("MementoSessionReplay: Resume playback.");
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                TogglePausePlayback();
                Debug.Log($"MementoSessionReplay: Playback {(isPaused ? "paused" : "resumed")}.");
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                RewindPlayback();
                Debug.Log("MementoSessionReplay: Rewind playback.");
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                FastForwardPlayback();
                Debug.Log("MementoSessionReplay: Fast-forward playback.");
            }
        }

        #endregion

        #region Replay Methods
        // begin playback of recording
        public virtual void BeginPlayback(string recordingPath)
        {   
            // begin by loading the recording into memory
            LoadRecording("/" + recordingPath + ".json");

            // set starting time
            if (playbackMementos.Count > 0)
            {
                playbackTime = playbackMementos[0].timestamp;
            }

            isPlaying = true;
            playbackIDX = 0;
        }

        // continues playback of the recording
        public virtual void ContinuePlayback(double aDeltaTime)
        {
            // ToDo: use loaded mementos to replay recording
            playbackTime += aDeltaTime;

            // // recording rate means that more than the max frames will not be able to replayed

            if (ModifyPlayback() == false)
            {
                // index increment after playback
                
                while(playbackIDX < playbackMementos.Count && playbackMementos[playbackIDX].timestamp <= playbackTime)
                {
                    ReplayMemento(playbackMementos[playbackIDX]);
                    playbackIDX++;
                }

                // auto-stop when reaching end of mementos
                if (playbackIDX >= playbackMementos.Count && playbackMementos.Count > 0)
                {
                    EndPlayback();
                    Debug.Log("MementoSessionReplay: Reached end of mementos, stopping playback.");
                }
            }
            
        }

        protected virtual void ReplayMemento(ItemMemento aIM)
        {
            // select a single Player from Memento Players if the ID is Present
            //MementoPlayer aIMP = mementoPlayers.Where(mp => mp.MemID == aIM.memID).Single();
            MementoPlayer aIMP = mementoPlayers.FirstOrDefault(m => m.MemID == aIM.memID);
            Debug.Log("Called pre-me");

            if (aIMP != null)
            {
                aIMP.PlayMemento(aIM);
                if (aIM.isDestroyed)
                {
                    aIMP.gameObject.SetActive(false);
                }
            }

            else
            {
                FactoryItem aFI = null;


                //aIMP = 
                ItemMementoManager.Instance.AbstractFactory_SCO.CreateItem(ref aFI, aIM);

                // track newly created object for cleanup
                replayCreatedObjects.Add(aFI.gameObject);

                // attempt to get the MementoPlayer component from instantiated factory item
                aIMP = aFI.GetComponent<MementoPlayer>();

                // make sure there is at least the default MementoPlayer
                if (aIMP == null)
                {
                    aIMP = aFI.gameObject.AddComponent<MementoPlayer>();
                }
                aIMP.PlayMemento(aIM);

                if (!mementoPlayers.Contains(aIMP))
                {
                    mementoPlayers.Add(aIMP);
                }

                // finalize the newly created item
                PlacableFactoryItemBody placableBody = aFI.GetComponent<PlacableFactoryItemBody>();
                if (placableBody != null)
                {
                    placableBody.FinalizeForReplay();
                }

                if (aIM.isDestroyed)
                {
                    aIMP.gameObject.SetActive(false);
                }
            }
        }

        

        // completely end playback of recording
        public virtual void EndPlayback()
        {
            // state we are no longer playing recording back
            isPlaying = false;

            // reset playback state
            playbackTime = 0.0;
            playbackIDX = 0;

            // empty the recording from memory
            playbackMementos = new List<ItemMemento>();
        }

        // clear all objects created by replay from the scene
        protected virtual void ClearReplayObjects()
        {
            foreach (GameObject obj in replayCreatedObjects)
            {
                if (obj != null)
                {
                    MementoPlayer player = obj.GetComponent<MementoPlayer>();
                    if (player != null)
                    {
                        mementoPlayers.Remove(player);
                    }

                    Destroy(obj);
                }
            }
            replayCreatedObjects.Clear();

            mementoPlayers.RemoveAll(mp => mp == null || mp.gameObject == null);
        }

        public virtual bool ModifyPlayback()
        {
            return isPaused;
        }

        protected virtual void ResumePlayback()
        {
            isPaused = false;
        }

        protected virtual void TogglePausePlayback()
        {
            isPaused = !isPaused;
        }

        protected virtual void RewindPlayback()
        {
            if (playbackMementos.Count == 0)
            {
                return;
            }

            double rewindAmount = 1.0;
            double targetTime = playbackTime - rewindAmount;
            if (targetTime < playbackMementos[0].timestamp)
            {
                targetTime = playbackMementos[0].timestamp;
            }

            ResetPlaybackToTime(targetTime);
        }

        protected virtual void FastForwardPlayback()
        {
            if (playbackMementos.Count == 0)
            {
                return;
            }

            double fastForwardAmount = 1.0;
            double targetTime = playbackTime + fastForwardAmount;
            if (targetTime > playbackMementos[playbackMementos.Count - 1].timestamp)
            {
                targetTime = playbackMementos[playbackMementos.Count - 1].timestamp;
            }

            ResetPlaybackToTime(targetTime);
        }

        protected virtual void ResetPlaybackToTime(double targetTime)
        {
            playbackTime = targetTime;
            playbackIDX = 0;

            ClearReplayObjects();

            while (playbackIDX < playbackMementos.Count && playbackMementos[playbackIDX].timestamp <= playbackTime)
            {
                ReplayMemento(playbackMementos[playbackIDX]);
                playbackIDX++;
            }
        }

        // pause playback of a recording
        protected virtual void PausePlayback()
        {
            isPaused = true;
        } 

        // load the recording into memento list
        protected virtual void LoadRecording(string recordingPath)
        {
            playbackMementos = MementoReadWrite.Instance.ReadItemMementos(recordingPath);
        }

        #endregion // Replay Methods

        #region Subscription Methods
        // add a subscriber to the Subscription
        public void AddSubscriber(Subscriber aSub)
        {

            if (aSub is MementoPlayer aMP)
            {
                mementoPlayers.Add(aMP);
            }
        }

        // remove a subscriber from the Subscription
        public void RemoveSubscriber(Subscriber aSub)
        {
            if (aSub is MementoPlayer aMP)
            {
                mementoPlayers.Remove(aMP);
            }
        }

        // notify
        public void NotifySubscribers()
        {
            
        }
        #endregion

        #endregion // Methods

        #region Constructors 
        protected MementoSessionReplay()
        {
            
        }

        #endregion

        #region Accessors
        public static MementoSessionReplay Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("MementoSessionReplay").AddComponent<MementoSessionReplay>();
                    DontDestroyOnLoad(instance.gameObject);
                }
                return instance;
            }
        }
        #endregion // Accessors
    }
}