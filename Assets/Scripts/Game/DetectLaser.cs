using UnityEngine;

public class DetectLaser : MonoBehaviour
{
  private Enemy _SmartEnemy;

  private void Start()
  {
    _SmartEnemy = GetComponentInParent<Enemy>();
  }

  public void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Laser_Player")
    {
      _SmartEnemy.DetectedLaser();
    }
  }
}