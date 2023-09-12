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
          _player.AddScore(Random.Range(1,11));
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
    _enemySpeed = 0;
    _deathAnim.SetTrigger("OnEnemyDeath");

    _audioSource.Play();
    Destroy(gameObject, 2.75f);
  }  
}