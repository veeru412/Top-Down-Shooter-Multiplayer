using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UIStateMachine
{
  public interface IUIMenu
  {
    void Show();
    void Hide();
    bool IsOverlay { get; }
  }
}