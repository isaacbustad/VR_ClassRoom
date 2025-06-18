// Written by Aaron Williams
using UnityEngine;

/// <summary>
/// Manages input modes specifically for VR gameplay.
/// Controls which VR interaction components are active and manages their state.
/// Extends BaseInputManager to share common functionality with the non-VR input manager.
/// </summary>
public class VRInputMapManager : BaseInputManager
{
    [Header("VR Component References")]
    [SerializeField, Tooltip("Reference to the room generator GameObject")]
    private GameObject roomGenerator;

    [SerializeField, Tooltip("Reference to the VR item placement gun")]
    private GameObject placableItemGun;

    private RoomGenerator roomGeneratorComponent;

    public static VRInputMapManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject managerObject = new GameObject("VRInputMapManager");
                instance = managerObject.AddComponent<VRInputMapManager>();
                DontDestroyOnLoad(managerObject);
            }
            return instance as VRInputMapManager;
        }
    }

    /// <summary>
    /// Initializes references to required components
    /// </summary>
    protected override void InitializeComponents()
    {
        base.InitializeComponents();

        if (roomGenerator != null)
        {
            roomGeneratorComponent = roomGenerator.GetComponent<RoomGenerator>();
        }

        if (placableItemGun == null)
        {
            Debug.LogWarning("Placable item gun reference is missing. VR item placement will not work.");
        }
    }

    /// <summary>
    /// Switches to the specified input mode
    /// </summary>
    /// <param name="newMode">The mode to switch to</param>
    /// <param name="isTemporary">Whether this is a temporary mode change</param>
    public override void SwitchToMode(InputMode newMode, bool isTemporary = false)
    {
        HandleModeSwitch(newMode, isTemporary);
    }

    /// <summary>
    /// Apply component state changes for the specified mode
    /// </summary>
    /// <param name="mode">The mode to apply</param>
    protected override void ApplyModeEffects(InputMode mode)
    {
        currentMode = mode;

        switch (mode)
        {
            case InputMode.Default:
                SafeSetGameObjectState(placableItemGun, false);
                SafeSetGameObjectState(roomGenerator, false);
                HideFloorPoints(roomGeneratorComponent);
                break;

            case InputMode.RoomCreation:
                SafeSetGameObjectState(placableItemGun, false);
                SafeSetGameObjectState(roomGenerator, true);
                ShowFloorPoints(roomGeneratorComponent);
                break;

            case InputMode.ItemPlacement:
                SafeSetGameObjectState(placableItemGun, true);
                SafeSetGameObjectState(roomGenerator, false);
                HideFloorPoints(roomGeneratorComponent);
                break;

            case InputMode.CatalogMenu:
                SafeSetGameObjectState(placableItemGun, false);
                SafeSetGameObjectState(roomGenerator, false);
                HideFloorPoints(roomGeneratorComponent);
                break;
        }
    }

    /// <summary>
    /// Called when the radial menu is opened
    /// </summary>
    public override void OnOpenRadialMenuUI()
    {
        // Only switch to default mode if we're not already in it
        if (currentMode != InputMode.Default)
        {
            SwitchToMode(InputMode.Default, true);
        }
    }

    /// <summary>
    /// Called when a menu is closed
    /// </summary>
    public override void OnCloseMenuUI()
    {
        // Return to the previous mode when closing a menu
        switch (previousMode)
        {
            case InputMode.RoomCreation:
                SwitchToRoomCreationMode();
                break;
            case InputMode.ItemPlacement:
                SwitchToItemPlacementMode();
                break;
            default:
                SwitchToDefaultMode(false);
                break;
        }
    }

    /// <summary>
    /// Switches to default mode (backward compatibility)
    /// </summary>
    /// <param name="isForRadialMenu">Whether this switch is for the radial menu</param>
    public void SwitchToDefaultMode(bool isForRadialMenu)
    {
        SwitchToMode(InputMode.Default, isForRadialMenu);
    }

    /// <summary>
    /// Switches to room creation mode (backward compatibility)
    /// </summary>
    public void SwitchToRoomCreationMode()
    {
        SwitchToMode(InputMode.RoomCreation);
    }

    /// <summary>
    /// Switches to item placement mode (backward compatibility)
    /// </summary>
    public void SwitchToItemPlacementMode()
    {
        SwitchToMode(InputMode.ItemPlacement);
    }

    /// <summary>
    /// Switches to catalog menu mode (backward compatibility)
    /// </summary>
    public void SwitchToCatalogMenuMode()
    {
        SwitchToMode(InputMode.CatalogMenu);
    }

    /// <summary>
    /// Checks if the item placer is active (backward compatibility)
    /// </summary>
    /// <returns>True if in item placement mode</returns>
    public bool IsInPlacerMode()
    {
        return IsInMode(InputMode.ItemPlacement);
    }
}