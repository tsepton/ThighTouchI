using System;
using UnityEngine;

public class ShowTouchPointsOnCanvas : MonoBehaviour {
  public GameObject pointer;
  public RectTransform canvas;

  private GameObject[] _touchPoints;
  private RectTransform[] _touchPointsRect;
  private Renderer[] _touchPointsRenderers;

  private void OnEnable() {
    _touchPoints = new GameObject[10]; // Assuming maximum of 10 touch points
    _touchPointsRect = new RectTransform[10];
    _touchPointsRenderers = new Renderer[10];
    Touchpad.OnTouch += DrawTouchPoint;
    Touchpad.OnRelease += HideTouchPoint;
  }

  public void Start() {
    Transform parentTransform = canvas.transform; // Parent the cubes to the canvas
    for (var i = 0; i < 10; i++) {
      var newPointer = Instantiate(pointer, parentTransform);
      _touchPoints[i] = newPointer;
      _touchPointsRect[i] = newPointer.GetComponent<RectTransform>();
      _touchPointsRenderers[i] = newPointer.GetComponent<Renderer>();
    }
  }

  private void DrawTouchPoint(TouchPoint touchPoint) {
    Vector2 canvasSize = canvas.sizeDelta; // canvasRect.sizeDelta

    float touchX = (-0.5f + touchPoint.coordinates.x) * canvasSize.x;
    float touchY = (-0.5f + touchPoint.coordinates.y) * canvasSize.y;

    if (!_touchPointsRenderers[touchPoint.id].enabled)
      _touchPointsRenderers[touchPoint.id].enabled = true;
    _touchPointsRect[touchPoint.id].anchoredPosition = new Vector2(touchX, -touchY);
  }

  private void HideTouchPoint(int touchPointId) {
    _touchPointsRenderers[touchPointId].enabled = false;
  }
}