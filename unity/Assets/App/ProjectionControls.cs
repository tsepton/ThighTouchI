using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionControls : GazeSelector {

  [SerializeField] public Camera camera;

  [SerializeField] public GameObject surface;

  [SerializeField] public float minimumFov;

  [SerializeField] public float maximumFov;

  private void OnEnable() {
    Touchpad.OnRotate += OnRotate;
    Touchpad.OnScale += OnScale;
    
  }

  private void OnRotate(Vector2 direction, float delta) {
    if (!Selection || Selection != surface) return;
    var current = camera.gameObject.transform.rotation.eulerAngles;
    if (direction.y > 0 && current.y >= 170) return;
    if (direction.y < 0 && current.y <= 140) return;
    camera.gameObject.transform.Rotate(new Vector3(0, 1, 0), direction.y);
  }

  private void OnScale(float delta) {
    if (!Selection || Selection != surface) return;
    var growth = Math.Abs(delta) * 100;
    var fov = Mathf.MoveTowards(camera.fieldOfView, delta > 0 ? minimumFov : maximumFov, growth);
    camera.fieldOfView = fov;
  }
}
