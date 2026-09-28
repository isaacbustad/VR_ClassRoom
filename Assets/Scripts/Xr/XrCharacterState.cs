// Created By   :   Isaac Bustad
// Created      :   6/20/2026


using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



namespace BugFreeProductions.VRClassroom
{
    
    public class XrCharacterState
    {
        #region Vars
        protected XrCharacterState exitXrCS = null;
        protected XrCharacterContext xrcc = null;
        #endregion Vars


        #region Methods
        #region Change States Methods
        // Build
        public virtual void BuildMode(XrCharacterState axrcs)
        {
            // // set the past or exit state
            // exitXrCS = axrcs;

            // xrcc.CurCharacterState = xrcc.BuildCharacterState;
            // xrcc.XRInteractionManager.enabled = false;
            // xrcc.ItemPlacer.SetActive(false);
        }


        // Edit
        public virtual void EditMode(XrCharacterState axrcs)
        {
            // xrcc.CurCharacterState = xrcc.EditCharacterState;
            // xrcc.ItemPlacer.SetActive(true);
            // xrcc.XRInteractionManager.enabled = false;
        }


        // Interact
        public virtual void InteractMode(XrCharacterState axrcs)
        {
            // xrcc.CurCharacterState = xrcc.InteractCharacterState;
            // xrcc.XRInteractionManager.enabled = true;
            // xrcc.ItemPlacer.SetActive(false);
        }

        // Interact
        public virtual void MenuMode(XrCharacterState axrcs)
        {
            // xrcc.CurCharacterState = xrcc.InteractCharacterState;
            // xrcc.XRInteractionManager.enabled = true;
            // xrcc.ItemPlacer.SetActive(false);
        }
        
        #endregion Change States Methods

        public virtual void OnStateBegin(XrCharacterState axrcs)
        {
            exitXrCS = axrcs;
        }

        public virtual void OnStateEnd()
        {
            if (exitXrCS != null)
            {
                xrcc.CurCharacterState = exitXrCS;
            }
            xrcc.CurCharacterState = exitXrCS;
        }


        #endregion Methods


        

        
        // Build

        #region Constructors
        public XrCharacterState(XrCharacterContext aXRCC)
        {
            xrcc = aXRCC;
        }
        #endregion Constructors

        #region Accessors
        public bool IsBuildMode
        {
            get
            {
                return false;
            }
        }

        public bool IsEditMode
        {
            get
            {
                return false;
            }
        }

        public bool IsInteractMode
        {
            get
            {
                return false;
            }
        }

        #endregion Accessors
    }
}