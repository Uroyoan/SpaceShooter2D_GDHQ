using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class OnBossDeath : MonoBehaviour
{
  private PostProcessVolume _postProcessVolume;
  private Bloom _bloom;

  private void Start()
  {
    _postProcessVolume = GetComponent<PostProcessVolume>();
    if (_postProcessVolume == null)
    {
      Debug.LogError("OnBossDeath :: _postProcessVolume is NULL");
    }
    _postProcessVolume.profile.TryGetSettings(out _bloom);
  }

  public void InitiateBossDeathSequence()
  {
    StartCoroutine(BossDeathSequence());
  }
  IEnumerator BossDeathSequence()
  {
    for(int i = 0; 15 > i; i++)
    {
      yield return new WaitForSeconds(0.1f);
      _bloom.intensity.value += 20;
      i++;
    }
    yield return new WaitForSeconds(1f);
    for (int i = 0; 15 > i; i++)
    {
      yield return new WaitForSeconds(0.1f);
      _bloom.intensity.value -= 20;
    }
    _bloom.intensity.value = 10;
    yield return new WaitForSeconds(1);
  }
}
