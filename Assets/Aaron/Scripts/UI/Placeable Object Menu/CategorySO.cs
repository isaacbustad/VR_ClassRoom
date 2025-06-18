// Written by Aaron Williams
using UnityEngine;

/// <summary>
/// Scriptable Object that defines a category for placeable items in the catalog system.
/// Categories allow for filtering of placeable objects in the Catalog.
/// </summary>
[CreateAssetMenu(fileName = "CategorySO", menuName = "ScriptableObject/CategorySO")]
public class CategorySO : ScriptableObject
{
    /// <summary>The unique name of this category within the catalog</summary>
    public string Category;

    /// <summary>The sprite used to visually represent this category in the catalog</summary>
    public Sprite Sprite;
}