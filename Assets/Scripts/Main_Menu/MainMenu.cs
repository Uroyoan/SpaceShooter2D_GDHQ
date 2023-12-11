using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
  [SerializeField]
  private GameObject _mainMenuScreen;
  [SerializeField]
  private GameObject _controlsScreen;
  [SerializeField]
  private GameObject _exampleControls;
  [SerializeField]
  private GameObject _creditsScreen;

  public void Update()
  {
    ExitGame();
  }

  public void LoadGame()
  {
    SceneManager.LoadScene(1);
  }

  public void ExitGame()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Application.Quit();
    }
  }

  public void ControlMenu()
  {
    _mainMenuScreen.SetActive(false);
    _controlsScreen.SetActive(true);
    _creditsScreen.SetActive(false);
    _exampleControls.SetActive(true);
  }

  public void CreditsMenu()
  {
    _mainMenuScreen.SetActive(false);
    _controlsScreen.SetActive(false);
    _creditsScreen.SetActive(true);
    _exampleControls.SetActive(false);
  }
  
}
