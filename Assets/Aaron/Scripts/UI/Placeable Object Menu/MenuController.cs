// Written by Aaron Williams
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Testing controller for catalog menu functionality.
/// Handles toggling and positioning of the item catalog menu in response to input.
/// </summary>
public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public Transform playerTransform;
    public GameObject rightHand;


    /// <summary>
    /// Toggles the item catalog menu visibility and position based on input.
    /// Positions the menu in front of the player and handles cursor visibility.
    /// </summary>
    /// <param name="context">Input action callback context</param>
    public void ToggleItemCatalogMenu(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (!menu.activeSelf)
            {
                Vector3 spawnPosition = playerTransform.position + playerTransform.forward * 2.5f;
                Quaternion spawnRotation = Quaternion.LookRotation(playerTransform.forward);
                menu.gameObject.transform.position = new Vector3(spawnPosition.x, 0.5f, spawnPosition.z);
                menu.gameObject.transform.rotation = spawnRotation;

                rightHand.SetActive(false);
                menu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                menu.SetActive(false);
                rightHand.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
// TODO integrate this with the player controller or something, as of now this is for testing.