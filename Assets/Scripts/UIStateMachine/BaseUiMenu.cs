using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UIStateMachine
{
  public class BaseUiMenu : MonoBehaviour, IUIMenu
  {
    [SerializeField] private bool isOverlay;
    bool IUIMenu.IsOverlay => isOverlay;

    public virtual void Hide() => gameObject.SetActive(false);
    
    public virtual void Show() => gameObject.SetActive(true);
    
  }
}