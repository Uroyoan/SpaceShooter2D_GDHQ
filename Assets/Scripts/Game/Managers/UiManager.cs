using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;

public class UiManager : MonoBehaviour
{

  [SerializeField]
  private TMP_Text _scoreText;

  [SerializeField]
  private Image _livesImg;
  [SerializeField]
  private Sprite[] _livesSprites;

  [SerializeField]
  private TMP_Text _gameOverText;
  [SerializeField]
  private TMP_Text _restartText;

  private GameManager _gameManager;

  [SerializeField]
  private Image _livesimg;

  [SerializeField]
  private Image _fuelImg;

  [SerializeField]
  private TMP_Text _ammoText;
  [SerializeField]
  private Image _ammoclipFill;

  [SerializeField]
  private TMP_Text _wavesText;

  void Start()
  {
    _gameOverText.gameObject.SetActive(false);
    _restartText.gameObject.SetActive(false);
    _scoreText.text = "Score: " + 0;
    _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
    if (_gameManager == null)
    {
      Debug.LogError("Game_Manager is null");
    }
  }

  public void UpdateScore(int playerScore)
  {
    _scoreText.text = "Score: " + playerScore;
  }

  public void UpdateLives(int currentLives)
  {
    if (currentLives > 0)
    {
      _livesImg.sprite = _livesSprites[currentLives];
    }
    else
    {
      currentLives = 0;
      _livesImg.sprite = _livesSprites[currentLives];
      GameOverSequence();
    }
  }

  IEnumerator GameOverFlikerRoutine()
  {
    while (true)
    {
      _gameOverText.gameObject.SetActive(true);
      yield return new WaitForSeconds(0.5f);
      _gameOverText.gameObject.SetActive(false);
      yield return new WaitForSeconds(0.5f);
    }
  }

  void GameOverSequence()
  {
    _gameOverText.gameObject.SetActive(true);
    _restartText.gameObject.SetActive(true);
    _gameManager.GameOver();
    StartCoroutine(GameOverFlikerRoutine());
  }

  public void UpdateFuel (float thrusterFuel)
  {
    _fuelImg.fillAmount = thrusterFuel;
  }

  public void UpdateAmmo(float ammoAmount)
  {
    _ammoText.text = "" + ammoAmount;
  }

  public void UpdateClip(float clipAmmount)
  {
    _ammoclipFill.fillAmount = clipAmmount / 4;
  }

  public void UpdateWaves(int currentWave)
  {
    _wavesText.text = "Wave: " + currentWave;
  }

}