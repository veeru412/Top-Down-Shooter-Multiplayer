using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Assets.Scripts.UIStateMachine;

namespace Assets.Scripts.Game
{
  public class GameSceneUiManager : BaseSceneUiManager
  {
    [SerializeField] private BaseUiMenu waitingScreen;
    [SerializeField] private BaseUiMenu inGameMenu;
    [SerializeField] private BaseUiMenu gameOverMenu;

    private void OnGameStateChanged(GameState newValue)
    {
      Debug.Log($"State changed to {newValue}");
      var uiState = GetMappedUiState(newValue);
      var uiMenu = GetCurrentStateMenu(uiState);
      if (uiMenu != null)
      {
        UiStateMachine.Instance.OpenMenu(uiMenu);
      }
    }

    protected override BaseUiMenu GetCurrentStateMenu(UiState state)
    {
      return state switch
      {
        UiState.WaitingForPlayers => waitingScreen,
        UiState.Game => inGameMenu,
        UiState.GameOver => gameOverMenu,
        _ => null
      };
    }

    private UiState GetMappedUiState(GameState state)
    {
      return state switch
      {
        GameState.Playing => UiState.Game,
        GameState.Finished => UiState.GameOver,
        _ => UiState.WaitingForPlayers
      };
    }

    private void Start()
    {
      GameStateManager.Instance.RegisterOnGameStateChanged(OnGameStateChanged);
      UiStateMachine.Instance.OpenMenu(waitingScreen);
    }
  
    private void OnDestroy()
    {
      if(GameStateManager.Instance != null)
      {
        GameStateManager.Instance.UnregisterOnGameStateChanged(OnGameStateChanged);
      }
    }
  }
}