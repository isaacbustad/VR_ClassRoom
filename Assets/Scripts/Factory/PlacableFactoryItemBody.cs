// Isaac Bustad
// 2/4/2025

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//using UnityEngine.InputSystem;

namespace BugFreeProductions.VRClassroom
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlacableFactoryItemBody : MonoBehaviour
    {
        // Vars
        // List of body colliders that will be changed from trigger
        [SerializeField, Tooltip("Assign coliders in inspector to be enabled for placement finalization")] 
        protected List<Collider> bodColliders = new List<Collider>();
        
        protected Rigidbody rb = null;


        // safe area trigger
        //[SerializeField] protected GameObject safeAreaGO = null;
        protected PlacableFactoryItemSafeArea safeArea = null;

        [SerializeField] protected Transform bodyObject = null;
        protected Transform rotHelper = null;

        [SerializeField, Range(0.1f,20)] protected float width = 1;
        [SerializeField, Range(0.1f, 20)] protected float height = 1;


        // vars for additional rotation
        protected Vector3 bodyAdditionalRotation = Vector3.zero;

        // finialize fix
        protected bool nPlaced = false; 

        // Methods
        #region Default and Finalize

        protected virtual void OnEnable()
        {
            CollectVars();
            DefaultVars();
        }

        protected virtual void CollectVars()
        {
            rotHelper = new GameObject().transform;
            rb = GetComponent<Rigidbody>();
            safeArea = GetComponent<PlacableFactoryItemSafeArea>();
        }

        protected virtual void DefaultVars()
        {
            // freeze rb
            //rb.freezeRotation = true;

            // set all coliders to trigger to avoid colissions
            if (bodColliders.Count > 0)
            {
                foreach (Collider col in bodColliders)
                {
                    col.isTrigger = true;
                }
            }

            // ensure sfe area trigger is enabled
            //safeAreaGO.SetActive(true);

        }

        // final setup for all body components
        public virtual void FinalizeBody()
        {
            // activate rb interections
            rb.freezeRotation = false;
            rb.useGravity = true;

            //PositionAndRotateBody();
            FinalizeBodyColiders();
            FinalizeSafeArea();
        }

        // finalize item for replay playback - similar structure to FinalizeBody but usable for replayed items
        public virtual void FinalizeForReplay()
        {
            if (rb == null)
            {
                CollectVars();
            }

            rb.freezeRotation = false;
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.detectCollisions = false;

            DisableReplayColliders();
            FinalizeSafeArea();
        }

        protected virtual void DisableReplayColliders()
        {
            Collider[] allColliders = GetComponentsInChildren<Collider>(true);
            foreach (Collider col in allColliders)
            {
                if (col != null)
                {
                    col.enabled = false;
                }
            }
        }

        // final setup for body coliders
        protected virtual void FinalizeBodyColiders()
        {
            if (bodColliders.Count > 0)
            {
                foreach (Collider col in bodColliders)
                {
                    col.isTrigger = false;
                }
            }
        }

        // finalize / remove safe area trigger
        protected virtual void FinalizeSafeArea()
        {
            /*if (safeArea == null)
            {
                safeArea = GetComponent<PlacableFactoryItemSafeArea>();
            }*/
            safeArea.FinalizeSafeArea();
        }
        #endregion

        public virtual void PositionAndRotateBody()
        {
            if (nPlaced != true)
            {
                PositionBody(transform.position);
            }
            
            //RotateBody(aLookPos);
            safeArea.PositionAndRotateBody(transform.position, height, CalcNewRot(transform.rotation.eulerAngles));
        }

        #region Align Object to Point and Rotation
        /*public virtual void PositionAndRotateBody(Vector3 aGlobePos, Vector3 aLookPos)
        {
            PositionBody(aGlobePos);
            RotateBody(aLookPos);
            safeArea.PositionAndRotateBody(aGlobePos, height, CalcNewRot(aLookPos));
        }*/

        public virtual void PositionAndRotateBody(Vector3 aGlobePos, Vector3 aLookPos, Vector3 aAdditionalRotation)
        {
            bodyAdditionalRotation += aAdditionalRotation;
            PositionBody(aGlobePos);
            RotateBody(aLookPos);
            safeArea.PositionAndRotateBody(aGlobePos, height, CalcNewRot(aLookPos));
        }

        protected virtual void PositionBody(Vector3 aGlobePos)
        {
            Vector3 nPos = aGlobePos;

            nPos.y += 0.01f;
            bodyObject.position = nPos;
        }

        protected virtual void RotateBody(Vector3 aLookPos)
        {
            

            bodyObject.transform.rotation = CalcNewRot(aLookPos);

        }

        protected virtual Quaternion CalcNewRot(Vector3 aLookPos)
        {
            // find target rotation
            rotHelper.LookAt(aLookPos);

            // create new rot
            Vector3 nRot = rotHelper.eulerAngles;
            nRot += bodyAdditionalRotation;

            // adjust rot
            nRot.z = 0;
            nRot.x = 0;

            Quaternion nQ = Quaternion.Euler(nRot);
            return nQ;
        }


        #endregion




        // Accessors        
        public Vector3 BodyAdditionalRotation { get { return bodyAdditionalRotation; } set { bodyAdditionalRotation = value; } }

        public Vector3 BodyRotation { get { return bodyObject.rotation.eulerAngles; } }
        public Vector3 BodyPosition { get { return bodyObject.position; } }



    }
}