using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
  private float _enemySpeed = 7f;

  void Start()
  {

  }
  void Update()
  {
    EnemyMovement();
  }

  private void OnTriggerEnter(Collider other)
  { //check collided objects tag
    switch (other.tag)
    {
      case "Player":
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
          player.Damage();
        }
        Destroy(gameObject);
        break;

      case "Laser":
        Destroy(other.gameObject);
        Destroy(gameObject);
        break;

      default:
        Debug.Log("Hit by: " + other.tag);
        break;
    }
  }

  void EnemyMovement()
  {
    //Movement
    Vector3 direction = new(0, -1, 0);
    transform.Translate(_enemySpeed * Time.deltaTime * direction);

    //Boundries
    float newPosition = Random.Range(-11,11);
    if (transform.position.y > 13.6f)
    {
      transform.position = new Vector3(newPosition, -1.5f, 0);
    }
    else if (transform.position.y < -1.6f)
    {
      transform.position = new Vector3(newPosition, 13.5f, 0);
    }
  }

}