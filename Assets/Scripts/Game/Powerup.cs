using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Powerup : MonoBehaviour
{

  private float _powerupSpeed = 3f;
  [SerializeField]
  private int _powerupID; // Check function for powerups
  [SerializeField]
  private AudioClip _clip;

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
        AudioSource.PlayClipAtPoint(_clip, transform.position);

        if (player != null)
        {
          switch (_powerupID)
          {
            case 0: // Triple Shot
              player.TripleShotActive();
              break;

            case 1: // Speed Boost
              player.AddFuel();
              break;

            case 2: // Shields
              player.ShieldActive();
              break;

            case 3: // Ammo
              player.addAmmo();
              break;

            case 4: // Lives
              player.AddLives();
              break;

            case 5: // SpreadShot
              player.SpreadShotActive();
              break;

            case 6: // Ion Field (Negative Power-up)
              player.SystemsOffline();
              break;

            default:
              Debug.Log("powerupID ERROR?");
              break;
          }
        }
        Destroy(this.gameObject);
        break;

      default:
        //Debug.Log(other.name);
        break;
    }
  }
}
