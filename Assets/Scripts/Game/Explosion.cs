using UnityEngine;

public class Explosion : MonoBehaviour
{
  void Start()
  {
    Destroy(gameObject, 2.75f);
  }
}