// Isaac Bustad
// 8/9/2024


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.Extentions
{
    public static class BugFreeTool
    {
        #region Velocity Limiter World Speed
        public const float worldSpeed = 25f;

        public static void LimitToWorldVelocity(this Vector3 tV3)
        {
            if (tV3.magnitude > worldSpeed)
            {
                // limit the velocity under word speed

                // store current Velocity


                // create new velocity

                tV3.x = Mathf.Clamp(tV3.x, 0, worldSpeed);
                tV3.y = Mathf.Clamp(tV3.y, 0, worldSpeed);
                tV3.z = Mathf.Clamp(tV3.z, 0, worldSpeed);
            }

        }

        public static void LimitVelocity(this Vector3 tV3, float aMag)
        {
            tV3.x = Mathf.Clamp(tV3.x, 0, aMag);
            tV3.y = Mathf.Clamp(tV3.y, 0, aMag);
            tV3.z = Mathf.Clamp(tV3.z, 0, aMag);
        }

        public static float CalcAngleViaSides(float oppSide, float oth1, float oth2)
        {
            float top = (oppSide * oppSide) - (oth1 * oth1) - (oth2 * oth2);
            float bottom = -2 * oth1 * oth2;
            float fract = top / bottom;

            return Mathf.Acos(fract) * Mathf.Rad2Deg;
        }
        // Accessors
        //public static float WorldSpeed { get { return worldSpeed; } }
        #endregion
    }
}
