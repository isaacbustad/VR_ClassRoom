// Isaac Bustad
// 10/8/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BugFreeProductions.Tools
{
    [CreateAssetMenu(fileName = "GenericFactory_SCO", menuName = "ScriptableObject/GenericFactory_SCO")]
    public class GenericFactory_SCO : ScriptableObject
    {

        // Vars
        // item we want to mas produce
        [SerializeField] protected GameObject factoryItem;
        // pool to store produced items
        protected GenericPool pool = new GenericPool();


        // Methods
        #region Create Factory Items
        public virtual void CreateItem(Transform createInfo)
        {
            // create ref for pool to return to
            Poolable aFact = null;

            // check pool for reusable item
            pool.PoolChk(ref aFact);

            // use new only if none were pooled
            if (aFact != null)
            {
                // reuse pooled item
                FactoryItem fi = aFact.GetComponent<FactoryItem>();
                if (fi != null)
                {
                    fi.UseFactoryItem(createInfo, pool);
                }
                
            }
            else
            {
                // create new item
                GameObject nGO = Instantiate(factoryItem, createInfo.position, createInfo.rotation);
                FactoryItem fi = nGO.GetComponent<FactoryItem>();

                if(fi != null)
                {
                    fi.UseFactoryItem(createInfo, pool);
                }
            }

        }

        public virtual void CreateItem(ObjectPlacement aPlacement)
        {            
            // create new item
            GameObject nGO = Instantiate(factoryItem, Vector3.zero, Quaternion.identity);
            FactoryItem fi = nGO.GetComponent<FactoryItem>();

            if (fi != null)
            {
                fi.UseFactoryItem(aPlacement, pool);
            }
        }

        public virtual void CreateItem(ref FactoryItem aFI, ObjectPlacement aPlacement)
        {
            // check if item is in this factory's item list
            if (ItemID == aPlacement.id)
            {
                // create new item
                GameObject nGO = Instantiate(factoryItem, Vector3.zero, Quaternion.identity);
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

           /* // create new item
            GameObject nGO = Instantiate(factoryItem, Vector3.zero, Quaternion.identity);
            FactoryItem fi = nGO.GetComponent<FactoryItem>();

            if (fi != null)
            {
                fi.UseFactoryItem(aPlacement, pool);
                aFI = fi;
                if (fi.ID == JSONPlacementMannager.Instance.RoomID)
                {
                    JSONPlacementMannager.Instance.Pool = pool;
                }
            }*/
        }

        #endregion

        #region Gather Factory Item Position Save Info

        public virtual List<ObjectPlacement> GatherFactItemPosInfo()
        {
            List<ObjectPlacement> retLst = new List<ObjectPlacement>();

            List<Poolable> pooledObjs = pool.PoolList;

            foreach (Poolable obj in pooledObjs)
            {
                retLst.Add(obj.GetComponent<FactoryItem>().ObjectPlacement());
            }

            return retLst;
        }

        #endregion

        // Accessors
        public virtual string ItemID { get { return factoryItem.GetComponent<FactoryItem>().ID; } }

        public virtual GenericPool Pool { get { return pool; } }


    }
}