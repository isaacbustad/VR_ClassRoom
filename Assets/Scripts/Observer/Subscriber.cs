// Created By   : Isaac Bustad
// Created      : 12/2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BugFreeProductions.Tools
{
    // interface for subscribers to the memento system
public interface Subscriber
{
    // method to recieve update from subscrition
    public void OnNotify();

    // method to subscribe to SubscriptionService
    public void Subscribe();

    // method to unsubscribe from SubscriptionService
    public void Unsubscribe();

}
}