using System;
using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
  private Transform _playerTransform;
  [SerializeField]
  private GameObject _enemyLaserPrefab;

  private float _fireRate = 1f,
                _canFire = -1f;
  private Quaternion _laserRotation;

  private Vector3 _enemyLocation,
                  _playerLocation,
                  _rightTriangle;
  private float _xValueofShip,
                _yValueofShip,
                _radians,
                _angle;

  private void Start()
  {
    _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    if (_playerTransform == null)
    {
      Debug.LogError("Turret :: _playerTransform is NULL");
    }
  }

  private void Update()
  {
    if (_playerTransform != null)
    {
      LookAtPlayer();
      StartCoroutine(TurretShooting());
    }
  }

  private void LookAtPlayer()
  {
    _enemyLocation = gameObject.transform.position;
    _playerLocation = _playerTransform.gameObject.transform.position;
    _rightTriangle = _playerLocation - _enemyLocation;
    _xValueofShip = _rightTriangle.x;
    _yValueofShip = _rightTriangle.y;

    _radians = (float)Math.Atan2(_yValueofShip, _xValueofShip);
    _angle = _radians * (180 / 3.1415f);
    gameObject.transform.rotation = Quaternion.Euler(0, 0, (_angle + 180));
  }

  IEnumerator TurretShooting()
  {
    if (Time.time > _canFire)
    {
      _canFire = Time.time + _fireRate;
      yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 2f));

      _laserRotation = gameObject.transform.rotation;
      Vector3 rotation = _laserRotation.eulerAngles;
      rotation.z -= 90;
      _laserRotation = Quaternion.Euler(rotation);

      GameObject enemyLaser = Instantiate(_enemyLaserPrefab,
                                           transform.position,
                                           _laserRotation);
      enemyLaser.GetComponent<Laser>().AssignEnemyLaser();
    }
  }
}