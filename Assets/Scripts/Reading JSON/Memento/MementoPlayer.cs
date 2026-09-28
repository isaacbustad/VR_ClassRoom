// Created by   : Isaac Bustad
// Created      : 2/15/2026


using System.Collections;
using System.Collections.Generic;
using BugFreeProductions.VRClassroom;
using UnityEngine;

// this would be on the Item 
// [ToDo] think about how
public class MementoPlayer : MonoBehaviour, Subscriber
{
    #region Vars
    protected int memID = -1;

    #endregion // Vars

    #region Methods

    #region Unity Methods
    protected virtual void OnEnable()
    {
        Subscribe();
    }
    protected virtual void OnDestroy()
    {
        Unsubscribe();
    }
    #endregion Unity Methods

    public virtual void PlayMemento(ItemMemento aIM)
    {
        memID = aIM.memID;
        Debug.Log("Called pre-me");
        transform.position = new Vector3(aIM.tpX, aIM.tpY, aIM.tpZ);
        transform.rotation = Quaternion.Euler(aIM.trX, aIM.trY, aIM.trZ);
    }

    #region Subscriber Methods
    // method to recieve update from subscrition
    public void OnNotify()
    {
        
    }

    // method to subscribe to SubscriptionService
    public void Subscribe()
    {
        MementoSessionReplay.Instance.AddSubscriber(this);
    }

    // method to unsubscribe from SubscriptionService
    public void Unsubscribe()
    {
        MementoSessionReplay.Instance.RemoveSubscriber(this);
    }

    #endregion // Subscriber Methods

    #endregion // Methods

    #region Accessors
    public int MemID
    {
        get
        {
            return memID;
        }
        
    }
    #endregion // Accessors

}
