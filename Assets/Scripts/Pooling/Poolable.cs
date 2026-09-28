// Isaac Bustad
// 10/8/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.VRClassroom
{
    public abstract class Poolable : MonoBehaviour
    {
        // Vars
        protected GenericPool pool;

        // Methods
        protected virtual void PoolSelf()
        {
            if (pool != null )
            {
                pool.PoolObj(this);
            }
        }

        

        // Accessors
        public virtual GenericPool Pool { get { return pool; } set { pool = value; } }
        


    }
}