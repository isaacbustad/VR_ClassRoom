// Isaac Bustad
// 9/17/2025


using BugFreeProductions.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BugFreeProductions.Tools
{
    // records a memento of a game object
    public class ItemMementoRecorder : MonoBehaviour
    {
        #region Vars
        

        #endregion


        #region Methods
        public virtual void RecordMemento()
        {            
            // ref to FactoryItem
            FactoryItem factoryItem = GetComponent<FactoryItem>();

            // get a ObjectPlacement to base the memento on
            ObjectPlacement objP = factoryItem.ObjectPlacement();
            
            // set the memento's unique identifier
            

            // create the memento
            ItemMemento itemMemento = new ItemMemento(objP);

            // set the memento's unique identifier
            //itemMemento.memID = factoryItem.InstanceID;

            // set the memento's unique identifier by requesting one from the manager
            itemMemento.memID = ItemMementoManager.Instance.GetNextMementoID(factoryItem.InstanceID, factoryItem);

            // add to manager
            ItemMementoManager.Instance.AddMemento(itemMemento);

            // return it to caller
            //return itemMemento;
        }
        #endregion


        #region Accessors

        #endregion


    }
}