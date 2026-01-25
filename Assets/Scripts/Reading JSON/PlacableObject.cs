// Isaac Bustad
// 11/6/2025


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.Tools
{
    public class PlacableObject : FactoryItem
    {
        // id standing for particular prefab
        //[SerializeField] protected string id = "NA";

        // where to place object
        protected ObjectPlacement placement;

        // represent transform position and rotation in Vector3s
        protected Vector3 targPose = Vector3.zero;
        protected Vector3 targRot = Vector3.zero;


        // Start is called before the first frame update
        

        protected virtual void OnEnable()
        {
            //placement = ObjectPlacementReadWrite.Instance.FindObjectPlacement(id);
            /*ReadPose();
            ReadRot();
            AssignPos();
            AssignRot();*/
        }

        // drpricated due to optimization as of 4/3/2025

        /*#region Read from OBJ Placement
        protected virtual void ReadPose()
        {
            if (placement != null)
            {
                targPose.x = placement.tpX;
                targPose.y = placement.tpY;
                targPose.z = placement.tpZ;
            }
            else
            {
                Debug.Log("No placement found");
            }
            
        }

        protected virtual void ReadRot()
        {
            if (placement != null)
            {
                targRot.x = placement.trX;
                targRot.y = placement.trY;
                targRot.z = placement.trZ;
            }
            else
            {
                Debug.Log("No placement found");
            }
        }
        #endregion

        #region Write to OBJ Placement
        #endregion

        #region Assign Rot and Pos
        protected virtual void AssignRot()
        {
            transform.rotation = Quaternion.Euler(targRot);
        }

        protected virtual void AssignPos()
        {
            transform.position = targPose;
        }
        #endregion*/

        // Accessors
        //public virtual string ID { get { return id; } }



    }
}