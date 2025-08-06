using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Common;
using Assets.Scripts.UIStateMachine;
using System;

namespace Assets.Scripts.Lobby
{
  public class UserNameInputMenu : BaseUiMenu
  {
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private Button submitButton;


    // PlayerUsername is static to ensure it is accessible globally across the application instance.
    // In a multiplayer setup using NGO (Netcode for GameObjects) with multiple instances running on the same machine,
    // storing the username in a local non-static variable could cause conflicts due to overlapping data.
    // Using a static property prevents these conflicts by keeping the value instance-specific within each running app process.
    public static string PlayerUsername { get; private set; }

    private void Start()
    {
      if (PlayerPrefs.HasKey(Constants.UserNameKey))
      {
        string savedName = PlayerPrefs.GetString(Constants.UserNameKey);
        usernameInputField.text = savedName;
        PlayerUsername = savedName;
        // UiStateMachine.Instance.UISTATE = UiState.Lobby;
      }

      submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
      string username = usernameInputField.text.Trim();

      if (string.IsNullOrEmpty(username))
      {
        Debug.LogWarning("Username is empty.");
        return;
      }

      PlayerUsername = username;
      PlayerPrefs.SetString(Constants.UserNameKey, username);
      PlayerPrefs.Save();

      Debug.Log("Username saved: " + PlayerUsername);
      UiStateMachine.Instance.UISTATE = UiState.Lobby;
    }
  }
}