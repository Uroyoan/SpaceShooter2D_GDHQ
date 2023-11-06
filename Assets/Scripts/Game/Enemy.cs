using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
  private float _enemySpeed = 7f;

  private Player _player;

  Animator _deathAnim;

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
    //Movement Direction
    if (Time.time > _sporadicMovementTimer)
    {
      switch (gameObject.tag)
      {
        case "Enemy":
          _direction.x = Random.Range(-1, 2);
          _sporadicMovementTimer = Time.time + 3f;
          break;

        case "Enemy_Rammer":
          AggressiveEnemyMovement();
          _sporadicMovementTimer = Time.time + 1.25f;
          break;
        case "Enemy_Smart":
          _direction.x = Random.Range(-1, 2);
          _sporadicMovementTimer = Time.time + 1f;
          break;
        default:
          break;
      }
    }

    //Movement
    transform.Translate(_enemySpeed * Time.deltaTime * _direction);

    // y boundaries
    float newXPosition = Random.Range(-11, 11);
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
    if (Time.time > _canFire)
    {
      _canFire = Time.time + _FireRate;
      if (_enemyIsDead == false)
      {
        switch (gameObject.tag)
        {
          case "Enemy" or "Enemy_Rammer":
            _FireRate = Random.Range(3f, 7f);
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab,
                                                transform.position,
                                                Quaternion.identity);
            Laser[] Basiclasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < Basiclasers.Length; i++)
            {
              Basiclasers[i].AssignEnemyLaser();
            }
            break;

          case "Enemy_Smart":
            _FireRate = Random.Range(1.5f, 3f);
            SmartEnemyLaser();
            break;

          default:
            break;
        }

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

        case "Laser":
          Destroy(other.gameObject);

          if (_player != null)
          {
            _player.AddScore(Random.Range(1, 11));
          }
          OndeathAnimation();


          break;

        default:
          Debug.Log("Hit by: " + other.tag);
          break;
      }
    }
    else
    {
      if (other.tag == "Laser")
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
    _playerLocation = _player.gameObject.transform.position;
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

  public void SmartEnemyMovement()
  {
    //Future Movmenent Enemy
  }

  public void SmartEnemyLaser()
  {
    _enemyLocation = gameObject.transform.position;
    _playerLocation = _player.gameObject.transform.position;
    _enemyProximity = _enemyLocation.x - _playerLocation.x;
    if (_enemyProximity < 3 && _enemyProximity > -3)
    {
      if (_enemyLocation.y > _playerLocation.y)
      {
        _laserRotation = Quaternion.identity;
      }
      if (_enemyLocation.y < _playerLocation.y)
      {
        _laserRotation = Quaternion.identity;
        _laserRotation.z = 180;
      }
      GameObject smartEnemyLaser = Instantiate(_enemyLaserPrefab,
                                               transform.localPosition,
                                               _laserRotation);
      Laser[] smartLasers = smartEnemyLaser.GetComponentsInChildren<Laser>();
      for (int i = 0; i < smartLasers.Length; i++)
      {
        smartLasers[i].AssignEnemyLaser();
      }
    }
  }

}