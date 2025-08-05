using UnityEngine;

namespace Assets.Scripts.UIStateMachine
{
  public abstract class BaseSceneUiManager : MonoBehaviour
  {
    protected virtual void Awake()
    {
      UiStateMachine.Instance.OnStateChange += OnUiStateChanged;
      UiStateMachine.Instance.UISTATE = UiState.UserInput;
    }

    protected virtual void OnDisable()
    {
      if (UiStateMachine.Instance != null)
      {
        UiStateMachine.Instance.OnStateChange -= OnUiStateChanged;
      }
    }

    private void OnUiStateChanged(UiState uiState)
    {
      if (uiState == UiState.SceneLoading)
      {
        UiStateMachine.Instance.CloseAll();
      }
      else
      {
        var menu = GetCurrentStateMenu(uiState);
        if (menu != null)
        {
          UiStateMachine.Instance.OpenMenu(menu);
        }
      }
    }

    protected abstract BaseUiMenu GetCurrentStateMenu(UiState state);
  }

}