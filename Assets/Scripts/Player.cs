using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Player : MonoBehaviour
{

  private float _playerSpeed = 10f;
  private float _fireRate = 0.2f;
  private float _canFire = -1f;

  [SerializeField]
  private GameObject _laserPrefab;
  [SerializeField]
  private GameObject _TripleShotPrefab;
  [SerializeField]
  private bool _TripleShotActive = false;
  private int _lives = 3;
  private SpawnManager _spawnManager;
  private float _tripleShotCooldown = 5f;

  void Start()
  {
    transform.position = new Vector3(0, -4.4f, 0);
    _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    if (_spawnManager == null)
    {
      Debug.LogError("Spawn Manager is NULL");
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
    if (_TripleShotActive == true)
    {
      Instantiate(_TripleShotPrefab,
                  transform.position,
                  Quaternion.identity);
    }
    else
    {
      Instantiate(_laserPrefab,
                  transform.position + new Vector3(0, 1.4f, 0),
                  transform.rotation);
    }
  }

  public void Damage()
  {
    _lives--;
    if (_lives < 1)
    {
      _spawnManager.OnPlayerDeath();
      Destroy(gameObject);
    }
  }

  public void TripleShotActive()
  {
    _TripleShotActive = true;
    StartCoroutine(TripleShotDowntime());
  }

  IEnumerator TripleShotDowntime()
  {
    yield return new WaitForSeconds(_tripleShotCooldown);
    _TripleShotActive = false;
  }
}