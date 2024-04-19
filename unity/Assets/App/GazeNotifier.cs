using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeNotifier : MonoBehaviour {
  [SerializeField] public GazeThighSelector gazeThigh;
  [SerializeField] public float scaleSpeed = 1.0f;
  [SerializeField] public float maxScale = 1.2f;
  [SerializeField] public float minScale = 1f;

  private bool _isBeingLookedAt = false;


  private void Update() {
    _isBeingLookedAt = gazeThigh.selection && gameObject.GetInstanceID() == gazeThigh.selection.GetInstanceID();

    if (_isBeingLookedAt && transform.localScale.x < maxScale) {
      transform.localScale += Vector3.one * Time.deltaTime * scaleSpeed;
    }
    else if (!_isBeingLookedAt && transform.localScale.x > minScale) {
      transform.localScale -= Vector3.one * Time.deltaTime * scaleSpeed;
    }
  }
}