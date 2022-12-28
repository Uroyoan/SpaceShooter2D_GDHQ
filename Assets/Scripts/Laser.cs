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
  //                                                                         |
  void LaserMovement()
  {
    //Movement
    transform.Translate(_laserSpeed * Time.deltaTime * Vector3.up);
    //Boundries
    if (transform.position.x > 13.3f || transform.position.x < -13.3f)
    {
      Debug.Log("destroyed X");
      Destroy(this.gameObject);
    }
    else if (transform.position.y > 13.3f || transform.position.y < -13.3f)
    {
      Debug.Log("destroyed Y");
      Destroy(this.gameObject);
    }
  }
    

















}