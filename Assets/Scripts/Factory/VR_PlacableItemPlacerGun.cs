// Isaac Bustad
// 3/14/2025


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.VRClassroom
{
    public class VR_PlacableItemPlacerGun : MonoBehaviour
    {
        // Vars
        [SerializeField] protected string itemID = "NA";
        [SerializeField] protected AbstractFactory_SCO itemFactory = null;


        protected Transform posRotHelperTF = null;
        protected LineRenderer lineRenderer = null;
        [SerializeField, Range(1, 100)] protected float maxPlaceDist = 5;
        protected bool isPlacing = false;


        // temp vars for placement
        [SerializeField] protected FactoryItem factoryItem = null;
        protected PlacableFactoryItem placableFactoryItem = null;


        // for additional rotation
        // PlayerInputBridge playerInputBridge = null;
        protected Vector3 additionalRot = Vector3.zero;
        [SerializeField, Range(0, 90)] protected float rotSense = 1f; 





        // Methods
        protected virtual void OnEnable()
        {
            //JSONPlacementMannager
            CollectVars();
        }

        protected virtual void FixedUpdate()
        {
            if (isPlacing == true)
            {
                CastAndCheckforPlacement();
            }
            ClearAdditionalRot();


        }

        public virtual void SaveRoomConfig()
        {
            JSONPlacementMannager.Instance.WriteRoomConfig();
        }

        public virtual void AddAdditionalRot(float aRotDir)
        {
            float nRotDir = Mathf.Clamp(aRotDir,-1,1);
            additionalRot.y += aRotDir * Time.deltaTime * rotSense;
        }
        public virtual void ClearAdditionalRot()
        {
            additionalRot.y = 0;
        }

        protected virtual void CollectVars()
        {
            // make sure we have a helper

            posRotHelperTF = new GameObject("posRotHelper").transform;

            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
            lineRenderer.enabled = false;
        }

        // input converted to bool callback to allow mapper mapping
        public virtual void UsePlacer(bool aCon)
        {
            Debug.Log("Use the placer");
            if (gameObject.activeSelf == true)
            {
                if (aCon == true)
                {
                    lineRenderer.enabled = true;
                    isPlacing = true;


                }
                else if (aCon == false)
                {
                    Debug.Log("pre place");
                    PlaceItem();
                    Debug.Log("post place");
                    //PlaceItem();
                    isPlacing = false;
                    lineRenderer.enabled = false;


                    // assigned null for re use
                    factoryItem = null;
                    placableFactoryItem = null;
                }
            }


        }


        // functionality to use placer
        protected virtual void PlaceItem()
        {
            if (placableFactoryItem != null)
            {
                placableFactoryItem.FinalizePlacement();
            }
        }

        // Set up placement data via custom calculation
        protected virtual ObjectPlacement CalcObjectPlacementData()
        {
            // declair returning var
            ObjectPlacement nPlacement = new ObjectPlacement();


            // Set ID
            nPlacement.id = itemID;

            // set transfom information
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
                    Debug.Log("Item created");
                    itemFactory.CreateItem(ref factoryItem, CalcObjectPlacementData());
                    placableFactoryItem = factoryItem.GetComponent<PlacableFactoryItem>();
                }


                // change item position
                //factoryItem.transform.position = posRotHelperTF.position;

                // change body position

                placableFactoryItem.PositionAndRotateBody(posRotHelperTF.position, transform.position, additionalRot/*, playerInputBridge.AdditionalRotation*/);
            }

        }

        // Draw the line of the placement ray
        protected virtual void DrawPlacementLine()
        {
            //lineRenderer.enabled = true;

            // create array of line points
            Vector3[] posArray = new Vector3[] { transform.position, posRotHelperTF.position };

            lineRenderer.SetPositions(posArray);
        }

        // Accessors
        public string ItemID { get { return itemID; } set { itemID = value; } }

        // public PlayerInputBridge PlayerInputBridge { get { return playerInputBridge; } set { playerInputBridge = value; } }


    }
}