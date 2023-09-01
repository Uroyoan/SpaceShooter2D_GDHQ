using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Powerup : MonoBehaviour
{

  private float _powerupSpeed = 3f;

  void Update()
  {
    PowerupMovement();
  }

  private void PowerupMovement()
  {
    //Speed and direction
    transform.Translate(_powerupSpeed * Time.deltaTime * Vector3.down);

    //Boundries
    if (transform.position.y > 7.5f)
    {
      Destroy(this.gameObject);
    }
    else if (transform.position.y < -7.5f)
    {
      Destroy(this.gameObject);
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    switch (other.tag)
    {
      case "Player":
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
          player.TripleShotActive();
        }
        Destroy(this.gameObject);
        break;

      default:
        Debug.Log(other.name);
        break;
    }
  }
}
