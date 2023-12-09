using System.Collections;
using UnityEngine;

public class GiantBeam : MonoBehaviour
{
  private Player _playerScript;

  private void Start()
  {
    _playerScript = GameObject.Find("Player").GetComponent<Player>();
    if (_playerScript == null)
    {
      Debug.LogError("GiantBeam :: _playerScript is NULL");
    }
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Player")
    {
      Debug.Log("inside: " + other.tag);
      _playerScript.CollisionWithBoss();
    }
    else
    {
      Debug.Log("Else: " + other.tag);
    }
    Debug.Log("none: " + other.tag);
  }

}
