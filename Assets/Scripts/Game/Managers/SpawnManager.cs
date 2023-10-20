using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
  //Variables
  private UiManager _uiManager;

  float _spawnTime = 5f;
  private bool _stopSpawning = false;

  [SerializeField]
  private GameObject[] _enemyPrefab;
  [SerializeField]
  private GameObject _enemyContainer;
  [SerializeField]
  private int _enemiesToSpawn = 0;
  private int _enemiesPerWave = 5;
  private int _enemiesInContainer;

  [SerializeField]
  private int _enemySelected;
  private int _enemyTotalPercentage;
  private int _enemyRandomNumber;
  private int _enemyCompareNumber = 0;
  private int[] _enemyDropTable =
                {
                  100  // Basic Enemy = 0 to 100
                };

  [SerializeField]
  private GameObject _asteroidPrefab;
  private int _currentWave = 0;

  [SerializeField]
  private GameObject[] _powerups;
  private int _powerupSelected;
  private int _powerupTotalPercentage;
  private int _powerupRandomNumber;
  private int _powerupCompareNumber = 0;
  private int[] _powerupDroptable =
                {
                  20, // Ammo = 1 to 20
                  20, // Speed = 21 to 40
                  15, // Triple Shot = 41 to 45
                  15, // Spread Shot = 56 to 70
                  15, // Life = 71 to 85
                  10, // Shield = 86 to 95
                  5,  // Ion Field = 96 to 100
                };

  private void Start()
  {

    foreach (var powerup in _powerupDroptable)
    {
      _powerupTotalPercentage += powerup;
    }
    if (_powerupTotalPercentage != 100)
    {
      Debug.LogError("SpawnManager ::" +
        "             Percentage of Powerup (" + _powerupTotalPercentage + ") is not equal to 100%");
    }

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
      GameObject newEnemy = Instantiate(_enemyPrefab[_enemySelected], posToSpawn, Quaternion.identity);
      newEnemy.transform.parent = _enemyContainer.transform;
      _enemiesToSpawn--;

      yield return new WaitForSeconds(_spawnTime);
    }

    _enemiesInContainer = _enemyContainer.transform.childCount;
    while (_enemiesInContainer >= 1)
    {
      _enemiesInContainer = _enemyContainer.transform.childCount;
      yield return new WaitForSeconds(1f);
    }

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

      PowerupSelector();
      Instantiate(_powerups[_powerupSelected], posToSpawn, Quaternion.identity);
      
      yield return new WaitForSeconds(Random.Range(3, 8));
    }
  }

  public void OnPlayerDeath()
  {
    _stopSpawning = true;
  }

  private void PowerupSelector()
  {
    _powerupRandomNumber = Random.Range(1, _powerupTotalPercentage + 1);
    _powerupCompareNumber = 0;

      for (int i = 0; i < _powerupDroptable.Length; i++)
      {
        _powerupCompareNumber += _powerupDroptable[i];

        if (_powerupCompareNumber >= _powerupRandomNumber)
        { 
          _powerupSelected = i;
          return;
        }
      }
  }

  private void EnemySelector()
  {
    _enemyRandomNumber = Random.Range(1, _enemyTotalPercentage + 1);
    _enemyCompareNumber = 0;

    for (int i = 0; i < _enemyDropTable.Length; i++)
    {
      _enemyCompareNumber += _enemyDropTable[i];

      if (_enemyCompareNumber >= _enemyRandomNumber)
      {
        _enemySelected = i;
        return;
      }
    }
  }
}