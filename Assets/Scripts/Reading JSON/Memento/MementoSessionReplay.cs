// Created By   : Isaac Bustad
// Created      : 2/15/2026


using System.Collections;
using System.Collections.Generic;
using BugFreeProductions.Tools;
using UnityEngine;
using System.Linq;


namespace BugFreeProductions.Tools
{
    public enum PlaybackModifier
    {
        fastForward,
        rewind,
        pause,
        resume
    }


    public class MementoSessionReplay : Subscription 
    {
        #region Vars
        // instance for singelton
        protected static MementoSessionReplay instance = null;

        // dictionary of unique items in mementos remove if not used
        protected Dictionary<int,MementoPlayer> mementoPlayerByInt = new Dictionary<int, MementoPlayer>();

        // list of mementos in recorded file
        protected List<ItemMemento> playbackMementos = new List<ItemMemento>();

        // list of MementoPlayers
        protected List<MementoPlayer> mementoPlayers = new List<MementoPlayer>();

        // bool to tell to play
        protected bool isPlaying = false;

        // bool to tell if paused
        protected bool isPaused = false;

        // batch size
        protected int maxBatchSize = 5;

        // current playback time
        protected double playbackTime = 0.00;

        #endregion // Vars

        #region Methods

        #region Replay Methods
        // begin playback of recording
        public virtual void BeginPlayback(string recordingPath)
        {   // path for testing to be removed later
            string recordingTestPath = "/" + "RecordTest" + ".json";

            // begin by loading the recording into memory
            LoadRecording(recordingTestPath);

            // set starting time
            if (playbackMementos.Count > 0)
            {
                playbackTime = playbackMementos[0].timestamp;
            }

            isPlaying = true;
        }

        // continues playback of the recording
        public virtual void ContinuePlayback(double aDeltaTime)
        {
            // ToDo: use loaded mementos to replay recording

            // ToDo: create a new memento player for ids not previously processed
            
            
            

            // ToDo: limit the number of mementos processed in a frame to max batch size

            // ToDo: Use Linq reuse existing memento players

            
        }

        protected virtual void ReplayMemento(ItemMemento aIM)
        {
            // select a single Player from Memento Players if the ID is Present
            MementoPlayer aIMP = mementoPlayers.Where(mp => mp.MemID == aIM.memID).Single();

            if (aIMP != null)
            {
                aIMP.PlayMemento(aIM);
            }

        }

        

        // completely end playback of recording
        public virtual void EndPlayback()
        {
            // state we are no longer playing recording back
            isPlaying = false;

            // empty the recording from memory
            playbackMementos = new List<ItemMemento>();
        }

        public virtual void ModifyPlayback(PlaybackModifier aPlaybackModifier)
        {
            switch (aPlaybackModifier)
            {

                case PlaybackModifier.pause:
                    PausePlayback();
                    break;

                case PlaybackModifier.resume:
                    ResumePlayback();
                    break;

                case PlaybackModifier.rewind:
                    RewindPlayback();
                    break;

                case PlaybackModifier.fastForward:
                    FastForwardPlayback();
                    break;

            }
                
        }

        protected virtual void ResumePlayback()
        {
            
        }

        protected virtual void RewindPlayback()
        {
            
        }

        protected virtual void FastForwardPlayback()
        {
            
        }

        // pause playback of a recording
        protected virtual void PausePlayback()
        {
            // toggle on and off pause bool
            isPaused = !isPaused;
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

        #endregion // Constructors

        #region Accessors
        public static MementoSessionReplay Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new MementoSessionReplay();
                }

                return instance;
            }
        }
        #endregion // Accessors
    }
}