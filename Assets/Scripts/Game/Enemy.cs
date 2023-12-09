using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
  private float _enemySpeed = 7f;

  private Player _player;

  private Animator _deathAnim;

  private AudioSource _audioSource;

  [SerializeField]
  private GameObject _enemyLaserPrefab;
  private float _FireRate = 3f;
  private float _canFire = -1f;
  private bool _enemyIsDead = false;

  private Vector3 _direction = new Vector3(0, -1, 0);
  private float _sporadicMovementTimer = -1;

  [SerializeField]
  private GameObject _enemyShieldVisualPrefab;
  private bool _isShieldActive = true;

  private Vector3 _enemyLocation;
  private Vector3 _playerLocation;
  private float _enemyProximity;

  private Quaternion _laserRotation;
  [SerializeField]
  private LayerMask _layerMask;
  private bool _shotLaser = false;
  private bool _detectedLaser = false;

  [SerializeField]
  private GameObject _enemyMissile;
  [SerializeField]
  private AudioClip _missileLockOnClip;
  private Quaternion _missileRotation;

  private Vector3 _rightTriangle;
  private float _xValueofShip;
  private float _yValueofShip;
  private float _radians;
  private float _angle;
  [SerializeField]
  private Transform _missileLocation;

  void Start()
  {
    if (this.gameObject.tag != "Enemy_Rammer")
    {
      DeactivateShield();
    }

    _player = GameObject.Find("Player").GetComponent<Player>();
    if (_player == null)
    {
      Debug.LogError("_player is NULL");
    }

    _deathAnim = GetComponent<Animator>();
    if (_deathAnim == null)
    {
      Debug.LogError("_deathAnim is NULL");
    }

    _audioSource = GetComponent<AudioSource>();
    if (_audioSource == null)
    {
      Debug.LogError("_audioSource is NULL");
    }
  }

  void Update()
  {
    EnemyMovement();
    EnemyShooting();
  }

  void EnemyMovement()
  {
    //Movement
    transform.Translate(_enemySpeed * Time.deltaTime * _direction);
   
    //Movement Direction
    if (Time.time > _sporadicMovementTimer)
    {
      switch (gameObject.tag)
      {
        case "Enemy":
          _sporadicMovementTimer = Time.time + 3f;
          _direction.x = UnityEngine.Random.Range(-1, 2);
          break;

        case "Enemy_Rammer":
          _sporadicMovementTimer = Time.time + 1.25f;
          AggressiveEnemyMovement();
          break;

        case "Enemy_Smart":
          SmartEnemyMovement();
          break;

        case "Enemy_HeatSeeker":
          if (transform.position.y <= 4)
          {
            _enemySpeed = 0;
            LookAtPlayer();
          }
          break;

        default:
          break;
      }
    }

    // y boundaries
    float newXPosition = UnityEngine.Random.Range(-11, 11);
    if (transform.position.y > 7.5f)
    {
      transform.position = new Vector3(newXPosition, -7.5f, 0);
    }
    else if (transform.position.y < -7.5f)
    {
      transform.position = new Vector3(newXPosition, 7.5f, 0);
    }

    // x Boundaries
    else if (transform.position.x > 11f)
    {
      transform.position = new Vector3(-11f, transform.position.y, 0);
    }
    else if (transform.position.x < -11f)
    {
      transform.position = new Vector3(11f, transform.position.y, 0);
    }
  }

  void EnemyShooting()
  {
    if (_enemyIsDead == false)
    {
      switch (gameObject.tag)
      {
        case "Enemy" or "Enemy_Rammer":
          if (Time.time > _canFire)
          {
            _FireRate = UnityEngine.Random.Range(3f, 7f);
            _canFire = Time.time + _FireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab,
                                                transform.position,
                                                Quaternion.identity);
            Laser[] Basiclasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < Basiclasers.Length; i++)
            {
              Basiclasers[i].AssignEnemyLaser();
            }
          }
          break;

        case "Enemy_Smart":
          if (Time.time > _canFire)
          {
            if (_shotLaser == false)
            {
              CheckObject();
            }
          }
          break;

        case "Enemy_HeatSeeker":
          if (Time.time > _canFire)
          {
            StartCoroutine(MissileCooldown());
          }
          break;

        default:
          break;
      }

    }

  }

  private void OnTriggerEnter2D(Collider2D other)
  { //check collided objects tag
    if (_isShieldActive == false)
    {
      switch (other.tag)
      {
        case "Player":
          if (_player != null)
          {
            _player.Damage();
          }
          OndeathAnimation();
          break;

        case "Laser_Player" or "Laser_Enemy" or "Missile_Player" or "Missile_Enemy":
          Destroy(other.gameObject);

          if (_player != null)
          {
            _player.AddScore(UnityEngine.Random.Range(1, 11));
          }
          OndeathAnimation();
          break;

        default:
          //Debug.Log("Hit by: " + other.tag);
          break;
      }
    }

    else
    {
      if (other.tag == "Laser_Player" || other.tag == "Laser_Enemy"||
          other.tag == "Missile_Player" || other.tag == "Missile_Enemy")
      {
        Destroy(other.gameObject);
        DeactivateShield();
      }
      else if (other.tag == "Player")
      {
        _player.Damage();
        DeactivateShield();
      }
    }
  }

  void OndeathAnimation()
  {
    _enemyIsDead = true;
    _enemySpeed = 0;
    _deathAnim.SetTrigger("OnEnemyDeath");

    _audioSource.Play();
    Destroy(GetComponent<Collider2D>());
    Destroy(gameObject, 2.70f);
  }

  public void DeactivateShield()
  {
    _isShieldActive = false;
    _enemyShieldVisualPrefab.SetActive(false);
  }

  public void AggressiveEnemyMovement()
  {
    _enemyLocation = gameObject.transform.position;
    if (_player != null)
    {
      _playerLocation = _player.gameObject.transform.position;
    }
    _enemyProximity = _enemyLocation.x - _playerLocation.x;
    if (_isShieldActive == true)
    {
      if (_enemyProximity < 4 && _enemyProximity > 0)
      {
        _direction.x = -1;
      }
      else if (_enemyProximity > -4 && _enemyProximity < 0)
      {
        _direction.x = 1;
      }
    }
  }

  private void CheckObject()
  {
    RaycastHit2D objectDetected = Physics2D.Raycast(transform.localPosition, -transform.up,
                                                    10, _layerMask);
    RaycastHit2D objectDetectedBack = Physics2D.Raycast(transform.localPosition, transform.up,
                                                        10, _layerMask);
    if (objectDetected)
    {
      if (objectDetected.collider.tag == "Power_Up" || objectDetected.collider.tag == "Player")
      {
        _laserRotation = Quaternion.identity;
        SmartEnemyLaser();
        StartCoroutine(LaserCooldown());
      }
    }

    else if (objectDetectedBack)
    {
      if (objectDetectedBack.collider.name == "Player")
      {
        _laserRotation = Quaternion.identity;
        _laserRotation.z += 180;
        SmartEnemyLaser();
        StartCoroutine(LaserCooldown());
      }
    }
  }

  public void SmartEnemyLaser()
  {
    GameObject smartEnemyLaser = Instantiate(_enemyLaserPrefab,
                                             transform.localPosition,
                                             _laserRotation);
    Laser[] smartLasers = smartEnemyLaser.GetComponentsInChildren<Laser>();
    for (int i = 0; i < smartLasers.Length; i++)
    {
      smartLasers[i].AssignEnemyLaser();
    }
  }

  IEnumerator LaserCooldown()
  {
    _shotLaser = true;
    _FireRate = UnityEngine.Random.Range(2f, 4f);
    _canFire = Time.time + _FireRate;

    yield return new WaitForSeconds(2f);
    _shotLaser = false;
  }

  public void SmartEnemyMovement()
  {
    if (_detectedLaser == true)
    {
      _direction.x = 1;
      StartCoroutine(MovementCooldown());
    }
  }

  public void DetectedLaser()
  {
    _detectedLaser = true;
  }

  IEnumerator MovementCooldown()
  {
    yield return new WaitForSeconds(1f);
    _detectedLaser = false;
    _direction.x = 0;
  }

  public void EnemyMissile()
  {
    _missileRotation = transform.localRotation;
    _missileRotation.z += 180;
    GameObject EnemyMissiles = Instantiate(_enemyMissile,
                                           _missileLocation.position,
                                           _missileRotation);
  }

  IEnumerator MissileCooldown()
  {
    _FireRate = 6f;
    _canFire = Time.time + _FireRate;
    _audioSource.clip = _missileLockOnClip;
    _audioSource.Play();
    yield return new WaitForSeconds(5f);
    if (_enemyIsDead == false)
    {
      EnemyMissile();
    }
  }

  private void LookAtPlayer()
  {
    _enemyLocation = gameObject.transform.position;
    _playerLocation = _player.gameObject.transform.position;
    _rightTriangle = _playerLocation - _enemyLocation;

    _xValueofShip = _rightTriangle.x;
    _yValueofShip = _rightTriangle.y;
    _radians = (float)Math.Atan2(_yValueofShip, _xValueofShip);
    _angle = _radians * (180 / 3.1415f);

    gameObject.transform.rotation = Quaternion.Euler(0, 0, (_angle += 90));
  }
}