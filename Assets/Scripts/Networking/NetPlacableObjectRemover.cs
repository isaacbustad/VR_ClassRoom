// Created By   :   Isaac Bustad
// Created      :   6/11/2026


using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace BugFreeProductions.VRClassroom
{
    public class NetPlacableObjectRemover : VR_PlacableItemRemoverGun
    {
        #region Vars
        [SerializeField] protected NetworkIdentity ni = null;

        
        protected NetPlacableItemSpawner npis = null;
        #endregion Vars


        #region Methods
        protected override void OnEnable()
        {
            base.OnEnable();
            // CollectVars();
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            // CastAndCheckforPlacement();
        }

        protected override void CollectVars()
        {
            ni = GetComponentInParent<NetworkIdentity>();
            npis = GetComponent<NetPlacableItemSpawner>();


            base.CollectVars();
            // // get the required line renderer
            // lineRenderer = GetComponent<LineRenderer>();

            // // create posRotHelper if none is assigned
            // if (posRotHelperTF == null)
            // {
            //     posRotHelperTF = new GameObject("PosRotHelper").transform;
            // }
        }

        protected override void CastAndCheckforPlacement()
        {
            base.CastAndCheckforPlacement();
            // if (isRemoving == true)
            // {
            //     // store raycast hit
            //     RaycastHit hit;

            //     if (Physics.Raycast(transform.position, transform.forward, out hit, maxRemoveDist, 31, QueryTriggerInteraction.Ignore))
            //     {


            //         posRotHelperTF.position = hit.point;
            //         posRotHelperTF.rotation = transform.rotation;

            //         DrawRemovalLine();

            //         // get the placable Factory Item from an object if it exist
            //         // else placableFactoryItem will be null
            //         PlacableItemHighlighter buffItem = placableItemHighlighter;
            //         placableItemHighlighter = hit.collider.gameObject.GetComponentInParent<PlacableItemHighlighter>();

            //         // if the placable Factory Item exist
            //         // run the method to highlight
            //         if (placableItemHighlighter != null)
            //         {
            //             placableItemHighlighter.HighlighNegative();
            //         }

            //         if (buffItem != null && buffItem != placableItemHighlighter)
            //         {
            //             buffItem.DeHighlight();
            //         }

            //     }
            // }
        }

        public override void UseRemover(bool aCon)
        {
            if (ni.isServer || NetGuestPermissionManager.GuestCanEdit)
            {
                base.UseRemover(aCon);
            }
            // Debug.Log("Call Use remover");
            // if(gameObject.activeSelf == true)
            // {
            //     if (aCon == true)
            //     {
            //         isRemoving = true;
            //         lineRenderer.enabled = true;
            //     }
            //     else if (aCon == false)
            //     {
            //         RemoveObject();
            //         lineRenderer.enabled = false;
            //         isRemoving = false;
            //     }
            // }

        }

        protected override void RemoveObject()
        {
            if (ni.isServer && placableItemHighlighter != null)
            {
                
                placableItemHighlighter.GetComponent<PlacableFactoryItem>().RemoveItem();

                return;
            }

            if (npis.isClient && NetGuestPermissionManager.GuestCanEdit && placableItemHighlighter != null)
            {
                npis.RequestNetworkItemDeSpawn(placableItemHighlighter.GetComponent<NetPlacableItemSpawnable>().netId);
                return;
            }
            
            // if (placableItemHighlighter != null)
            // {
            //     placableItemHighlighter.GetComponent<PlacableFactoryItem>().RemoveItem();
            // }

        }

        


        

        protected override void DrawRemovalLine()
        {
            base.DrawRemovalLine();

            // // create array of line points
            // Vector3[] posArray = new Vector3[] { transform.position, posRotHelperTF.position };

            // lineRenderer.SetPositions(posArray);
        }

        #endregion Methods
    }
}