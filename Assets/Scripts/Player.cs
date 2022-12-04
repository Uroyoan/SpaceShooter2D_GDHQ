using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Player : MonoBehaviour
{
  public float playerSpeed = 15f;

  // Start is called before the first frame update
  void Start()
  {
    transform.position = new Vector3(0, 1, 0);
  }

  // Update is called once per frame
  void Update()
  {
    PlayerMovement();
  }
  void PlayerMovement()
  {
    //Movement
    float horizontalInput = Input.GetAxis("Horizontal");
    float verticalInput = Input.GetAxis("Vertical");
    Vector3 direction = new(horizontalInput, verticalInput, 0);
    transform.Translate(playerSpeed * Time.deltaTime * direction);

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
}