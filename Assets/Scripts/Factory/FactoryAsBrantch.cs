// Isaac Bustad
// 6/29/2025

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BugFreeProductions.Tools {

    [CreateAssetMenu(fileName = "FactoryAsBrantch", menuName = "ScriptableObject/FactoryAsBrantch")]
    public class FactoryAsBrantch : GenericFactory_SCO
    {
        #region Class Variables
        [SerializeField] protected GameObject[] factoryItems;


        #endregion


        #region Class Methods
        public override void CreateItem(ref FactoryItem aFI, ObjectPlacement aPlacement)
        {
            // check if item is in this factory's item list
            foreach (GameObject fItem in factoryItems)
            {
                if (fItem.GetComponent<FactoryItem>().ID == aPlacement.id)
                {
                    // create new item
                    GameObject nGO = Instantiate(fItem, Vector3.zero, Quaternion.identity);
                    FactoryItem fi = nGO.GetComponent<FactoryItem>();

                    if (fi != null)
                    {
                        fi.UseFactoryItem(aPlacement, pool);
                        aFI = fi;
                        if (fi.ID == JSONPlacementMannager.Instance.RoomID)
                        {
                            JSONPlacementMannager.Instance.Pool = pool;
                        }
                    }
                }
            }

            
        }
        #endregion
    }
}