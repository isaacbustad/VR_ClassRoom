// Isaac Bustad 
// 10/23/2024


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


namespace BugFreeProductions.VRClassroom
{
    public class CameraViewportManager : MonoBehaviour
    {
        // Vars
        // cams for cam adjustment
        protected List<Camera> playerCams = new List<Camera>();

        // toggle by X or Y
        [SerializeField] protected bool stack = true;


        // Methods
        public virtual void ChangeCamViewport(Camera aCam, bool addPlayer)
        {
            if (aCam != null)
            {
                if (addPlayer)
                {
                    playerCams.Add(aCam);
                }
                else
                {
                    playerCams.Remove(aCam);
                }

                DividePositionCams();
            }
        }
        

        protected virtual void DividePositionCams()
        {
            // get step increment x and y
            float stepX = 1;
            float stepY = 1;

            // if displaying more than one camera
            if (playerCams.Count > 1) 
            {
                // calculate the x step based on if they are stacked or not
                if (stack)
                {
                    stepX = Mathf.Floor(Mathf.Sqrt(playerCams.Count));
                }
                else
                {
                    stepX = Mathf.Ceil(Mathf.Sqrt(100));
                }
                stepY = Mathf.Ceil(playerCams.Count / stepX);
                //Debug.Log(stepY);

                // convert steps to rates
                float rateX = 1;
                float rateY = 1;
                if (stepY > 0)
                {
                    rateY = 1 / stepY;
                }
                if (stepX > 0)
                {
                    rateX = 1 / stepX;
                }
                


                // x y position then with height
                // all values are normalized 0-1
                float curStepX = 0;
                float curStepY = 1 - rateY;
                foreach (Camera cam in playerCams)
                {
                    cam.rect = new Rect(curStepX, curStepY, rateX, rateY);

                    // increment and decrement
                    curStepY = curStepY - rateY;
                    if (curStepY < 0)
                    {
                        curStepY = 1 - rateY;
                        curStepX += rateX;
                    }
                }

            }
            
            

            

        }
        // Accessors




    }
}