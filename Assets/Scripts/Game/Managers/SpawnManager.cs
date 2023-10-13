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

  [SerializeField]
  private GameObject _asteroidPrefab;
  private int _currentWave = 0;

  [SerializeField]
  private int _enemiesToSpawn = 0;
  private int _enemiesPerWave = 5;
  private int _enemiesInContainer;

  private UiManager _uiManager;

  private void Start()
  {
    _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
    if (_uiManager == null)
    {
      Debug.LogError("SpawnManager :: UI Manager is Null");
    }
  }

  public void StartSpawning()
  {
    _currentWave++;
    _uiManager.UpdateWaves(_currentWave);
    _stopSpawning = false;
    _enemiesToSpawn = _currentWave * _enemiesPerWave;

      StartCoroutine(SpawnEnemyRoutine());
      StartCoroutine(SpawnPowerupRoutine());

  }

  IEnumerator SpawnEnemyRoutine()
  {
    yield return new WaitForSeconds(2f);

    while (_stopSpawning == false && _enemiesToSpawn > 0)
    {
      Vector3 posToSpawn = new Vector3(Random.Range(-10f, 10f), 7f, 0f);
      GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
      newEnemy.transform.parent = _enemyContainer.transform;
      _enemiesToSpawn--;

      yield return new WaitForSeconds(_spawnTime);
    }

    _enemiesInContainer = _enemyContainer.transform.childCount;

    if (_stopSpawning == false && _enemiesToSpawn <=0 && _enemiesInContainer <= 0)
    {
      _stopSpawning = true;
      Vector3 _asteroidPos = new Vector3(0, 7, 0);
      GameObject newWave = Instantiate(_asteroidPrefab, _asteroidPos, Quaternion.identity);
      newWave.transform.parent = _enemyContainer.transform;
    }
  }

  IEnumerator SpawnPowerupRoutine()
  {
    yield return new WaitForSeconds(3f);
    while (_stopSpawning == false && _enemiesToSpawn > 0)
    {
      Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7f, 0f);

      powerupSelector();
      Instantiate(_powerups[_powerupSelected], posToSpawn, Quaternion.identity);
      
      yield return new WaitForSeconds(Random.Range(3, 8));
    }
  }

  public void OnPlayerDeath()
  {
    _stopSpawning = true;
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