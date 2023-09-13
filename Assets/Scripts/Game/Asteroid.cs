using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

  private float _rotationSpeed = 20f;
  //private float _movementSpeed = 0f;

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
    transform.Rotate(Vector3.forward * _rotationSpeed * Time.deltaTime);
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Laser")
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
