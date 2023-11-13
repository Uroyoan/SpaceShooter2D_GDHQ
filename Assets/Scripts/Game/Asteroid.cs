using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

  private float _rotationSpeed = 20f;
  private float _movementSpeed = 1f;
  [SerializeField]
  private GameObject _explosionPrefab;
  private SpawnManager _spawnManager;

  void Start()
  {
    _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    if (_spawnManager == null)
    {
      Debug.LogError("_spawnManager is null");
    }

  }

  void Update()
  {
    AsteroidMovement();
  }

  void AsteroidMovement()
  {
    Vector3 currentPos = transform.position;
    transform.Rotate (Vector3.forward * _rotationSpeed * Time.deltaTime);
    if (currentPos.y >= 4)
    {
      transform.Translate(Vector3.down * _movementSpeed * Time.deltaTime ,Space.World);
    }
    else
    {
      transform.Translate(Vector3.down * 0 * Time.deltaTime);
    }
    
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Laser_Player" || other.tag == "Missile_Player")
    {
      Destroy(other.gameObject);
      Instantiate(_explosionPrefab,
                  transform.position,
                  Quaternion.identity);

      _spawnManager.StartSpawning();
      Destroy(gameObject, 0.1f);
    }
  }
}
