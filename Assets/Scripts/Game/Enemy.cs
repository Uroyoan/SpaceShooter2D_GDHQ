using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
  private float _enemySpeed = 7f;

  private Player _player;

  void Start()
  {
    _player = GameObject.Find("Player").GetComponent<Player>();
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
        Destroy(gameObject);
        break;

      case "Laser":
        Destroy(other.gameObject);
        if (_player != null)
        {
          _player.AddScore(Random.Range(1,11));
        }
        Destroy(gameObject);
        break;

      default:
        Debug.Log("Hit by: " + other.tag);
        break;
    }
  }
}