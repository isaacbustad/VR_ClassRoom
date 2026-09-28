// Isaac Bustad
// 3/3/2025


using BugFreeProductions.VRClassroom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BugFreeProductions.VRClassroom {
    [RequireComponent(typeof(LineRenderer))]
    public class PlacableItemRemover : MonoBehaviour
    {
        // Vars
        protected LineRenderer lineRenderer =null;
        [SerializeField, Range(1, 100)] protected float maxRemoveDist = 5;
        protected bool isRemoving = false;
        [SerializeField] protected Transform posRotHelperTF = null;

        // temp selected item ref
        protected PlacableItemHighlighter placableItemHighlighter = null;


        // Methods
        protected virtual void OnEnable()
        {
            CollectVars();
        }
        protected virtual void FixedUpdate()
        {
            CastAndCheckforPlacement();
        }

        protected virtual void CollectVars()
        {
            // get the required line renderer
            lineRenderer = GetComponent<LineRenderer>();

            // create posRotHelper if none is assigned
            if (posRotHelperTF == null)
            {
                posRotHelperTF = new GameObject("PosRotHelper").transform;
            }
        }

        protected virtual void CastAndCheckforPlacement()
        {
            if (isRemoving == true)
            {
                // store raycast hit
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit, maxRemoveDist, 31, QueryTriggerInteraction.Ignore))
                {


                    posRotHelperTF.position = hit.point;
                    posRotHelperTF.rotation = transform.rotation;

                    DrawRemovalLine();

                    // get the placable Factory Item from an object if it exist
                    // else placableFactoryItem will be null
                    PlacableItemHighlighter buffItem = placableItemHighlighter;
                    placableItemHighlighter = hit.collider.gameObject.GetComponentInParent<PlacableItemHighlighter>();

                    // if the placable Factory Item exist
                    // run the method to highlight
                    if (placableItemHighlighter != null)
                    {
                        placableItemHighlighter.HighlighNegative();
                    }

                    if (buffItem != null && buffItem != placableItemHighlighter)
                    {
                        buffItem.DeHighlight();
                    }

                }
            }
        }

        public virtual void UseRemover(InputAction.CallbackContext aCon)
        {
            if (aCon.started)
            {
                isRemoving = true;
                lineRenderer.enabled = true;
            }
            else if (aCon.canceled)
            {
                RemoveObject();
                lineRenderer.enabled = false;
                isRemoving = false;    
            }
        }

        protected virtual void RemoveObject()
        {
            if (placableItemHighlighter != null)
            {
                placableItemHighlighter.GetComponent<PlacableFactoryItem>().RemoveItem();
            }
            
        }

        protected virtual void DrawRemovalLine()
        {
            

            // create array of line points
            Vector3[] posArray = new Vector3[] { transform.position, posRotHelperTF.position };

            lineRenderer.SetPositions(posArray);
        }


        // Accessors





    }
}