using Assets.Scripts.UIStateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

namespace Assets.Scripts.Lobby
{
  public class LobbyJoinMenu : BaseUiMenu
  {
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_InputField uniqueCodeInputField;
    [SerializeField] private Button joinBtn;
    private void Awake()
    {
      closeButton.onClick.AddListener(()=> UiStateMachine.Instance.CloseTopMenu());
      joinBtn.onClick.AddListener(JoinLobby);
    }

    private void JoinLobby()
    {
      var code = uniqueCodeInputField.text.Trim();
      if(string.IsNullOrEmpty(code) )
      {
        Debug.LogError("Must enter code to join lobby");
        return;
      }
      NetworkManager.Singleton.StartClient();
      UiStateMachine.Instance.UISTATE = UiState.SceneLoading;
    }
  }
}