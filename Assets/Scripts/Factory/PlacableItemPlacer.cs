// Isaac Bustad
// 2/4/2025

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BugFreeProductions.VRClassroom
{
    [RequireComponent(typeof(LineRenderer))]
    public class PlacableItemPlacer : FactoryItemPlacer
    {
        // Vars
        protected Transform posRotHelperTF = null;
        protected LineRenderer lineRenderer = null;
        [SerializeField, Range(1, 100)] protected float maxPlaceDist = 5;
        protected bool isPlacing = false;


        // temp vars for placement
        protected FactoryItem factoryItem = null;
        protected PlacableFactoryItem placableFactoryItem = null;

        // for additional rotation
        protected PlayerInputBridge playerInputBridge = null;

        

        // Methods
        protected virtual void OnEnable()
        {
            CollectVars();
        }

        protected virtual void FixedUpdate()
        {
            if (isPlacing == true)
            {
                CastAndCheckforPlacement();
            }
        }

        protected virtual void CollectVars()
        {
            // make sure we have a helper
            
            posRotHelperTF = new GameObject("posRotHelper").transform;
            
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
            lineRenderer.enabled = false;
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
        }

        // input testing
        public override void UsePlacer(InputAction.CallbackContext aCon)
        {
            if (aCon.started == true)
            {
                lineRenderer.enabled = true;
                isPlacing = true;


            }
            else if (aCon.canceled == true)
            {
                PlaceItem();
                //PlaceItem();
                isPlacing = false;
                lineRenderer.enabled = false;


                // assigned null for re use
                factoryItem = null;
                placableFactoryItem = null;
            }

        }


        // functionality to use placer
        protected override void PlaceItem()
        {
            if ( placableFactoryItem != null)
            {
                placableFactoryItem.FinalizePlacement();
            }
        }

        // Set up placement data via custom calculation
        protected virtual ObjectPlacement CalcObjectPlacementData()
        {
            // declare returning var
            ObjectPlacement nPlacement = new ObjectPlacement();


            // Set ID
            nPlacement.id = itemID;

            // set transform information
            Vector3 aPos = posRotHelperTF.position;

            nPlacement.tpX = aPos.x;
            nPlacement.tpY = aPos.y;
            nPlacement.tpZ = aPos.z;


            // set rotation Data
            Vector3 aRot = transform.eulerAngles;

            nPlacement.trX = aRot.x;
            nPlacement.trY = aRot.y;
            nPlacement.trZ = aRot.z;

            // return calculated placement data
            return nPlacement;
        }


        // Use Raycast and other checks to find where to place object
        protected virtual void CastAndCheckforPlacement()
        {
            // store raycast hit
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, maxPlaceDist, 31, QueryTriggerInteraction.Ignore))
            {


                posRotHelperTF.position = hit.point;
                posRotHelperTF.rotation = transform.rotation;

                DrawPlacementLine();

                // if we have not created an object to place create here
                // validates that we are pointing at a valid position
                if (factoryItem == null)
                {
                    itemFactory.CreateItem(ref factoryItem, CalcObjectPlacementData());
                    placableFactoryItem = factoryItem.GetComponent<PlacableFactoryItem>();
                }


                // change body position

                placableFactoryItem.PositionAndRotateBody(posRotHelperTF.position,transform.position,playerInputBridge.AdditionalRotation);
            }

        }

        // Draw the line of the placement ray
        protected virtual void DrawPlacementLine()
        {
            //lineRenderer.enabled = true;

            // create array of line points
            Vector3[] posArray = new Vector3[] {transform.position,posRotHelperTF.position};

            lineRenderer.SetPositions(posArray);
        }

        // Accessors
        public string ItemID { get { return itemID; } set { itemID = value; } }

        public PlayerInputBridge PlayerInputBridge { get { return playerInputBridge; } set { playerInputBridge = value; } }

    }
}