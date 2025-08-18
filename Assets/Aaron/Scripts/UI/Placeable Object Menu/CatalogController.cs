// Written by Aaron Williams
using BugFreeProductions.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the catalog UI for placeable objects with category filtering and item selection.
/// Handles different behaviors for VR and non-VR environments.
/// </summary>
public class CatalogController : MonoBehaviour
{
    [SerializeField]
    private bool isVR = false;
    [SerializeField]
    private Transform vrCameraRigTransform;

    [SerializeField]
    private string ITEM_FOLDER = "ClassItems";
    [SerializeField]
    private string CATEGORIES_FOLDER = "Categories";
    [SerializeField]
    private Transform canvasTransform;
    [SerializeField]
    private GameObject catalogItemPrefab;
    [SerializeField]
    private GameObject filterTogglePrefab;
    [SerializeField]
    private Transform itemsContentPanel;
    [SerializeField]
    private Transform filtersContentPanel;

    [SerializeField]
    private PlacableItemPlacer itemPlacer;

    [SerializeField]
    private VR_PlacableItemPlacerGun itemPlacerGun;

    private const string toggleSuffix = " toggle";

    [SerializeField]
    private List<CatalogFilterToggle> categoryToggles;
    [SerializeField]
    private List<CatalogItemData> allItems = new List<CatalogItemData>();

    public GameObject CatalogItemPrefab { get => catalogItemPrefab; set => catalogItemPrefab = value; }
    public Transform ItemsContentPanel { get => itemsContentPanel; set => itemsContentPanel = value; }
    public Transform FiltersContentPanel { get => filtersContentPanel; set => filtersContentPanel = value; }
    public List<CatalogFilterToggle> CategoryToggles { get => categoryToggles; set => categoryToggles = value; }

    private void Start()
    {
        LoadItems();
        LoadFilterToggles();
        UpdateCatalog();
    }

    /// <summary>
    /// Loads and instantiates all filter toggle buttons (CategorySO) from the CATEGORIES_FOLDER within Resources.
    /// </summary>
    private void LoadFilterToggles()
    {
        categoryToggles.Clear();

        CategorySO[] categories = Resources.LoadAll<CategorySO>(CATEGORIES_FOLDER);

        foreach (CategorySO category in categories)
        {
            GameObject filterToggleGameObject = Instantiate(filterTogglePrefab, filtersContentPanel);
            filterToggleGameObject.name = category + toggleSuffix;

            CatalogFilterToggle filterToggleComponent = filterToggleGameObject.GetComponent<CatalogFilterToggle>();
            filterToggleComponent.Category = category.Category;
            filterToggleComponent.onValueChanged.AddListener(delegate { UpdateCatalog(); });

            filterToggleGameObject.transform.GetChild(0).GetComponent<Image>().sprite = category.Sprite;

            categoryToggles.Add(filterToggleComponent);
        }
    }

    /// <summary>
    /// Loads and instantiates all catalog item buttons (ItemSO) from the ITEM_FOLDER within Resources.
    /// </summary>
    private void LoadItems()
    {
        allItems.Clear();

        ItemSO[] items = Resources.LoadAll<ItemSO>(ITEM_FOLDER);

        foreach (ItemSO item in items)
        {
            GameObject catalogButton = Instantiate(catalogItemPrefab, itemsContentPanel);
            catalogButton.GetComponent<Button>().onClick.AddListener(delegate { SelectObjectToPlace(catalogButton); });
            CatalogItemData catalogItemData = catalogButton.AddComponent<CatalogItemData>();
            catalogButton.GetComponent<Image>().sprite = item.Sprite;
            catalogItemData.Initialize(item.Id, item.Category, item.Sprite);
            allItems.Add(catalogItemData);
        }
    }

    /// <summary>
    /// Updates which catalog items are visible based on active category filters
    /// </summary>
    private void UpdateCatalog()
    {
        foreach (CatalogItemData item in allItems)
        {
            bool shouldDisplay = false;

            foreach (var filter in categoryToggles)
            {
                if (filter.isOn && (string.Equals(item.Category, filter.Category, StringComparison.OrdinalIgnoreCase)))
                {
                    shouldDisplay = true;
                }
            }
            item.gameObject.SetActive(shouldDisplay);
        }
    }

    /// <summary>
    /// Sets the selected item to be placed when a catalog item is clicked
    /// </summary>
    /// <param name="catalogButton">The catalog button GameObject that was clicked</param>
    private void SelectObjectToPlace(GameObject catalogButton)
    {
        if(isVR)
        {
            itemPlacerGun.ItemID = catalogButton.GetComponent<CatalogItemData>().Id;
        }
        else
        {
            itemPlacer.ItemID = catalogButton.GetComponent<CatalogItemData>().Id;
        }
    }

    /// <summary>
    /// Toggles the visibility of the catalog menu with appropriate positioning and input handling
    /// </summary>
    public void ToggleMenu()
    {
        if (!canvasTransform.gameObject.activeInHierarchy)
        {
            if (isVR && VRInputMapManager.Instance.IsInPlacerMode())
            {
                Vector3 spawnPosition = vrCameraRigTransform.position + vrCameraRigTransform.forward * 2.5f;
                Quaternion spawnRotation = Quaternion.LookRotation(vrCameraRigTransform.forward, Vector3.up);
                gameObject.transform.position = new Vector3(spawnPosition.x, 1f, spawnPosition.z);
                gameObject.transform.rotation = spawnRotation;
                VRInputMapManager.Instance.SwitchToCatalogMenuMode();
                canvasTransform.gameObject.SetActive(true);
            }
            
            if(!isVR)
            {
                InputMapManager.Instance.SwitchToCatalogMenuActionMap();
                UIUtils.EnableUILock();
                canvasTransform.gameObject.SetActive(true);
            }
        }
        else
        {
            if (isVR)
            {
                VRInputMapManager.Instance.OnCloseMenuUI();
            }

            if (!isVR)
            {
                InputMapManager.Instance.SwitchToItemPlacementActionMap();
                UIUtils.DisableUILock();
            }
            canvasTransform.gameObject.SetActive(false);
        }
    }
}