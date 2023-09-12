using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
  private int _lives = 3;
  private SpawnManager _spawnManager;

  [SerializeField]
  private GameObject _laserPrefab;
  private float _fireRate = 0.2f;
  private float _canFire = -1f;

  [SerializeField]
  private GameObject _TripleShotPrefab;
  private bool _tripleShotActive = false;
  private float _tripleShotCooldown = 5f;

  private float _playerSpeed = 10f;

  [SerializeField]
  private GameObject _thrusters;
  private bool _speedBoostActive = false;
  private float _speedBoostCooldown = 5f;
  private float _speedBoostMultiplier = 1.5f;

  [SerializeField]
  private GameObject _shieldVisualPrefab;
  private bool _shieldActive = false;

  private int _score;
  private UiManager _uiManager;

  [SerializeField]
  private GameObject _engineLeft, _engineRight;

  void Start()
  {
    transform.position = new Vector3(0, -4.4f, 0);

    _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    if (_spawnManager == null)
    {
      Debug.LogError("Spawn Manager is NULL");
    }

    _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
    if (_uiManager == null)
    {
      Debug.LogError("UI Manager is NULL");
    }
  }

  void Update()
  {
    PlayerMovement();
    if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
    {
      PlayerShooting();
    }
  }

  void PlayerMovement()
  {
    //Movement
    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");
    Vector3 direction = new(horizontalInput, verticalInput, 0);
    transform.Translate(_playerSpeed * Time.deltaTime * direction);

    //Boundries
    transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.5f, 4.5f), 0);
    if (transform.position.x > 11.5f)
    {
      transform.position = new Vector3(-11.5f, transform.position.y, 0);
    }
    else if (transform.position.x < -11.5f)
    {
      transform.position = new Vector3(11.5f, transform.position.y, 0);
    }

  }

  void PlayerShooting()
  {
    _canFire = Time.time + _fireRate;
    if (_tripleShotActive == true)
    {
      Instantiate(_TripleShotPrefab,
                  transform.position,
                  Quaternion.identity);
    }
    else
    {
      Instantiate(_laserPrefab,
                  transform.position + new Vector3(0, 1.2f, 0),
                  transform.rotation);
    }
  }

  public void Damage()
  {

    if (_shieldActive == true)
    {
      _shieldActive = false;
      _shieldVisualPrefab.SetActive(false);
      return;
    }
    else
    {
      _lives--;
      _uiManager.UpdateLives(_lives);
    }

    switch (_lives)
    {
      case 3:
        _engineLeft.SetActive(false);
        _engineRight.SetActive(false);
        break;
      case 2:
        _engineLeft.SetActive(true);
        _engineRight.SetActive(false);
        break;
      case 1:
        _engineLeft.SetActive(true);
        _engineRight.SetActive(true);
        break;
      case 0:
        _spawnManager.OnPlayerDeath();
        Destroy(gameObject);
        break;
      default:
        Debug.LogError("ERROR Lives at: " + _lives);
        break;
    }
  }

  public void TripleShotActive()
  {
    _tripleShotActive = true;
    StartCoroutine(TripleShotDowntime());
  }

  IEnumerator TripleShotDowntime()
  {
    yield return new WaitForSeconds(_tripleShotCooldown);
    _tripleShotActive = false;
  }

  public void SpeedBoostActive()
  {
    _speedBoostActive = true;
    _thrusters.SetActive(true);
    _playerSpeed *= _speedBoostMultiplier;
    StartCoroutine(SpeedBoostDowntime());
  }  

  IEnumerator SpeedBoostDowntime()
  {
    yield return new WaitForSeconds(_speedBoostCooldown);
    _speedBoostActive = false;
    _thrusters.SetActive(false);
    _playerSpeed /= _speedBoostMultiplier;
  }

  public void ShieldActive()
  {
    _shieldActive = true;
    _shieldVisualPrefab.SetActive(true);
  }

  public void AddScore(int points)
  {
    _score += points;
    _uiManager.UpdateScore(_score);
  }
}