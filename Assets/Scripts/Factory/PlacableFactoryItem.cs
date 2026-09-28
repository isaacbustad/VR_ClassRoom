// Isaac Bustad
// 2/4/25


using System.Collections;
using System.Collections.Generic;
using Meta.XR.ImmersiveDebugger.UserInterface;
using Mirror.BouncyCastle.Asn1.Cmp;
using UnityEngine;


namespace BugFreeProductions.VRClassroom
{
    public class PlacableFactoryItem : FactoryItem, Subscriber
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
           Setup();
        }

        protected virtual void Setup()
        {
            Subscribe();
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
        #endregion Setup and Finalize Placement

        #region Add Subscription service
        // method to recieve update from subscrition
        public void OnNotify()
        {

        }

        // method to subscribe to SubscriptionService
        public void Subscribe()
        {
            JSONPlacementMannager.Instance.AddSubscriber(this);
            Debug.Log(gameObject.name + " Subscribed");
        }

        // method to unsubscribe from SubscriptionService
        public void Unsubscribe()
        {
            JSONPlacementMannager.Instance.RemoveSubscriber(this);
            Debug.Log(gameObject.name + " UnSubscribed");
        }
        #endregion Add Subscription service


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
                //Debug.Log(gameObject.name + " my Saved transform = " + " " + nObjPos);

                nObjPlace.tpX = nObjPos.x;
                nObjPlace.tpY = nObjPos.y;
                nObjPlace.tpZ = nObjPos.z;

                Vector3 nObjRot = body.BodyRotation;
                //Debug.Log(gameObject.name + " my Saved rotation = " + " " + nObjRot);

                nObjPlace.trX = nObjRot.x;
                nObjPlace.trY = nObjRot.y;
                nObjPlace.trZ = nObjRot.z;
            }
            return nObjPlace;
        }


        #endregion 

        public virtual void RemoveItem()
        {
            Unsubscribe();
            Destroy(gameObject);
        }
        // Accessors





    }
}