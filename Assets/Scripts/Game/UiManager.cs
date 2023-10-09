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

  private Transform _camera;
  private float _timePassed = 0f;
  private float _xOffset;
  private float _yOffset;
  private float _shakeDuration = 1f;
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
    _camera = GameObject.Find("Main Camera").GetComponent<Transform>();
    if (_camera == null)
    {
      Debug.LogError("Camera is null");
    }
  }

  public void UpdateScore(int playerScore)
  {
    _scoreText.text = "Score: " + playerScore;
  }

  public void UpdateLives(int currentLives)
  {
    _livesImg.sprite = _livesSprites[currentLives];
    if (currentLives <= 0)
    {
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
    StartCoroutine(CameraShake());
  }

  public void UpdateFuel (float thrusterFuel)
  {
    _fuelImg.fillAmount = thrusterFuel;
  }

  public void UpdateAmmo(float ammoAmount)
  {
    _ammoText.text = "Ammo: " + ammoAmount;
  }

  public IEnumerator CameraShake()
  {

    Vector3 _startPos = _camera.transform.localPosition;

    while (_timePassed < _shakeDuration)
    {
      _xOffset = Random.Range(-1f, 1f) * 0.5f;
      _yOffset = Random.Range(-1f, 1f) * 0.5f;

      _camera.transform.localPosition = new Vector3(_xOffset, _yOffset, _startPos.z);
      _timePassed += Time.deltaTime;
      yield return new WaitForEndOfFrame();
    }
    
    _camera.transform.localPosition = _startPos;
    _timePassed = 0;
  }
}
