using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Player : MonoBehaviour
{
  private float _playerSpeed = 10f;
  private float _fireRate = 0.2f;
  private float _canFire = -1f;
  [SerializeField]
  private GameObject _laserPrefab;

  void Start()
  {
    transform.position = new Vector3(0, 1, 0);
  }
  void Update()
  {
    PlayerMovement();
    if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
    {
      PlayerShooting();
    }

  }
  void PlayerMovement()
  {
    //Movement
    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");
    Vector3 direction = new(horizontalInput, verticalInput, 0);
    transform.Translate(_playerSpeed * Time.deltaTime * direction);

    //Boundries
    transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 0, 12f), 0);
    if (transform.position.x > 13.3f)
    {
      transform.position = new Vector3(-13.3f, transform.position.y, 0);
    }
    else if (transform.position.x < -13.3f)
    {
      transform.position = new Vector3(13.3f, transform.position.y, 0);
    }
  }
  void PlayerShooting()
  {
      _canFire = Time.time + _fireRate;
      Instantiate(_laserPrefab,
                  transform.position + new Vector3(0, 0.8f, 0),
                  transform.rotation);
  }
}