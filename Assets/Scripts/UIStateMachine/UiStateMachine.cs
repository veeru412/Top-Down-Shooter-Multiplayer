using System;
using System.Collections.Generic;
using Assets.Scripts.Common;

namespace Assets.Scripts.UIStateMachine
{
  public class UiStateMachine : Singleton<UiStateMachine>
  {
    private Stack<IUIMenu> menuStack = new();
    public event Action<UiState> OnStateChange;

    private UiState uiState;
    public UiState UISTATE
    {
      get { return uiState; }
      set { uiState = value;  
      OnStateChange?.Invoke(uiState);
      }
    }
    public void OpenMenu(IUIMenu newMenu)
    {
      if (newMenu == null) return;

      if (!newMenu.IsOverlay && menuStack.Count > 0)
      {
        var topMenu = menuStack.Peek();
        topMenu.Hide();
      }

      newMenu.Show();
      menuStack.Push(newMenu);
    }

    public void CloseTopMenu()
    {
      if (menuStack.Count == 0) return;

      var topMenu = menuStack.Pop();
      topMenu.Hide();

      if (!topMenu.IsOverlay && menuStack.Count > 0)
      {
        menuStack.Peek().Show();
      }
    }

    public void CloseAll()
    {
      while (menuStack.Count > 0)
      {
        menuStack.Pop().Hide();
      }
    }
  }
}