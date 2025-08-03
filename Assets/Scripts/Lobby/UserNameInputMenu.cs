using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.Scripts.Lobby
{
  public class UserNameInputMenu : MonoBehaviour
  {
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private Button submitButton;

    public static string PlayerUsername { get; private set; }

    private void Start()
    {
      if (PlayerPrefs.HasKey("Username"))
      {
        string savedName = PlayerPrefs.GetString("Username");
        usernameInputField.text = savedName;
        PlayerUsername = savedName;
        gameObject.SetActive(false);
      }

      submitButton.onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
      string username = usernameInputField.text.Trim();

      if (string.IsNullOrEmpty(username))
      {
        Debug.LogWarning("Username is empty.");
        return;
      }

      PlayerUsername = username;
      PlayerPrefs.SetString("Username", username);
      PlayerPrefs.Save();

      Debug.Log("Username saved: " + PlayerUsername);

    }
  }
}