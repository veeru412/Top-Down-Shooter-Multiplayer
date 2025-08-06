using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common;
using Assets.Scripts.UIStateMachine;
using Unity.Netcode;

namespace Assets.Scripts.Lobby
{
  public class LobbyMenu : BaseUiMenu
  {
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI usernameTxt;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button quitButton;


    private void Start()
    {
      hostButton.onClick.AddListener(HostLobby);
      joinButton.onClick.AddListener(JoinLobby);
      quickJoinButton.onClick.AddListener(QuickJoin);
      quitButton.onClick.AddListener(Quit);
      SetUsername();
    }

    private void SetUsername()
    {
      var userName = PlayerPrefs.GetString(Constants.UserNameKey);
      usernameTxt.SetText($"Welcome: {userName}");
    }

    private void HostLobby()
    {
      UiStateMachine.Instance.UISTATE = UiState.LobbyCodeMenu;
    }

    private void JoinLobby()
    {
      UiStateMachine.Instance.UISTATE = UiState.JoinLobbyWithCode;
    }

    private void QuickJoin()
    {
      Debug.Log("Attempting to quick join a random lobby...");
      NetworkManager.Singleton.StartClient();
      UiStateMachine.Instance.UISTATE = UiState.SceneLoading;
    }
    private void Quit()
    {
      Application.Quit();
    }
  }
}