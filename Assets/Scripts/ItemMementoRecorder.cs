// Isaac Bustad
// 9/17/2025


using BugFreeProductions.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// records a memento of a game object
public class ItemMementoRecorder : MonoBehaviour
{
    #region Vars
    // hold the recording of all mementos
    protected List<ItemMemento> mementos = new List<ItemMemento>();

    // hold the start time of recording / 
    protected float startTime = 0;

    // hold path
    protected string recordPath = "/record/test";

    #endregion


    #region Methods
    public ItemMemento RecordMemento()
    {
        ItemMemento itemMemento = new ItemMemento();
        
        // get a ObjectPlacement to base the memento on
        GetComponent<PlacableFactoryItem>().ObjectPlacement();

        return itemMemento;
    }
    #endregion


    #region Accessors

    #endregion


}
