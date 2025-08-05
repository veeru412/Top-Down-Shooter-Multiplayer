using UnityEngine;
using Assets.Scripts.UIStateMachine;
using Assets.Scripts.Common;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace Assets.Scripts.Lobby
{
  public class LobbySceneManager : BaseSceneUiManager
  {
    [SerializeField] private BaseUiMenu userNameMenu;
    [SerializeField] private BaseUiMenu lobbyMenu;
    [SerializeField] private BaseUiMenu lobbyCreationMenu;
    [SerializeField] private BaseUiMenu joinLobbyMenu;
    [SerializeField] private BaseUiMenu gameMenu;

    protected override BaseUiMenu GetCurrentStateMenu(UiState state)
    {
      return state switch
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
}