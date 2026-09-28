// Isaac Bustad
// 2/4/2025


using BugFreeProductions.VRClassroom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace BugFreeProductions.VRClassroom
{
    public class FactoryItemPlacer : MonoBehaviour
    {
        // Vars
        [SerializeField] protected string itemID = "NA";
        [SerializeField] protected AbstractFactory_SCO itemFactory = null;



        // Methods
        // public for input testing
        public virtual void UsePlacer(InputAction.CallbackContext aCon)
        {
            PlaceItem();
        }

        protected virtual void PlaceItem()
        {
            
        }



        // Accessors




    }
}