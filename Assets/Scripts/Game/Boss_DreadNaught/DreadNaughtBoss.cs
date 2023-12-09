using System.Collections;
using UnityEngine;

public class DreadNaughtBoss : MonoBehaviour
{
  private Transform _playerTransform;
  private Player _playerScript;
  private SpriteRenderer _spriteRenderer;
  private OnBossDeath _bossdeath;
  private Animator _deathAnim;
  private UiManager _wonGameFlicker;

  private bool _isBossDead = false;

  [SerializeField]
  private GameObject _leftSide;
  [SerializeField]
  private GameObject _fakeleftSide;

  [SerializeField]
  private int _bossHealth = 10;
  private Collider2D _collider;

  private float _bossSpeed = 10;
  [SerializeField]
  private float _bossCurrentSpeed = 0;
  private Vector3 _position;
  private Vector3 _direction = new Vector3(0, 1, 0);

  [SerializeField]
  private bool _isGiantLaser = false;
  private bool _isRotating = false;
  [SerializeField]
  private GameObject _giantLaser;
  [SerializeField]
  private GameObject _chargingGiantLaser;

  private void Start()
  {
    _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    if (_playerTransform == null)
    {
      Debug.LogError("DreadNaughtBoss :: _playerTransform is NULL");
    }

    _playerScript = GameObject.Find("Player").GetComponent<Player>();
    if (_playerScript == null)
    {
      Debug.LogError("DreadNaughtBoss :: _playerScript is NULL");
    }

    _bossdeath = GameObject.Find("Post_Process_Volume").GetComponent<OnBossDeath>();
    if (_bossdeath == null)
    {
      Debug.LogError("DreadNaughtBoss :: _bossdeath is NULL");
    }

    _deathAnim = GetComponent<Animator>();
    if (_deathAnim == null)
    {
      Debug.LogError("DreadNaughtBoss :: _deathAnim is NULL");
    }

    _wonGameFlicker = GameObject.Find("Canvas").GetComponent<UiManager>();
    if (_wonGameFlicker == null)
    {
      Debug.LogError("DreadNaughtBoss :: _wonGameFlicker is Null");
    }

    _spriteRenderer = GetComponent<SpriteRenderer>();

    //Starts Left Side
    _position = new Vector3(-10, 24, 0);
    _bossCurrentSpeed = _bossSpeed;

  }

  private void Update()
  {
    if (_isBossDead == false)
    {
      BossMovement();
      RotateBoss();
      if ((_position.x >= -5 && _position.x <= 5) && _position.y <= 13 && _isGiantLaser == false)
      {
        StartCoroutine(GiantLaser());
      }
    }

  }

  private void BossMovement()
  {

    _position = gameObject.transform.position;

    // goes to the right side
    if (_position.x <= -10 && _position.y <= -29)
    {
      gameObject.transform.position = new Vector3(10, -24, 0);
      gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // goes to the center
    else if (_position.x >= 10 && _position.y >= 29)
    {
      gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
      gameObject.transform.position = new Vector3(0, 24, 0);
      _leftSide.SetActive(false);
      _fakeleftSide.SetActive(true);
    }

    //goes back to the left side
    else if (_position.x <= -14 || _position.x >= 14)
    {
      gameObject.transform.position = new Vector3(-10, 24, 0);
      gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
      _leftSide.SetActive(true);
      _fakeleftSide.SetActive(false);
    }

    transform.Translate(Vector3.up * _bossCurrentSpeed * Time.deltaTime);
  }

  private void RotateBoss()
  {
    if (_isRotating == true && _playerTransform == true)
    {

      if (_playerTransform.transform.position.x >= 0)
      {
        gameObject.transform.Rotate(0, 0, (10 * Time.deltaTime));
      }
      else
      {
        gameObject.transform.Rotate(0, 0, (-10 * Time.deltaTime));
      }
    }
  }

  IEnumerator GiantLaser()
  {
    //stops moving
    _isGiantLaser = true;
    _bossCurrentSpeed = 0;

    yield return new WaitForSeconds(1f);

    // Charges Laser
    for (int i = 0; i < 3; i++)
    {
      _chargingGiantLaser.SetActive(true);
      yield return new WaitForSeconds(0.5f);
      _chargingGiantLaser.SetActive(false);
      yield return new WaitForSeconds(0.5f);
    }

    //Shoots laser and rotates
    if (_isBossDead == false)
    {
      _giantLaser.SetActive(true);
      _isRotating = true;
      yield return new WaitForSeconds(2f);
    }

    //Stops rotating and shooting laser, begins to move.
    _giantLaser.SetActive(false);
    _isRotating = false;
    _bossCurrentSpeed = _bossSpeed;
    yield return new WaitForSeconds(3f);
    _isGiantLaser = false;

  }

  private void BossDamage()
  {
    if (_bossHealth > 0)
    {
      _bossHealth--;
      StartCoroutine(CollisionOFF());
      StartCoroutine(SpriteOFF());
    }
    if (_bossHealth <= 0)
    {
      BossDeath();
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    switch (other.tag)
    {
      case "Player":
        _playerScript.CollisionWithBoss();
        break;
      case "Laser_Player" or "Missile_Player":
        BossDamage();
        Destroy(other.gameObject);
        break;
      default:
        break;
    }
  }

  IEnumerator CollisionOFF()
  {
    _collider = GetComponent<Collider2D>();
    _collider.enabled = false;
    yield return new WaitForSeconds(1f);
    _collider.enabled = true;
  }
  IEnumerator SpriteOFF()
  {
    _spriteRenderer.enabled = false;
    yield return new WaitForSeconds(0.200f);
    _spriteRenderer.enabled = true;
    yield return new WaitForSeconds(0.200f);
    _spriteRenderer.enabled = false;
    yield return new WaitForSeconds(0.200f);
    _spriteRenderer.enabled = true;
    yield return new WaitForSeconds(0.400f);
  }

  private void BossDeath()
  {
    Destroy(GetComponent<Collider2D>());
    _isBossDead = true;
    _bossCurrentSpeed = 0;
    _deathAnim.SetTrigger("OnEnemyDeath");
    _leftSide.SetActive(false);
    _fakeleftSide.SetActive(false);
    _bossdeath.InitiateBossDeathSequence();
    _wonGameFlicker.GameWonSequence();
    Destroy(gameObject, 2.70f);
  }
}
