using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SplashLoader : MonoBehaviour
{
  private async void Start()
  {
    await InitializeServices();
    SceneManager.LoadScene("Lobby");
  }

  private async Task InitializeServices()
  {
    /*if (!UnityServices.State.Equals(ServiceState.Initialized))
    {
      try
      {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
          await AuthenticationService.Instance.SignInAnonymouslyAsync();
          Debug.Log("Signed in anonymously as: " + AuthenticationService.Instance.PlayerId);
        }
      }
      catch (System.Exception e)
      {
        Debug.LogError("Failed to initialize Unity Services: " + e.Message);
      }
    }*/
    await Task.Delay(1000); // Simulate some initialization delay 
    Debug.Log("Services initialized successfully.");
  }
}
