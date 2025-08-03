
using Assets.Scripts.UIStateMachine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

namespace Assets.Scripts.Lobby
{
  public class LobbyCreationMenu : BaseUiMenu
  {
    [SerializeField] private Button closeButton;
    [SerializeField] TextMeshProUGUI uniqueCodeText;
    [SerializeField] private Button joinBtn;

    private void Awake()
    {
      closeButton.onClick.AddListener(() => UiStateMachine.Instance.CloseTopMenu());
      joinBtn.onClick.AddListener(CreateAndJoinLobby);
      uniqueCodeText.text = GenerateLobbyCode();
    }

    private void CreateAndJoinLobby()
    {
      NetworkManager.Singleton.StartHost();
      UiStateMachine.Instance.UISTATE = UiState.Game;
    }

    private string GenerateLobbyCode()
    {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      System.Random random = new System.Random();
      char[] code = new char[6];
      for (int i = 0; i < code.Length; i++)
      {
        code[i] = chars[random.Next(chars.Length)];
      }
      return new string(code);
    }
  }
}