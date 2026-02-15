// Created by   : Isaac Bustad
// Created      : 2/15/2026


using System.Collections;
using System.Collections.Generic;
using BugFreeProductions.Tools;
using UnityEngine;


public class MementoPlayer : MonoBehaviour, Subscriber
{
    #region Vars
    protected ItemMemento memento = null;

    #endregion // Vars

    #region Methods

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
        
    }

    #endregion // Subscriber Methods

    #endregion // Methods

    #region Accessors

    #endregion // Accessors

}
