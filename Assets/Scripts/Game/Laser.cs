using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
  [SerializeField]
  private float _laserSpeed = 14f;

  void Update()
  {
    LaserMovement();
  }

  void LaserMovement()
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

}