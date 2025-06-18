// Written by Aaron Williams
using UnityEngine;

/// <summary>
/// Scriptable Object that defines a placeable item in the catalog system.
/// Contains all data needed to display and instantiate a placeable object.
/// </summary>
[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObject/ItemSO")]
public class ItemSO : ScriptableObject
{
    /// <summary>Unique identifier for this item that is passed to the PlacableItemGun</summary>
    public string Id;

    /// <summary>Category this item belongs to, used for filtering</summary>
    public string Category;

    /// <summary>The sprite used to display this item in the catalog</summary>
    public Sprite Sprite;
}