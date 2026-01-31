// Isaac Bustad
// 10/8/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.Tools
{
    public class FactoryItem : Poolable
    {
        // Vars
        // id standing for particular prefab
        [SerializeField] protected string id = "NA";

        protected int instanceID = -1;



        // Methods
        public virtual void UseFactoryItem(Transform aTF, GenericPool aGP)
        {
            gameObject.SetActive(true);
            pool = aGP;
            // position and align
            transform.position = aTF.position;
            transform.rotation = aTF.rotation;
        }

        #region Stuff For Object Placement
        public virtual void UseFactoryItem(ObjectPlacement aPlacement, GenericPool aGP)
        {
            gameObject.SetActive(true);
            pool = aGP;
            // position and align
            transform.position = new Vector3(aPlacement.tpX, aPlacement.tpY, aPlacement.tpZ);
            transform.rotation = Quaternion.Euler(aPlacement.trX, aPlacement.trY, aPlacement.trZ);
            
            // set the instance ID based on the placement
            //instanceID = aPlacement.instanceID;

            // pool objects created with placement data
            pool.PoolObj(this);
        }
        #endregion

        protected virtual void OnDestroy()
        {
            pool.RevFromPool(this);
        }

        // Create a ObjectPlacement that represents the Item and current state 
        public virtual ObjectPlacement ObjectPlacement() 
        {
            ObjectPlacement nObjPlace = new ObjectPlacement();

            // set item category
            nObjPlace.id = id;

            Vector3 nObjPos = transform.position; 

            // set position
            nObjPlace.tpX = nObjPos.x;
            nObjPlace.tpY = nObjPos.y;
            nObjPlace.tpZ = nObjPos.z;

            Vector3 nObjRot = transform.eulerAngles;

            // set rotation
            nObjPlace.trX = nObjRot.x;
            nObjPlace.trY = nObjRot.y;
            nObjPlace.trZ = nObjRot.z;
            
            // set unique identifier
            //nObjPlace.instanceID = instanceID;

            return nObjPlace;
        }

        // Accessors
        public virtual string ID { get { return id; } }
        
        public virtual int InstanceID { get { return instanceID; }}


    }
}