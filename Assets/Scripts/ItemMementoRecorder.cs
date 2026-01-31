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
            // get a ObjectPlacement to base the memento on
            ObjectPlacement objP = GetComponent<PlacableFactoryItem>().ObjectPlacement();

            // create the memento
            ItemMemento itemMemento = new ItemMemento(objP);

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