// Isaac Bustad
// 10/8/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace BugFreeProductions.VRClassroom
{
    public class GenericPool
    {
        // Vars
        protected Queue<Poolable> poolables = new Queue<Poolable>();


        // Methods
        public virtual void PoolChk(ref Poolable aPoolable)
        {
            if (poolables.Count > 0)
            {
                aPoolable = poolables.Dequeue();
            }
        }

        // add object to pool
        public virtual void PoolObj(Poolable aPoolable)
        {
            poolables.Enqueue(aPoolable);
        }

        public virtual void RevFromPool(Poolable aPoolable)
        {
            List<Poolable> nPool = poolables.ToList();

            nPool.Remove(aPoolable);

            RefillQueue(nPool);
        }

        protected virtual void RefillQueue(List<Poolable> aList)
        {
            // clear Queue
            poolables.Clear();

            // refill Queue
            foreach (Poolable poolable in aList)
            {
                poolables.Enqueue(poolable);
            }
        }

        public virtual void ReorderQueue(int index)
        {
            if (poolables.Count <= 1)
            {
                return;
            }

            List<Poolable> poolList = poolables.ToList();
            Poolable lastItem = poolList[poolList.Count - 1];

            poolList.RemoveAt(poolList.Count - 1);
            poolList.Insert(index, lastItem);

            RefillQueue(poolList);
        }

        // Accessors
        public virtual List<Poolable> PoolList { get { return poolables.ToList(); } }

    }
}