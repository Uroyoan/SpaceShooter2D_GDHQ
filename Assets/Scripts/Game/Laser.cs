using UnityEngine;

public class Laser : MonoBehaviour
{
  [SerializeField]
  private float _laserSpeed = 14f;
  private bool _isEnemyLaser = false;


  void Update()
  {
    if (_isEnemyLaser == false)
    {
      PlayerLaserMovement();
    }
    else
    {
      EnemyLaserMovement();
    }
  }

  void PlayerLaserMovement()
  {
    //Movement
    transform.Translate(_laserSpeed * Time.deltaTime * Vector3.up);

    //Boundries
    if (transform.position.x > 13.3f || transform.position.x < -13.3f)
    {
      if (transform.parent != null)
      {
        Destroy(transform.parent.gameObject);
      }
      Destroy(this.gameObject);
    }
    else if (transform.position.y > 10f || transform.position.y < -10f)
    {
      if (transform.parent != null)
      {
        Destroy(transform.parent.gameObject);
      }
      Destroy(this.gameObject);
    }
  }

  void EnemyLaserMovement()
  {
    //Movement
    transform.Translate(_laserSpeed * Time.deltaTime * Vector3.down);

    //Boundries
    if (transform.position.x > 13.3f || transform.position.x < -13.3f)
    {
      if (transform.parent != null)
      {
        Destroy(transform.parent.gameObject);
      }
      Destroy(this.gameObject);
    }
    else if (transform.position.y > 10f || transform.position.y < -10f)
    {
      if (transform.parent != null)
      {
        Destroy(transform.parent.gameObject);
      }
      Destroy(this.gameObject);
    }
  }

  public void AssignEnemyLaser()
  {
    _isEnemyLaser = true;
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Player" && _isEnemyLaser == true)
    {
      Player player = other.GetComponent<Player>();

      if (player != null)
      {
        player.Damage();
      }
    }
  }

}