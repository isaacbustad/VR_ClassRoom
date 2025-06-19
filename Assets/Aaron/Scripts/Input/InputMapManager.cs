// Written by Aaron Williams
using BugFreeProductions.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Singleton manager for Unity's Input System action maps.
/// Controls which input map is active and manages component states when switching between different modes.
/// </summary>
public class InputMapManager : BaseInputManager
{
    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField]
    private RoomGenerator roomGenerator;

    [SerializeField]
    private PlacableItemPlacer itemPlacer;

    [SerializeField]
    private PlacableItemRemover itemRemover;

    /// <summary>Input action map name for default navigation state</summary>
    private const string DEFAULT_ACTION_MAP = "Default_State_Input";
    /// <summary>Input action map name for room creation tools</summary>
    private const string ROOM_CREATION_ACTION_MAP = "Room_Creation_Input";
    /// <summary>Input action map name for item placement tools</summary>
    private const string ITEM_PLACEMENT_ACTION_MAP = "Item_Placement_Input";
    /// <summary>Input action map name for catalog menu interaction</summary>
    private const string CATALOG_MENU_ACTION_MAP = "Catalog_Menu_Input";

    public static InputMapManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("InputMapManager").AddComponent<InputMapManager>();
            }
            return instance as InputMapManager;
        }
    }

    /// <summary>
    /// Initialize required components if not assigned in the inspector
    /// </summary>
    protected override void InitializeComponents()
    {
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        if (roomGenerator == null)
        {
            roomGenerator = FindObjectOfType<RoomGenerator>();
        }

        if (itemPlacer == null)
        {
            itemPlacer = FindObjectOfType<PlacableItemPlacer>();
        }

        if (itemRemover == null)
        {
            itemRemover = FindObjectOfType<PlacableItemRemover>();
        }
    }

    /// <summary>
    /// Switches to the specified input mode
    /// </summary>
    /// <param name="newMode">The mode to switch to</param>
    /// <param name="isTemporary">Whether this is a temporary mode change</param>
    public override void SwitchToMode(InputMode newMode, bool isFromRadialMenu = false)
    {
        HandleModeSwitch(newMode, isFromRadialMenu);
    }

    /// <summary>
    /// Apply the component states and action map changes for a specific mode
    /// </summary>
    /// <param name="mode">The mode to apply</param>
    protected override void ApplyModeEffects(InputMode mode)
    {
        switch (mode)
        {
            case InputMode.Default:
                SwitchActionMap(DEFAULT_ACTION_MAP);
                SafeSetComponentState(itemPlacer, false);
                SafeSetComponentState(itemRemover, false);
                SafeSetComponentState(roomGenerator, false);
                HideFloorPoints(roomGenerator);
                break;

            case InputMode.RoomCreation:
                SwitchActionMap(ROOM_CREATION_ACTION_MAP);
                SafeSetComponentState(itemPlacer, false);
                SafeSetComponentState(itemRemover, false);
                SafeSetComponentState(roomGenerator, true);
                ShowFloorPoints(roomGenerator);
                break;

            case InputMode.ItemPlacement:
                SwitchActionMap(ITEM_PLACEMENT_ACTION_MAP);
                SafeSetComponentState(itemPlacer, true);
                SafeSetComponentState(itemRemover, true);
                SafeSetComponentState(roomGenerator, false);
                HideFloorPoints(roomGenerator);
                break;

            case InputMode.CatalogMenu:
                SwitchActionMap(CATALOG_MENU_ACTION_MAP);
                SafeSetComponentState(itemPlacer, false);
                SafeSetComponentState(itemRemover, false);
                SafeSetComponentState(roomGenerator, false);
                HideFloorPoints(roomGenerator);
                break;
        }
    }

    /// <summary>
    /// Switches the Unity Input System action map
    /// </summary>
    /// <param name="actionMapName">The name of the action map to switch to</param>
    private void SwitchActionMap(string actionMapName)
    {
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component is null in InputMapManager");
            return;
        }

        playerInput.SwitchCurrentActionMap(actionMapName);
    }

    #region Legacy Method Wrappers
    // Keep these for backward compatibility, so we don't have to change existing method calls in the editor
    /// <summary>
    /// Switches to default mode (backward compatibility)
    /// </summary>
    public void SwitchToDefaultActionMap() => SwitchToMode(InputMode.Default, false);
    /// <summary>
    /// Switches to room creation mode (backward compatibility)
    /// </summary>
    public void SwitchToRoomCreationActionMap() => SwitchToMode(InputMode.RoomCreation, false);
    /// <summary>
    /// Switches to item placement mode (backward compatibility)
    /// </summary>
    public void SwitchToItemPlacementActionMap() => SwitchToMode(InputMode.ItemPlacement, false);
    /// <summary>
    /// Switches to catalog menu mode (backward compatibility)
    /// </summary>
    public void SwitchToCatalogMenuActionMap() => SwitchToMode(InputMode.CatalogMenu, false);
    #endregion
}