using System;
using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
  private Collider2D _collider;
  private Transform _player;
  private DreadNaughtBoss _boss;
  private Quaternion _initialRotation;

  [SerializeField]
  private GameObject _enemyLaserPrefab;
  private float _fireRate = 1f,
                _canFire = -1f;
  private Quaternion _laserRotation;

  private bool _beamCharge = false;

  private Vector3 _enemyLocation;
  private Vector3 _playerLocation;
  private Vector3 _rightTriangle;
  private float _xValueofShip;
  private float _yValueofShip;
  private float _radians;
  private float _angle;

  private void Start()
  {
    _boss = GetComponentInParent<DreadNaughtBoss>();
    _player = GameObject.Find("Player").GetComponent<Transform>();
    if (_player == null)
    {
      Debug.LogError("Turret :: _player is NULL");
    }
    _initialRotation = gameObject.transform.rotation;
  }

  private void Update()
  {
    if (_beamCharge == false && _player != null)
    {
      LookAtPlayer();
      StartCoroutine(TurretShooting());
    }
  }

  private void LookAtPlayer()
  {
    _enemyLocation = gameObject.transform.position;
    _playerLocation = _player.gameObject.transform.position;
    _rightTriangle = _playerLocation - _enemyLocation;

    _xValueofShip = _rightTriangle.x;
    _yValueofShip = _rightTriangle.y;
    _radians = (float)Math.Atan2(_yValueofShip, _xValueofShip);
    _angle = _radians * (180 / 3.1415f);
    if (_beamCharge == false)
    {
      gameObject.transform.rotation = Quaternion.Euler(0, 0, (_angle + 180));
    }
    else
    {
      gameObject.transform.rotation = _initialRotation;
    }
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
      //Debug.Break();
      enemyLaser.GetComponent<Laser>().AssignEnemyLaser();

    }
  }

}