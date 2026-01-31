// Isaac Bustad
// 2/4/25


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.Tools
{
    public class PlacableFactoryItem : FactoryItem
    {
        #region Vars
        protected PlacableFactoryItemBody body;
        protected Rigidbody rb;
        // Components in body use get component to collect


        #endregion

        // Methods
        #region Setup and Finalize placement
        public virtual void OnEnable()
        {
           CollectVars();
        }

        protected virtual void CollectVars()
        {
            // get and default Rigidbody
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            // collect bodyScript
            body = GetComponent<PlacableFactoryItemBody>();
        }

        public virtual void FinalizePlacement()
        {
            body = GetComponent<PlacableFactoryItemBody>();
            if (body != null)
            {                
                body.FinalizeBody();
            }
        }
        #endregion


        #region Align Object to Point and Rotation
        

        public virtual void PositionAndRotateBody(Vector3 aGlobePos, Vector3 aLookPos,  Vector3 aAdditionalRotation)
        {
            transform.position = aGlobePos;
            body.PositionAndRotateBody(aGlobePos, aLookPos, aAdditionalRotation);
        }


        public override ObjectPlacement ObjectPlacement()
        {
            ObjectPlacement nObjPlace = new ObjectPlacement();
            if (body == null)
            {
                nObjPlace = base.ObjectPlacement();
            }
            else
            {
                

                nObjPlace.id = id;

                Vector3 nObjPos = body.BodyPosition;
                Debug.Log(gameObject.name + " my Saved transform = " + " " + nObjPos);

                nObjPlace.tpX = nObjPos.x;
                nObjPlace.tpY = nObjPos.y;
                nObjPlace.tpZ = nObjPos.z;

                Vector3 nObjRot = body.BodyRotation;
                Debug.Log(gameObject.name + " my Saved rotation = " + " " + nObjRot);

                nObjPlace.trX = nObjRot.x;
                nObjPlace.trY = nObjRot.y;
                nObjPlace.trZ = nObjRot.z;
            }
            return nObjPlace;
        }


        #endregion 

        public virtual void RemoveItem()
        {
            Destroy(gameObject);
        }
        // Accessors





    }
}