// Created by   : Isaac Bustad
// Created      : 2/12/2026


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.Tools
{
    public interface Subscription 
    {
        // add a subscriber to the Subsctition
        public void AddSubscriber(Subscriber subscriber);

        // remove a subscriber from the Subscription
        public void RemoveSubscriber(Subscriber subscriber);
        protected void NotifySubscribers(string message);

        


    }    
}

