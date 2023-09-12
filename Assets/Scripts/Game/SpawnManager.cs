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

	IEnumerator SpawnEnemyRoutine()
	{
    yield return new WaitForSeconds(3f);
    while (_stopSpawning == false)
		{

			Vector3 posToSpawn = new Vector3(Random.Range(-10f, 10f), 8f, 0f);
			GameObject newEnemy = Instantiate(_enemyPrefab,posToSpawn,Quaternion.identity);
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
			int _randomPowerup = Random.Range(0,3);
			Instantiate(_powerups[_randomPowerup], posToSpawn, Quaternion.identity);
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
}