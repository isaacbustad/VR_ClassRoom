// Isaac Bustad
// 2/5/2025

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace BugFreeProductions.VRClassroom
{
    //[RequireComponent(typeof(Collider))]
    public class PlacableFactoryItemSafeArea : MonoBehaviour
    {
        // Vars
        //colors
        //[SerializeField] protected Color positiveColor = Color.white;
        //[SerializeField] protected Color negativeColor = Color.white;

        //[SerializeField] protected Material material = null;
        protected PlacableItemHighlighter placableItemHighlighter = null;

        [SerializeField] protected float yBuff = 0.001f;


        protected int objInTrig = 0;



        // Methods
        #region Setup
        protected virtual void OnEnable()
        {
            CollectVars();
        }

        protected virtual void CollectVars()
        {
            //material = GetComponent<Material>();
            placableItemHighlighter = GetComponent<PlacableItemHighlighter>();
            
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            objInTrig ++;
            Debug.Log("We finnish");
        }

        protected virtual void OnTriggerExit()
        {
            objInTrig --;
        }

        protected virtual void Update()
        {
            if (objInTrig > 0)
            {
                placableItemHighlighter.HighlighNegative();
            }
            else 
            {
                placableItemHighlighter.HighlighPositive();
            }
        }
        #endregion

        #region Align Object to Point and Rotation
        public virtual void PositionAndRotateBody(Vector3 aGlobePos, float aHeight, Quaternion aLookRot)
        {
            //PositionBody(aGlobePos, aHeight);
            //RotateBody(aLookRot);
        }

        /*protected virtual void PositionBody(Vector3 aGlobePos, float aHeight)
        {
            Vector3 nPos = aGlobePos;

            nPos.y += aHeight / 2;
            nPos.y += yBuff;
            transform.position = nPos;
        }*/

        protected virtual void RotateBody(Quaternion aLookRot)
        {
            transform.rotation = aLookRot;
        }


        #endregion

        #region Finalize Safe Area
        public virtual void FinalizeSafeArea()
        {
            
            //gameObject.GetComponent<Collider>().isTrigger = false;
            placableItemHighlighter.DeHighlight();

            enabled = false;

            //gameObject.SetActive(false);
        }

        #endregion

        // Accessors






    }
}