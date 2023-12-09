using UnityEngine;

public class Powerup : MonoBehaviour
{

  private float _powerupSpeed = 3f;
  [SerializeField]
  private int _powerupID; // Check function for powerups
  [SerializeField]
  private AudioClip _clip;
  private Player _player;

  private void Start()
  {
    _player = GameObject.Find("Player").GetComponent<Player>();
    if (_player == null)
    {
      Debug.LogError("Powerup :: _player is NULL");
    }
  }

  void Update()
  {
    PowerupMovement();

    if (Input.GetKey(KeyCode.C))
    {
      PickupCollect();
    }
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

  private void PickupCollect()
  {
    Vector3 playerPos = _player.transform.position;
    Vector3 powerupPos = transform.position;

    if (powerupPos.y > playerPos.y)
    {
      transform.Translate(_powerupSpeed * Time.deltaTime * Vector3.down);
    }
    else
    {
      transform.Translate(_powerupSpeed * Time.deltaTime * (Vector3.up * 2));
    }

    if (powerupPos.x > playerPos.x)
    {
      transform.Translate(_powerupSpeed * Time.deltaTime * Vector3.left);
    }
    else
    {
      transform.Translate(_powerupSpeed * Time.deltaTime * Vector3.right);
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
            case 0: // Ammo
              player.addAmmo();
              break;

            case 1: // Speed Boost
              player.AddFuel();
              break;

            case 2: // Triple Shot
              player.TripleShotActive();
              break;

            case 3: // Spread Shot
              player.SpreadShotActive();
              break;

            case 4: // Lives
              player.AddLives();
              break;

            case 5: // Shields
              player.ShieldActive();
              break;

            case 6: // Ion Field (Negative Power-up)
              player.SystemsOffline();
              break;

            case 7: // Missiles
              player.MissilesActive();
              break;

            default:
              Debug.Log("powerupID ERROR?");
              break;
          }
        }
        Destroy(this.gameObject);
        break;

      case "Laser_Enemy" or "Missile_Enemy":
        Destroy(other.gameObject);
        Destroy(this.gameObject);
        break;

      case "Enemy_Boss":
        Destroy(this.gameObject);
        break;

      default:
        //Debug.Log(other.name);
        break;
    }
  }
}
