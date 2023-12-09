using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

  private bool _isGameOver = false;

  private void Update()
  {
    ResetGame();
    ExitGame();
  }

  public void GameOver()
  {
    _isGameOver = true;
  }

  public void ExitGame()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Application.Quit();
    }
  }

  public void ResetGame()
  {
    if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
    {
      SceneManager.LoadScene(1); //Current Game Scene
    }
  }

}
