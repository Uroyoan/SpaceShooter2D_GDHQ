using System.Collections;
using System.Collections.Generic;
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
  void Start()
  {
    _player = GameObject.Find("Player").GetComponent<Player>();
    _audioSource = GetComponent<AudioSource>();
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
    transform.Translate(_enemySpeed * Time.deltaTime * Vector3.down);

    //Boundries
    float newPosition = Random.Range(-11, 11);
    if (transform.position.y > 7.5f)
    {
      transform.position = new Vector3(newPosition, -7.5f, 0);
    }
    else if (transform.position.y < -7.5f)
    {
      transform.position = new Vector3(newPosition, 7.5f, 0);
    }
  }

  void EnemyShooting()
  {
    if (Time.time > _canFire)
    {
      _FireRate = Random.Range(3f, 7f);
      _canFire = Time.time + _FireRate;

      if (_enemyIsDead == false)
      {
        GameObject enemyLaser = Instantiate(_enemyLaserPrefab,
                                    transform.position,
                                    Quaternion.identity);
        Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
        for (int i = 0; i < lasers.Length; i++)
        {
          lasers[i].AssignEnemyLaser();
        }
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  { //check collided objects tag
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

  void OndeathAnimation()
  {
    _enemyIsDead = true;
    _enemySpeed = 0;
    _deathAnim.SetTrigger("OnEnemyDeath");

    _audioSource.Play();
    Destroy(GetComponent<Collider2D>());
    Destroy(gameObject, 2.75f);
  }
}