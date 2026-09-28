// Isaac Bustad
//4/3/2025

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.VRClassroom
{
    public class MannagedJSONReader
    {
        // Methods        
        

        // Spawns the list of objects from a list
        public virtual void SpawnObjects(string aFilePath)
        {
            if (JSONPlacementMannager.Instance.ABF_SCO != null)
            {
                // list of object placements
                List<ObjectPlacement> objPlacements = ObjectPlacementReadWrite.Instance.ReadObjectPlacements(/*"/" +*/ aFilePath);

                foreach (ObjectPlacement objPlacement in objPlacements)
                {
                    FactoryItem aFI = null;
                    JSONPlacementMannager.Instance.ABF_SCO.CreateItem(ref aFI, objPlacement);
                    if (aFI != null)
                    {                        
                        aFI.GetComponent<PlacableFactoryItem>().FinalizePlacement();
                    }
                }

            }
        }
    }
}
