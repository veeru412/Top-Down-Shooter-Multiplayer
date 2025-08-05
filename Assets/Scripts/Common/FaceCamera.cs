using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Common
{
  public class FaceCamera : MonoBehaviour
  {
    private Transform mTransform;
    private Transform mainCamea;

    private void Awake()
    {
      mTransform= transform;
      mainCamea= Camera.main.transform;
    }

    private void Update()
    {
      mTransform.LookAt(mainCamea, Vector3.up);
    }
  }
}