// Written by Aaron Williams
using BugFreeProductions.Tools;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Controls the start menu UI flow, allowing users to create new rooms or select existing ones.
/// Handles transitions between different menu panels and loading into selected rooms.
/// </summary>
public class StartMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject startButtonPanel;
    [SerializeField] private GameObject createOrSelectRoomPanel;
    [SerializeField] private GameObject roomSelectionPanel;
    [SerializeField] private GameObject createRoomPanel;

    [SerializeField] private Transform roomSelectionContent;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private TMP_InputField textInput;

    private void Start()
    {
        PopulateRoomButtons();
    }

    #region UI Navigation Methods
    /// <summary>
    /// Handler for the start button - transitions to the create/select room menu
    /// </summary>
    public void StartButtonPressed()
    {
        startButtonPanel.SetActive(false);
        createOrSelectRoomPanel.SetActive(true);
    }

    /// <summary>
    /// Handler for the select room button - shows the room selection panel
    /// </summary>
    public void SelectRoomButtonPressed()
    {
        createOrSelectRoomPanel.SetActive(false);
        roomSelectionPanel.SetActive(true);
    }

    /// <summary>
    /// Handler for the create room button - shows the create room panel
    /// </summary>
    public void CreateRoomButtonPressed()
    {
        createOrSelectRoomPanel.SetActive(false);
        createRoomPanel.SetActive(true);
    }
    #endregion

    #region Room Management Methods
    /// <summary>
    /// Creates a new room with the name entered in the input field and loads into it
    /// </summary>
    public void CreateRoomWithName()
    {
        JSONPlacementMannager.Instance.RoomConfigPath = textInput.text;
        Debug.Log("Creating room with name: " + textInput.text);
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Selects and loads into an existing room
    /// </summary>
    /// <param name="roomName">Name of the room to load</param>
    public void SelectAndLoadIntoRoom(string room)
    {
        JSONPlacementMannager.Instance.RoomConfigPath = room;
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Creates a button for a room in the selection menu
    /// </summary>
    /// <param name="roomName">Name of the room to create a button for</param>
    private void CreateRoomButton(string roomName)
    {
        if (buttonPrefab != null && roomSelectionContent != null)
        {
            GameObject buttonGameObject = Instantiate(buttonPrefab, roomSelectionContent);
            buttonGameObject.name = "Room Option: [" + roomName + "] Button";

            TMP_Text buttonTextComponent = buttonGameObject.GetComponentInChildren<TMP_Text>();
            if (buttonTextComponent != null)
            {
                buttonTextComponent.text = roomName;
            }

            Button buttonButtonComponent = buttonGameObject.GetComponent<Button>();
            if (buttonButtonComponent != null)
            {
                buttonButtonComponent.onClick.AddListener(() => SelectAndLoadIntoRoom(roomName));
            }
        }
    }

    /// <summary>
    /// Populates the room selection panel with buttons for all available rooms
    /// </summary>
    private void PopulateRoomButtons()
    {
        if (JSONPlacementMannager.Instance != null && JSONPlacementMannager.Instance.RoomList != null)
        {
            List<string> roomNames = JSONPlacementMannager.Instance.RoomList;

            if (roomNames != null && roomNames.Count > 0)
            {
                foreach (string roomName in roomNames)
                {
                    CreateRoomButton(roomName);
                }
            }
            else
            {
                Debug.Log("Rooms not found.");
            }
        }
        else
        {
            Debug.Log("RoomList is null");
        }
    }
    #endregion
}