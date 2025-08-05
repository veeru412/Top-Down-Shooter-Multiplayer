using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Game
{
  public class RoundTimerUI : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private RoundTimeManager timeManager;

    private void Update()
    {
      if (timeManager == null) return;

      var time = timeManager.TimeRemaining;
      string formattedTime = $"{time / 60:D2}:{time % 60:D2}";
      timerText.text = formattedTime.ToString();
    }
  }
}