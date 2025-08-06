using System.Collections;
using UnityEngine;
using Assets.Scripts.UIStateMachine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game
{
  public class GameOverMenu : BaseUiMenu
  {
    [SerializeField] private Button replayButton;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private TextMeshProUGUI winnerReferenceText;
    [SerializeField] private Button exitButton;

    private void Start()
    {
      UpdateWinnerText();
      replayButton.onClick.AddListener(StartGameServerRpc);
      exitButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void UpdateWinnerText()
    {
      string[] parts = winnerReferenceText.text.Split(new string[] { " - " }, System.StringSplitOptions.None);
      string playerName = parts[0];
      string killsInfo = parts.Length > 1 ? parts[1] : "";

      string formatted = $"{playerName}\n\n<color=#FFD700><size=120%><b>{killsInfo}</b></size></color>";

      winnerText.text = formatted;
    }

    public void OnMainMenuButtonClicked()
    {
      NetworkManager.Singleton.Shutdown();
      SceneManager.LoadScene("Lobby");
    }

    [ServerRpc]
    private void StartGameServerRpc()
    {
      GameStateManager.Instance.SetGameStateServerRpc(GameState.Waiting);
    }

  }
}