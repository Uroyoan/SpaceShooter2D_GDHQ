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

  void Start()
		{
			StartCoroutine(SpawnRoutine());
		}

		void Update()
		{
				
		}

	IEnumerator SpawnRoutine()
	{
		while (_stopSpawning == false)
		{

			Vector3 posToSpawn = new Vector3(Random.Range(-10f, 10f), 8f, 0f);
			GameObject newEnemy = Instantiate(_enemyPrefab,posToSpawn,Quaternion.identity);
			newEnemy.transform.parent = _enemyContainer.transform;

      yield return new WaitForSeconds(_spawnTime);
    }
	}

	public void OnPlayerDeath()
	{
		_stopSpawning = true;
	}
}