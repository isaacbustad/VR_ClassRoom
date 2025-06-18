// Written by Aaron Williams
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Controls a radial menu that allows users to select options by pointing the mouse pointer
/// or their VR controller at different segments.
/// </summary>
public class RadialMenu : MonoBehaviour
{
    [Header("Is In Virtual Reality")]
    [SerializeField] private bool isVR;
    [SerializeField] private Camera mainCamera;

    [Header("UI Options")]
    [SerializeField] private List<MenuOption> Menus;

    [Header("Keybaord + Mouse Settings")]
    [SerializeField] private KeyCode menuActivationKey;

    [Header("Radial MenuGameObject Settings")]
    
    [SerializeField] private float segmentPadding;

    [Header("Editor Assigments")]
    [SerializeField] private Transform menuCanvas;
    [SerializeField] private Transform handTransform;
    [SerializeField] private GameObject haloPrefab;
    [SerializeField] private GameObject centerText;

    [Header("Spawning Adjustments")]
    [SerializeField] private float menuDistanceOffset = 0.15f;
    [SerializeField] private float kbmMenuDistanceOffset = 0.5f;

    private List<GameObject> radialMenuOption = new();

    private int selectedMenuOptionIndex;

    private const string CLOSE = "Close";

    private void Start()
    {
        InitializeRadialMenu();
    }
    private void Update()
    {
        if (!isVR)
        {
            if (menuCanvas.gameObject.activeInHierarchy)
            {
                GetSelectedRadialPart();

                if (Input.GetMouseButtonUp(0))
                {
                    SelectMenuOption();
                }
            }
        }
    }

    // TODO: evntually it would be good to make these assignments programatically. Leaving here for reference still in case the inspector breaks
    // Old method, left for now, so we can see what methods are assigned to what buttons in case the inspector breaks
    //private void HandleVRInput()
    //{
    //
    //    if (OVRInput.GetDown(selectOptionButton, ovrController))
    //    {
    //        EnableRadialMenu();
    //    }

    //    if (OVRInput.Get(selectOptionButton, ovrController))
    //    {
    //        GetSelectedRadialPart();
    //    }

    //    if (OVRInput.GetUp(selectOptionButton, ovrController))
    //    {
    //        SelectMenuOption();
    //    }
    //}

    /// <summary>
    /// Toggle the radial menu visibility for keyboard and mouse
    /// </summary>
    public void HandleToggleRadialMenuKBM()
    {
        if (!menuCanvas.gameObject.activeInHierarchy)
        {
            EnableRadialMenu();
        }
        else
        {
            DisableRadialMenu(false);
        }
    }

    /// <summary>
    /// Create and initialize the radial menu segments
    /// </summary>
    private void InitializeRadialMenu()
    {
        if (isVR)
        {
            menuCanvas.gameObject.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        }
        else if (!isVR)
        {
            menuCanvas.gameObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        }

        // this Close option is sort of hard coded for now until a UI/UX master comes in and reworks the UI in the future, as I am just a humble code monkey
        MenuOption closeMenuOption = new MenuOption(CLOSE);

        if(isVR)
        {
            closeMenuOption.OnSelect.AddListener(() => VRInputMapManager.Instance.SwitchToDefaultMode(false));
        }
        else if(!isVR)
        {
            closeMenuOption.OnSelect.AddListener(() => InputMapManager.Instance.SwitchToDefaultActionMap());
        }

        Menus.Insert(1, closeMenuOption);

        for (int i = 0; i < Menus.Count; i++)
        {
            float angle = -i * 360 / Menus.Count - segmentPadding / 2;
            Vector3 radialPartEulerAngle = new Vector3(0, 0, angle);

            GameObject menOption = Instantiate(haloPrefab, menuCanvas);
            menOption.transform.position = menuCanvas.position;
            menOption.transform.localEulerAngles = radialPartEulerAngle;

            menOption.GetComponent<Image>().fillAmount = (1 / (float)Menus.Count) - (segmentPadding / 360);

            radialMenuOption.Add(menOption);
        }
    }

