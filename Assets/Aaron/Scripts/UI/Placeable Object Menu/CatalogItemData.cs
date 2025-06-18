// Written by Aaron Williams
using UnityEngine;

/// <summary>
/// Component attached to catalog item buttons to store and manage associated item data.
/// Provides easy access to item properties for the catalog system. 
/// Instantiated from ItemSO scriptable objects.
/// </summary>
public class CatalogItemData : MonoBehaviour
{
    [SerializeField]
    private string id;
    [SerializeField]
    private string category;
    [SerializeField]
    private Sprite sprite;

    /// <summary>
    /// Constructor to create a new catalog item data instance
    /// </summary>
    /// <param name="name">Unique identifier</param>
    /// <param name="category">Item's category</param>
    /// <param name="sprite">Item's sprite</param>
    public CatalogItemData(string name, string category, Sprite sprite)
    {
        this.id = name;
        this.category = category;
        this.sprite = sprite;
    }

    /// <summary>
    /// Initializes the catalog item data with the specified values
    /// </summary>
    /// <param name="name">Unique identifier</param>
    /// <param name="category">Item's category</param>
    /// <param name="sprite">Item's sprite</param>
    public void Initialize(string name, string category, Sprite sprite)
    {
        this.id = name;
        this.category = category;
        this.sprite = sprite;
    }

    public string Id { get => id; set => id = value; }
    public string Category { get => category; set => category = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
}