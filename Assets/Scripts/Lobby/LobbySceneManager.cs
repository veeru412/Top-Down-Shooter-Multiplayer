using UnityEngine;
using Assets.Scripts.UIStateMachine;
using System;

namespace Assets.Scripts.Lobby
{
  public class LobbySceneManager : MonoBehaviour
  {
    [SerializeField] private BaseUiMenu userNameMenu;
    [SerializeField] private BaseUiMenu lobbyMenu;
    [SerializeField] private BaseUiMenu lobbyCreationMenu;
    [SerializeField] private BaseUiMenu joinLobbyMenu;
    [SerializeField] private BaseUiMenu gameMenu;
    private void Awake()
    {
      UiStateMachine.Instance.OnStateChange += OnUiStateChanged;
      UiStateMachine.Instance.UISTATE = UiState.UserInput;
    }
    private void OnDisable()
    {
      UiStateMachine.Instance.OnStateChange -= OnUiStateChanged;
    }
    private void OnUiStateChanged(UiState uiState)
    {
      var menu = GetCurrentStateMenu(uiState); 
      if (menu != null) { 
        UiStateMachine.Instance.OpenMenu(menu);
      }
      if(uiState== UiState.Game) {
        lobbyMenu.gameObject.SetActive(false);
      }
    }

    private IUIMenu GetCurrentStateMenu(UiState state) => state switch
    {
      UiState.UserInput => userNameMenu,
      UiState.Lobby => lobbyMenu,
      UiState.LobbyCodeMenu => lobbyCreationMenu,
      UiState.JoinLobbyWithCode => joinLobbyMenu,
      UiState.Game => gameMenu,
      _ => null
    };
  }
}