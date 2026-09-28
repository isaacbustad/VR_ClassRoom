// Isaac Bustad
// 9/17/2025


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BugFreeProductions.VRClassroom
{
    // records a memento of a game object
    public class ItemMementoRecorder : MonoBehaviour , Subscriber
    {

        #region Vars
        // instance ID to be used for recording specific item
        protected int instanceID = -1;

        #endregion


        #region Methods

        #region Unity Event Methods
        protected virtual void OnEnable()
        {
            Debug.Log($"Copilot ItemMementoRecorder.OnEnable: subscribing recorder, current instanceID={instanceID}");
            // subscribe on enable to ensure object mementos are collected
            Subscribe();

        }

        protected virtual void OnDestroy()
        {
            // Record a memento stating the object was destroyed
            Debug.Log($"Copilot ItemMementoRecorder.OnDestroy: recorder instanceID={instanceID}");
            RecordMemento(true);

            // remove from the subscription before destruction
            Unsubscribe();
        }

        #endregion // Unity Event Methods

        public virtual void RecordMemento(bool isDestroyed)
        {
            // ref to FactoryItem
            FactoryItem factoryItem = GetComponent<FactoryItem>();

            // get a ObjectPlacement to base the memento on
            ObjectPlacement objP = factoryItem.ObjectPlacement();

            // create the memento
            ItemMemento itemMemento = new ItemMemento(objP);

            // set the memento's unique identifier for both playback and serialized instance tracking
            itemMemento.memID = instanceID;
            itemMemento.instanceID = instanceID;
            //Debug.Log($"Copilot ItemMementoRecorder.RecordMemento: recorder instanceID={instanceID}, itemMemento.memID={itemMemento.memID}, itemMemento.instanceID={itemMemento.instanceID}");

            // if ()
            // {
            //     // Set the instance id of this reusable recording object
            //     instanceID = MementoSessionRecorder.Instance.GetMementoID(this);
            //     itemMemento.memID = MementoSessionRecorder.Instance.GetMementoID(this);
            // }

            // else
            // {
            //     Debug.LogError("MementoSessionRecorder is not initialized.");
            // }

            // this.instanceID = itemMemento.memID;

            // add to manager
            MementoSessionRecorder.Instance.AddMemento(itemMemento);

            // set a mementos is destroyed value
            itemMemento.IsDestroyed = isDestroyed;
        }

        #region Subscriber Methods
        public void OnNotify()
        {

        }
        public void Subscribe()
        {
            MementoSessionRecorder.Instance.AddSubscriber(this);
        }

        public void Unsubscribe()
        {
            MementoSessionRecorder.Instance.RemoveSubscriber(this);
        }
        #endregion // Subscriber Methods

        #endregion // Methods


        #region Accessors
        public int InstanceID
        {
            get
            {
                return instanceID;
            }

            set
            {
                instanceID = value;
            }
        }

        #endregion // Accessors


    }
}