// Isaac Bustad
// 3/6/2025

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BugFreeProductions.VRClassroom
{
    public class PlacableItemHighlighter : MonoBehaviour
    {
        // Vars
        [SerializeField] protected MeshRenderer meshRenderer = null;

        // Materials to change to for highlight effect
        // Material the item should be
        [SerializeField] protected Material primaryMat = null;
        // Material for a positive highlight
        [SerializeField] protected Material positiveHighlightMat = null;
        // Material for a negative highlight
        [SerializeField] protected Material negativeHighlightMat = null;


        // Methods
        public virtual void HighlighNegative()
        {
            if (meshRenderer != null && negativeHighlightMat != null)
            {
                meshRenderer.material = negativeHighlightMat;
            }
        }

        public virtual void HighlighPositive()
        {
            if (meshRenderer != null && positiveHighlightMat != null)
            {
                meshRenderer.material = positiveHighlightMat;
            }
        }

        public virtual void DeHighlight()
        {
            if (meshRenderer != null && primaryMat != null)
            {
                meshRenderer.material = primaryMat;
            }
        }

        


        // Accessors






    }
}