using System.Collections;
using UnityEngine;
using Assets.Scripts.UIStateMachine;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
  public class GameOverMenu : BaseUiMenu
  {
    [SerializeField] private Button exitButton;

    private void Start()
    {
      exitButton.onClick.AddListener(() => GameStateManager.Instance.SetGameStateServerRpc(GameState.Waiting));
    }
  }
}