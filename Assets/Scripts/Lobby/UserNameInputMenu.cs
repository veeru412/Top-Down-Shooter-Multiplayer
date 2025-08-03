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