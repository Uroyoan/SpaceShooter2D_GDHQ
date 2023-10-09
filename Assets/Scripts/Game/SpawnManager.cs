using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
  //Variables
  float _spawnTime = 5f;
  private bool _stopSpawning = false;

  [SerializeField]
  private GameObject _enemyPrefab;
  [SerializeField]
  private GameObject _enemyContainer;
  [SerializeField]
  private GameObject[] _powerups;
  private int _powerupRandomizer;
  private int _powerupSelected;

  IEnumerator SpawnEnemyRoutine()
  {
    yield return new WaitForSeconds(3f);
    while (_stopSpawning == false)
    {

      Vector3 posToSpawn = new Vector3(Random.Range(-10f, 10f), 7f, 0f);
      GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
      newEnemy.transform.parent = _enemyContainer.transform;

      yield return new WaitForSeconds(_spawnTime);
    }
  }

  IEnumerator SpawnPowerupRoutine()
  {
    yield return new WaitForSeconds(3f);
    while (_stopSpawning == false)
    {
      Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7f, 0f);
      powerupSelector();

      switch (_powerupSelected)
      {
        case 0: // TripleShot
          Instantiate(_powerups[_powerupSelected], posToSpawn, Quaternion.identity);
          break;

        case 1: // Speed
          Instantiate(_powerups[_powerupSelected], posToSpawn, Quaternion.identity);
          break;

        case 2: // Shield
          Instantiate(_powerups[_powerupSelected], posToSpawn, Quaternion.identity);
          break;

        case 3: // Ammo
          Instantiate(_powerups[_powerupSelected], posToSpawn, Quaternion.identity);
          break;

        case 4: // Life
          Instantiate(_powerups[_powerupSelected], posToSpawn, Quaternion.identity);
          break;

        case 5: // Spread
          Instantiate(_powerups[_powerupSelected], posToSpawn, Quaternion.identity);
          break;
      }
      yield return new WaitForSeconds(Random.Range(3, 8));
    }
  }

  public void OnPlayerDeath()
  {
    _stopSpawning = true;
  }

  public void StartSpawning()
  {
    StartCoroutine(SpawnEnemyRoutine());
    StartCoroutine(SpawnPowerupRoutine());
  }

  private void powerupSelector()
  {
    _powerupRandomizer = Random.Range(0, 101);

    if (_powerupRandomizer < 10) //Triple Shot
    {
      _powerupSelected = 0;
    }
    else if (_powerupRandomizer >= 10 && _powerupRandomizer < 39) // Speed
    {
      _powerupSelected = 1;
    }
    else if (_powerupRandomizer >= 40 && _powerupRandomizer < 49) // Shield
    {
      _powerupSelected = 2;
    }
    else if (_powerupRandomizer >= 50 && _powerupRandomizer < 79) // Ammo
    {
      _powerupSelected = 3;
    }
    else if (_powerupRandomizer >= 80 && _powerupRandomizer < 89) // Life
    {
      _powerupSelected = 4;
    }
    else if (_powerupRandomizer >= 90) // Spread
    {
      _powerupSelected = 5;
    }
  }
}