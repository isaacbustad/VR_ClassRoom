using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMementoManager : MonoBehaviour
{
    #region Vars
    // hold the recording of all mementos
    protected List<ItemMemento> mementos = new List<ItemMemento>();

    // hold the start time of recording / 
    protected float startTime = 0;

    // hold path
    protected string recordPath = "/record/test";

    #endregion

}
