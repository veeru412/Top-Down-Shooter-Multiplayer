using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.UIStateMachine;

namespace Assets.Scripts.Game
{
  public class MainMenu : BaseUiMenu
  {
    [SerializeField] private Button startGameBtn;
    private void Awake()
    {
      startGameBtn.onClick.AddListener(() => GameStateManager.Instance.SetGameStateServerRpc(GameState.Playing));
      startGameBtn.gameObject.SetActive(false);
    }
  }
}