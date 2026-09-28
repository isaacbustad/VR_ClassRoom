// Isaac Bustad
// 12/4/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BugFreeProductions.VRClassroom
{
    public class PlaceableObjLoader : MonoBehaviour
    {
        // Vars
        [SerializeField] protected AbstractFactory_SCO abf = null;



        // Methods        
        protected virtual void OnEnable()
        {
            //SpawnObjects();
        }

        // Spawns the list of objects from a list
        protected virtual void SpawnObjects()
        {
            if (abf != null)
            {
                // list of object placements
                List<ObjectPlacement> objPlacements = ObjectPlacementReadWrite.Instance.ReadObjectPlacements();

                foreach (ObjectPlacement objPlacement in objPlacements)
                {
                    FactoryItem aFI = null;
                    abf.CreateItem( ref aFI, objPlacement);
                    if (aFI != null)
                    {
                        aFI.GetComponent<PlacableFactoryItem>().FinalizePlacement();
                    }
                }

            }
        }


        // Accessors



    }
}