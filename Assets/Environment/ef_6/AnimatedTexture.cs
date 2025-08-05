using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class AnimatedTexture : MonoBehaviour
{
  [Header("Settings")]
  public float fps = 30.0f;
  public Texture2D[] frames;

  private MeshRenderer rendererMy;
  private Coroutine animationCoroutine;

  private void Awake()
  {
    rendererMy = GetComponent<MeshRenderer>();
    rendererMy.enabled = false; 
  }

  public void Play()
  {
    if (frames == null || frames.Length == 0) return;

    if (animationCoroutine != null)
      StopCoroutine(animationCoroutine);

    animationCoroutine = StartCoroutine(PlayAnimation());
  }

  private IEnumerator PlayAnimation()
  {
    rendererMy.enabled = true;

    float frameTime = 1f / fps;

    for (int i = 0; i < frames.Length; i++)
    {
      rendererMy.sharedMaterial.SetTexture("_MainTex", frames[i]);
      yield return new WaitForSeconds(frameTime);
    }

    rendererMy.enabled = false;
    animationCoroutine = null;
  }
}