    /// <summary>
    /// Determines which menu option is currently being selected based on pointer/controller location about the menu.
    /// </summary>
    public void GetSelectedRadialPart()
    {
        Vector3 selectionPoint;

        if (isVR)
        {
            selectionPoint = GetTransform().position;
        }
        else
        {
            Vector2 mousePosition = Input.mousePosition;
            selectionPoint = mousePosition;
        }

        Vector3 centerToSelectionPoint = selectionPoint - menuCanvas.position;
        Vector3 centerToSelectionPointProjected = Vector3.ProjectOnPlane(centerToSelectionPoint, menuCanvas.forward);

        float angle = Vector3.SignedAngle(menuCanvas.up, centerToSelectionPointProjected, -menuCanvas.forward);

        if (angle < 0) { angle += 360; }

        selectedMenuOptionIndex = (int)angle * Menus.Count / 360;

        foreach (GameObject menuOption in radialMenuOption)
        {
            menuOption.GetComponent<Image>().color = Color.white;
            menuOption.transform.localScale = Vector3.one;
            menuOption.GetComponentInChildren<Text>().enabled = false;
        }

        for (int i = 0; i < radialMenuOption.Count; i++)
        {
            if (i == selectedMenuOptionIndex)
            {
                radialMenuOption[i].GetComponent<Image>().color = i == 1 ? Color.red : Color.green;
                radialMenuOption[i].transform.localScale = 1.1f * Vector3.one;
                radialMenuOption[i].GetComponentInChildren<Text>().enabled = true;
                centerText.GetComponent<Text>().text = Menus[selectedMenuOptionIndex].MenuName;
            }
            else
            {
                radialMenuOption[i].GetComponent<Image>().color = Color.white;
                radialMenuOption[i].transform.localScale = Vector3.one;
                radialMenuOption[i].GetComponentInChildren<Text>().enabled = false;
            }
        }
    }

    /// <summary>
    /// Execute the action for the currently selected menu option
    /// </summary>
    public void SelectMenuOption()
    {
        radialMenuOption[selectedMenuOptionIndex].SetActive(true);

        if (selectedMenuOptionIndex < radialMenuOption.Count)
        {
            Menus[selectedMenuOptionIndex].OnSelect?.Invoke();
            DisableRadialMenu(false);
        }
        else
        {
            DisableRadialMenu(false);
        }
    }

    /// <summary>
    /// Show the radial menu and position it. Different positioning for VR and keyboard/mouse.
    /// </summary>
    public void EnableRadialMenu()
    {
        if(!isVR)
        {
            UIUtils.EnableUILock();
            InputMapManager.Instance.OnOpenRadialMenuUI();
        }
        else if(isVR)
        {
            VRInputMapManager.Instance.OnOpenRadialMenuUI();
        }

        menuCanvas.gameObject.SetActive(true);

        if (isVR)
        {
            Vector3 eulerAngles = handTransform.rotation.eulerAngles;
            eulerAngles.x = 0;
            eulerAngles.z = 0;
            Vector3 spawnPosition = handTransform.position + (handTransform.forward * GetDistanceOffset());

            menuCanvas.SetPositionAndRotation(spawnPosition, Quaternion.Euler(eulerAngles));
        }
    }

    // <summary>
    /// Hide the radial menu and restore previous input state
    /// </summary>
    /// <param name="selectedNewMenu">Whether a new menu was selected (affects input handling)</param>
    public void DisableRadialMenu(bool selectedNewMenu)
    {
        if (!isVR && !selectedNewMenu)
        {
            UIUtils.DisableUILock();
            InputMapManager.Instance.OnCloseMenuUI();
        }
        else if (isVR)
        {
            VRInputMapManager.Instance.OnCloseMenuUI();
        }
        menuCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Get the appropriate transform for input based on VR/non-VR mode
    /// </summary>
    private Transform GetTransform()
    {
        return isVR? handTransform : mainCamera.transform;
    }

    /// <summary>
    /// Get the appropriate distance offset for menu placement based on VR/non-VR mode
    /// </summary>
    private float GetDistanceOffset()
    {
        return isVR ? menuDistanceOffset : kbmMenuDistanceOffset;
    }
}

/// <summary>
/// Represents a selectable option in the radial menu with an associated action
/// </summary>
[System.Serializable]
public class MenuOption
{
    [SerializeField] private string menuName;
    // This is cool, it's how the unity input maps let you assigns methods to be ran on an action.
    [SerializeField] private UnityEvent onSelect;

    public string MenuName { get => menuName; set => menuName = value; }
    public UnityEngine.Events.UnityEvent OnSelect { get => onSelect; set => onSelect = value; }

    public MenuOption(string name)
    {
        this.menuName = name;
        this.onSelect = new();
    }
}
