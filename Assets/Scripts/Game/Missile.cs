using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
  private Transform _enemyShip;
  private Transform _playerShip;

  Vector3 rightTriangle;
  private float _xValueofShip;
  private float _yValueofShip;
  private float _radians;
  private float _angle;

  private float _speed = 12f;

  private Collider2D _collider;

  private void Start()
  {
    _enemyShip = GameObject.Find("EnemyContainer").transform.GetChild(0);
    if (_enemyShip == null)
    {
      Debug.LogError("Missile::_enemyShip IS NULL");
    }

    _playerShip = GameObject.Find("Player").transform;
    if (_playerShip == null)
    {
      Debug.LogError("Missile::_playerShip IS NULL");
    }

    StartCoroutine(CollisionOFF());

  }

  private void Update()
  {
    CalculateMovement();
  }
  
  private void FindAngle(Vector3 ship)
  {
    rightTriangle = ship - transform.position;
    _xValueofShip = rightTriangle.x;
    _yValueofShip = rightTriangle.y;

    _radians = (float)Math.Atan2(_yValueofShip, _xValueofShip);
    _angle = _radians * (180 / 3.1415f);
  }

  private void LockedOnEnemy()
  {
    if (_enemyShip != null)
    {
      FindAngle(_enemyShip.position);
      gameObject.transform.rotation = Quaternion.Euler(0, 0, (_angle -= 90));
    }
  }

  private void LockedOnPlayer()
  {
    FindAngle(_playerShip.position);
    gameObject.transform.rotation = Quaternion.Euler(0, 0, (_angle -= 90));
  }

  private void CalculateMovement()
  {

    transform.Translate(new Vector3(0, 1f, 0) * _speed * Time.deltaTime);

    switch (this.tag)
    {
      case "Missile_Player":
        LockedOnEnemy();
        break;

      case "Missile_Enemy":
        LockedOnPlayer();
        break;

      default:
        Debug.Log("Missile :: CalculateMovement Error, Tag Found: " + this.tag);
        break;
    }

    // Boundries
    if (transform.position.x > 13.3f || transform.position.x < -13.3f)
    {
      Destroy(this.gameObject);
    }
    else if (transform.position.y > 10f || transform.position.y < -10f)
    {
      Destroy(this.gameObject);
    }
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Player")
    {
      Player player = other.GetComponent<Player>();

      if (player != null)
      {
        player.Damage();
        Destroy(this.gameObject);
      }
    }
  }

  IEnumerator CollisionOFF()
  {
    _collider = GetComponent<Collider2D>();
    _collider.enabled = false;
    yield return new WaitForSeconds(1f);
    _collider.enabled = true;
  }
}