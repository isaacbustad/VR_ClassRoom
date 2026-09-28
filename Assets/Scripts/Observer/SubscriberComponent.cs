// Created by   :   Isaac Bustad
// Created      :   6/26/2026



using System.Collections;
using System.Collections.Generic;
using BugFreeProductions.VRClassroom;
using UnityEngine;

public abstract class SubscriberComponent : MonoBehaviour, Subscriber
{
    #region Methods
    #region Subscriber Methods
    public virtual void OnNotify()
    {
        OnNotification();
    }

    public virtual void Subscribe()
    {
        OnSubscribe();
    }

    public virtual void Unsubscribe()
    {
        OnUnsubscribe();
    }
    #endregion Subscriber Methods

    #region Overridable Subscriber Actions
    protected virtual void OnSubscribe()
    {
        throw new System.NotImplementedException();
    }

    protected virtual void OnUnsubscribe()
    {
        throw new System.NotImplementedException();
    }

    protected virtual void OnNotification()
    {
        throw new System.NotImplementedException();
    }

    #endregion Overridable Subscriber Actions

    #endregion Methods

    
}
