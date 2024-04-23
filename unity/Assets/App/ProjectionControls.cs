using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionControls : MonoBehaviour {
  [SerializeField] public Camera camera;

  [SerializeField] public GameObject surface;
  
  [SerializeField] public Renderer glowingSurfaceRenderer;

  [SerializeField] public float minimumFov;

  [SerializeField] public float maximumFov;
  
  [SerializeField] public float maximumAngle;
  
  [SerializeField] public float minimumAngle;

  [SerializeField] public GazeThighSelector gazeThigh;

  private TouchPoint? _initTouchPoint;
  
  private Coroutine _timeoutCoroutine;

  private void OnEnable() {
    // FIXME once interface is rewritten
    Touchpad.OnOneTouch += OnOneTouch;
    Touchpad.OnRelease += OnRelease;
    Touchpad.OnScale += OnScale;
  }

  private void Update() {
    glowingSurfaceRenderer.enabled = gazeThigh.selection && gazeThigh.selection == surface;
  }

  private void OnOneTouch(TouchPoint touchpoint) {
    if (!gazeThigh.selection || gazeThigh.selection != surface) return;

    if (_initTouchPoint == null) _initTouchPoint = touchpoint;
    else {
      var first = (TouchPoint)_initTouchPoint; 
      var distance = (touchpoint.coordinates - first.coordinates) * 100;
      var current = camera.gameObject.transform.rotation.eulerAngles;
      if (distance.y > 0 && current.y >= maximumAngle) return;
      if (distance.y < 0 && current.y <= minimumAngle) return;
      camera.gameObject.transform.Rotate(new Vector3(0, 1, 0), distance.y);
      if (_timeoutCoroutine != null) StopCoroutine(_timeoutCoroutine);
      _timeoutCoroutine = StartCoroutine(TimeoutCoroutine(0.02f));
      _initTouchPoint = touchpoint;
    }
  }
  
  IEnumerator TimeoutCoroutine(float delay)
  {
    // Wait for the specified delay
    yield return new WaitForSeconds(delay);
    _initTouchPoint = null;
  }

  private void OnRelease(int _) {
    _initTouchPoint = null;
  }
  
  private void OnScale(float delta) {
    if (!gazeThigh.selection || gazeThigh.selection != surface) return;
    var growth = Math.Abs(delta) * 100;
    var fov = Mathf.MoveTowards(camera.fieldOfView, delta > 0 ? minimumFov : maximumFov, growth);
    camera.fieldOfView = fov;
    _initTouchPoint = null; // this ensures that no rotation is done when user release one finger 
  }
}