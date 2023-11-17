using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
  [SerializeField]
  private GameObject _exampleShooting;

  public void Update()
  {
    ExitGame();
    moveLaser();
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

  public void moveLaser()
  {
    //Movement
    _exampleShooting.transform.Translate(0.5f * Time.deltaTime * Vector3.up);

    //Bounce back
    if (_exampleShooting.transform.position.y > 2.8f)
    {
      _exampleShooting.transform.position = new Vector3(-3f, 2f, 0);
    }
  }
}
