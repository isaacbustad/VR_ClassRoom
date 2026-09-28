using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


namespace BugFreeProductions.VRClassroom
{
    
    public class XrCharacterContext : MonoBehaviour
    {
        #region Vars
        [Header("Hand Interactors")]
        [SerializeField] protected UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor leftHandGrabInteractor;
        [SerializeField] protected UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor rightHandGrabInteractor;

        [Header("Component to disable in states")]
        [SerializeField] protected XRInteractionManager xrInteractionManager;
        [SerializeField] protected GameObject itemPlacer;

        // states for context use
        protected XrCharacterState curCharacterState;
        protected XrCharacterState buildCharacterState;
        protected XrCharacterState editCharacterState;
        protected XrCharacterState interactCharacterState;
        protected XrCharacterState menuCharacterState;


        #endregion Vars

        #region Methods

        #region Unity Methods
        protected virtual void OnEnable()
        {
            // create states with access to this object
            buildCharacterState = new BuildXrCharacterState(this);
            editCharacterState = new EditXrCharacterState(this);
            interactCharacterState = new InteractXrCharacterState(this);
            menuCharacterState = new MenuXrCharacterState(this);

            // select entry state
            curCharacterState = interactCharacterState;
            curCharacterState.InteractMode(curCharacterState);
        }        
        #endregion Unity Methods

        #region Change State Methods
        public virtual void BuildMode()
        {
            curCharacterState.BuildMode(curCharacterState);
        }

        public virtual void EditMode()
        {
            curCharacterState.EditMode(curCharacterState);
        }

        public virtual void InteractMode()
        {
            curCharacterState.InteractMode(curCharacterState);
        }

        public virtual void MenuMode()
        {
            curCharacterState.MenuMode(curCharacterState);
        }

        #endregion Change State Methods

        
        #endregion Methods


        #region Accessors
        public XrCharacterState CurCharacterState
        {
            get
            {
                return curCharacterState;
            }

            set
            {
                curCharacterState = value;
            }
        }

        #region State Accessors
        public XrCharacterState BuildCharacterState
        {
            get
            {
                return buildCharacterState;
            }
        }

        public XrCharacterState EditCharacterState
        {
            get
            {
                return editCharacterState;
            }
        }

        public XrCharacterState InteractCharacterState
        {
            get
            {
                return interactCharacterState;
            }
        }

        #endregion State Accessors

        #region Component Accessors
        public XRInteractionManager XRInteractionManager
        {
            get
            {
                return xrInteractionManager;
            }
        }

        public GameObject ItemPlacer
        {
            get
            {
                return itemPlacer;
            }
        }
        #endregion Component Accessors

        #endregion Accessors
    }
}