using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
  private int _lives = 3;
  private SpawnManager _spawnManager;

  [SerializeField]
  private GameObject _laserPrefab;
  private float _fireRate = 0.2f,
                _canFire = -1f;

  [SerializeField]
  private GameObject _TripleShotPrefab;
  private bool _tripleShotActive = false;
  private float _tripleShotCooldown = 5f;

  [SerializeField]
  private GameObject _thrusters;
  private float _playerBaseSpeed = 10f,
                _modifiedSpeed,
                _speedBoostMultiplier = 1.5f;

  [SerializeField]
  private float _fuelamount = 100,
                _fuelBurnSpeed = 25;

  [SerializeField]
  private GameObject _shieldVisualPrefab;
  private bool _shieldActive = false;
  private int _shieldHealth = 3;
  private SpriteRenderer _shieldColor;

  private int _score;
  private UiManager _uiManager;

  [SerializeField]
  private GameObject _engineLeft,
                     _engineRight;

  [SerializeField]
  private AudioSource _audioSource;
  [SerializeField]
  private AudioClip _LaserShotClip;
  [SerializeField]
  private AudioClip _noAmmoClip;
  [SerializeField]
  private AudioClip _ReloadClip;

  private int _currentAmmo = 5,
              _currentAmmoClip = 1;

  private Transform _camera;
  private float _timePassed = 0f,
                _xOffset,
                _yOffset,
                _shakeDuration = 1f;

  [SerializeField]
  private GameObject _SpreadShotPrefab;
  private bool _spreadShotActive = false;
  private float _spreadShotCooldown = 5f;

  private float _slowSpeed = 2f;
  private bool _systemsActive = true;

  [SerializeField]
  private GameObject _missilePrefab;
  private bool _missileActive = false;
  private float _missileCooldown = 5f;
  [SerializeField]
  private AudioClip _missileClip;

  private Animator _playerMovementAnim;

  void Start()
  {
    transform.position = new Vector3(0, -3f, 0);
    _modifiedSpeed = _playerBaseSpeed;

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

    _audioSource = GetComponent<AudioSource>();
    if (_audioSource == null)
    {
      Debug.LogError("Sound Source is NULL");
    }

    _shieldColor = _shieldVisualPrefab.GetComponent<SpriteRenderer>();
    if (_shieldColor == null)
    {
      Debug.LogError("_Sprite Renderer is NULL");
    }

    _camera = GameObject.Find("Main Camera").GetComponent<Transform>();
    if (_camera == null)
    {
      Debug.LogError("Camera is null");
    }

    _playerMovementAnim = GetComponent<Animator>();
    if (_playerMovementAnim == null)
    {
      Debug.LogError("Player :: _playerMovementAnim is NULL");
    }

    _uiManager.UpdateAmmo(_currentAmmo);
    _uiManager.UpdateClip(_currentAmmoClip);

  }

  void Update()
  {
    PlayerMovement();
    SpeedBoostActive();

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
    transform.Translate(_modifiedSpeed * Time.deltaTime * direction);

    if (direction.x >= 0.2)
    {
      _playerMovementAnim.SetBool("PlayerLeft", false);
      _playerMovementAnim.SetBool("PlayerRight", true);
    }
    else if (direction.x <= -0.2)
    {
      _playerMovementAnim.SetBool("PlayerLeft", true);
      _playerMovementAnim.SetBool("PlayerRight", false);
    }
    else
    {
      _playerMovementAnim.SetBool("PlayerLeft", false);
      _playerMovementAnim.SetBool("PlayerRight", false);
    }

    //Boundries
    transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.7f, 4.7f), 0);
    if (transform.position.x > 11f)
    {
      transform.position = new Vector3(-11f, transform.position.y, 0);
    }
    else if (transform.position.x < -11f)
    {
      transform.position = new Vector3(11f, transform.position.y, 0);
    }

  }

  void PlayerShooting()
  {
    if (_systemsActive == true && _currentAmmo >= 1)
    {
      _canFire = Time.time + _fireRate;

      if (_tripleShotActive == true)
      {
        Instantiate(_TripleShotPrefab,
                    transform.position,
                    Quaternion.identity);
        _audioSource.clip = _LaserShotClip;
        _audioSource.Play();
      }
      else if (_spreadShotActive == true)
      {
        Instantiate(_SpreadShotPrefab,
                    transform.position,
                    Quaternion.identity);
        _audioSource.clip = _LaserShotClip;
        _audioSource.Play();
      }
      else if (_missileActive == true)
      {
        Instantiate(_missilePrefab,
                    transform.position,
                    Quaternion.identity);
        _audioSource.clip = _missileClip;
        _audioSource.Play();
      }
      else
      {
        Instantiate(_laserPrefab,
                    transform.position + new Vector3(0, 1f, 0),
                    transform.rotation);
        _audioSource.clip = _LaserShotClip;
        _audioSource.Play();
      }
        
      _currentAmmo--;
      _uiManager.UpdateAmmo(_currentAmmo);
      _uiManager.UpdateClip(_currentAmmoClip);
    }

    else if (_currentAmmo <= 0 && _currentAmmoClip >= 1 && _systemsActive == true)
    {
      _currentAmmoClip--;
      _uiManager.UpdateClip(_currentAmmoClip);
      _currentAmmo = 10;
      _uiManager.UpdateAmmo(_currentAmmo);
      _audioSource.clip = _ReloadClip;
      _audioSource.Play();
    }

    else
    {
      _audioSource.clip = _noAmmoClip;
      _audioSource.Play();
    }
  }

  public void addAmmo()
  {
    if (_currentAmmoClip <= 3)
    {
      _currentAmmoClip++;
      _uiManager.UpdateClip(_currentAmmoClip);
    }

    else
    {
      _currentAmmo = 10;
      _uiManager.UpdateAmmo(_currentAmmo);
    }
    _audioSource.clip = _ReloadClip;
    _audioSource.Play();
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
    if (Input.GetKey(KeyCode.LeftShift) && _fuelamount > 0 && _systemsActive == true)
    {
      _thrusters.SetActive(true);
      _modifiedSpeed = _playerBaseSpeed * _speedBoostMultiplier;
      _fuelamount -= Time.deltaTime * (_fuelBurnSpeed);
    }
    else if (_systemsActive == true)
    {
      _thrusters.SetActive(false);
      _modifiedSpeed = _playerBaseSpeed;
    }

    if (_fuelamount <= 0)
    {
      _fuelamount = 0;
    }

    _uiManager.UpdateFuel(_fuelamount * 0.01f);
  }

  public void AddFuel()
  {
    _fuelamount = 100;
    _uiManager.UpdateFuel(_fuelamount / 100);
  }

  public void Damage()
  {
    if (_shieldActive == true)
    {
      switch (_shieldHealth)
      {
        case 3:
          _shieldColor.material.color = new Color(1f, 1f, 0f, 1f);
          _shieldHealth--;
          break;

        case 2:
          _shieldColor.material.color = new Color(1f, 0f, 0f, 1f);
          _shieldHealth--;
          break;

        case 1:
          _shieldHealth = 0;
          _shieldActive = false;
          _shieldVisualPrefab.SetActive(false);
          break;

        default:
          Debug.Log("Player :: Damage :: _shieldActive switch Statement error");
          break;
      }
    }
    else
    {
      StartCoroutine(CameraShake());
      _lives--;
      updateEngines();
      _uiManager.UpdateLives(_lives);
    }

    if (_lives <= 0)
    {
      _spawnManager.OnPlayerDeath();
      Destroy(gameObject);
    }
  }

  public void ShieldActive()
  {
    _shieldActive = true;
    _shieldHealth = 3;
    _shieldColor.material.color = new Color(1f, 1f, 1f, 1f);
    _shieldVisualPrefab.SetActive(true);
  }

  public void AddScore(int points)
  {
    _score += points;
    _uiManager.UpdateScore(_score);
  }

  public void AddLives()
  {
    if (_lives < 3)
    {
      _lives++;
    }
    _uiManager.UpdateLives(_lives);
    updateEngines();
  }

  private void updateEngines()
  {
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
        //check damage function
        break;

      default:
        Debug.LogError("Lives at: " + _lives);
        break;
    }
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
    yield return new WaitForEndOfFrame();
    _camera.transform.localPosition = _startPos;
    _timePassed = 0;
  }

  public void SpreadShotActive()
  {
    _spreadShotActive = true;
    StartCoroutine(SpreadShotDowntime());
  }

  IEnumerator SpreadShotDowntime()
  {
    yield return new WaitForSeconds(_spreadShotCooldown);
    _spreadShotActive = false;
  }

  public void SystemsOffline()
  {
    _systemsActive = false;
    _modifiedSpeed = _playerBaseSpeed / _slowSpeed;
    StartCoroutine(SystemDowntime());
  }

  IEnumerator SystemDowntime()
  {
    yield return new WaitForSeconds(5f);
    _modifiedSpeed = _playerBaseSpeed;
    _systemsActive = true;
  }

  public void MissilesActive()
  {
    _missileActive = true;
    StartCoroutine(MissilesDowntime());
  }

  IEnumerator MissilesDowntime()
  {
    yield return new WaitForSeconds(_missileCooldown);
    _missileActive = false;
  }

  public void CollisionWithBoss()
  {
    _shieldHealth = 0;
    _shieldActive = false;
    _shieldVisualPrefab.SetActive(false);
    _lives = 0;
    _uiManager.UpdateLives(_lives);

    _spawnManager.OnPlayerDeath();
    Destroy(gameObject);
  }
}