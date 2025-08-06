using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UIStateMachine;
using TMPro;
namespace Assets.Scripts.Game
{
  public class MainMenu : BaseUiMenu
  {
    [SerializeField] private Button startGameBtn;
    [SerializeField] private TextMeshProUGUI waitingText;

    private void Start()
    {
      startGameBtn.onClick.AddListener(() => GameStateManager.Instance.SetGameStateServerRpc(GameState.Playing));
      waitingText.text = GameStateManager.Instance.CurrentGameState == GameState.Playing ? 
        "Wait for current round to finish." 
        : "Waiting for others to connect!";
    }
  }
}