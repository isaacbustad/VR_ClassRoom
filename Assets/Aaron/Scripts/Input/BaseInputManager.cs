using UnityEngine;

/// <summary>
/// Base abstract class that provides common functionality for input map management.
/// Implements a singleton pattern and handles mode switching for different input contexts.
/// </summary>
public abstract class BaseInputManager : MonoBehaviour
{
    /// <summary>Static reference to the active instance</summary>
    protected static BaseInputManager instance = null;

    /// <summary>Current active input mode</summary>
    protected InputMode currentMode = InputMode.Default;

    /// <summary>Previously active input mode, used when temporarily switching modes</summary>
    protected InputMode previousMode = InputMode.Default;

    protected virtual void OnEnable()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning($"Multiple {GetType().Name} instances detected. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeComponents();
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    /// <summary>
    /// Defines the possible input modes for the application
    /// </summary>
    public enum InputMode
    {
        /// <summary>Standard navigation/interaction mode</summary>
        Default,

        /// <summary>Mode for creating and editing room geometry</summary>
        RoomCreation,

        /// <summary>Mode for placing objects in the environment</summary>
        ItemPlacement,

        /// <summary>Mode for interacting with the item catalog menu</summary>
        CatalogMenu
    }

    /// <summary>
    /// Switches to the specified input mode
    /// </summary>
    /// /// <param name="newMode">The mode to switch to</param>
    /// <param name="isTemporary">Whether this is a temporary mode change (e.g. for UI)</param>
    public abstract void SwitchToMode(InputMode newMode, bool isTemporary = false);

    /// <summary>
    /// Applies the effects of switching to a mode (enabling/disabling components, etc.)
    /// </summary>
    /// <param name="mode">The mode whose effects should be applied</param>
    protected abstract void ApplyModeEffects(InputMode mode);

    /// <summary>
    /// Common logic for switching between input modes
    /// </summary>
    /// <param name="newMode">The mode to switch to</param>
    /// <param name="isTemporary">Whether this is a temporary mode change</param>
    protected void HandleModeSwitch(InputMode newMode, bool isTemporary = false)
    {
        if (!isTemporary)
        {
            previousMode = currentMode;
        }

        currentMode = newMode;
        ApplyModeEffects(newMode);
    }

    /// <summary>
    /// Called when opening the radial menu UI
    /// </summary>
    public virtual void OnOpenRadialMenuUI()
    {
        SwitchToMode(InputMode.Default, true);
    }

    /// <summary>
    /// Called when closing any menu UI
    /// </summary>
    public virtual void OnCloseMenuUI()
    {
        SwitchToMode(previousMode);
    }

    /// <summary>
    /// Initialize any required components - override in derived classes
    /// </summary>
    protected virtual void InitializeComponents()
    {
        // Override this method in derived classes to initialize specific components
    }

    /// <summary>
    /// Safely enable or disable a component with null checking
    /// </summary>
    /// <typeparam name="T">Type of component to modify</typeparam>
    /// <param name="component">The component reference</param>
    /// <param name="enabled">Whether the component should be enabled</param>
    protected void SafeSetComponentState<T>(T component, bool enabled) where T : Behaviour
    {
        if (component != null)
        {
            component.enabled = enabled;
            Debug.Log($"{component.GetType().Name} has been {(enabled ? "enabled" : "disabled")}.");
        }
    }

    /// <summary>
    /// Safely activate or deactivate a GameObject with null checking
    /// </summary>
    /// <param name="obj">The GameObject reference</param>
    /// <param name="active">Whether the GameObject should be active</param>
    protected void SafeSetGameObjectState(GameObject obj, bool active)
    {
        if (obj != null)
        {
            obj.SetActive(active);
        }
    }

    /// <summary>
    /// Show floor points in the room generator
    /// </summary>
    /// <param name="roomGen">The room generator reference</param>
    protected void ShowFloorPoints(RoomGenerator roomGen)
    {
        if (roomGen != null)
        {
            roomGen.ShowFloorPoints();
        }
    }

    /// <summary>
    /// Hide floor points in the room generator
    /// </summary>
    /// <param name="roomGen">The room generator reference</param>
    protected void HideFloorPoints(RoomGenerator roomGen)
    {
        if (roomGen != null)
        {
            roomGen.HideFloorPoints();
        }
    }

    /// <summary>
    /// Check if a specific input mode is currently active
    /// </summary>
    /// <param name="mode">The mode to check</param>
    /// <returns>True if the specified mode is active</returns>
    public bool IsInMode(InputMode mode)
    {
        return currentMode == mode;
    }
}
