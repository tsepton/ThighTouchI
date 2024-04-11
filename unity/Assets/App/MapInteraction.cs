using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInteraction : MonoBehaviour {
  public GameObject map;

  public GameObject cursor;

  private Vector2? _cursorRef;
  private Vector2? _moveRef;

  private void OnEnable() {
    Touchpad.OnRelease += OnRelease;
    Touchpad.OnRotate += MoveMap;
    Touchpad.OnScale += ScaleMap;
    Touchpad.OnTouch += MoveCursor;
  }

  private void MoveMap(Vector2 direction, float delta) {
    if (_moveRef == null) _moveRef = direction;
    else {
      var movement = new Vector3(-direction.x, 0, direction.y) / 100;
      var velocity = 0.45f; // 0 > x < 1
      var growth = -1 + (float)Math.Pow(1 - velocity, -delta * 100);
      map.transform.position += (movement * growth);
      _moveRef = direction;
    }
  }

  private void ScaleMap(float delta) {
    if (map.transform.localScale.x < 0.1f && delta < 0) return;
    var velocity = 0.45f; // 0 > x < 1
    var growth = -1 + (float)Math.Pow(1 - velocity, -delta);
    map.transform.localScale += new Vector3(growth, growth, growth);
  }

  private void OnRelease(int touchPointId) {
    _cursorRef = null;
    _moveRef = null;
  }

  private void MoveCursor(TouchPoint touchPoint) {
    if (_cursorRef == null) _cursorRef = touchPoint.coordinates;
    else {
      var reef = (Vector2)_cursorRef;
      var delta = new Vector3(reef.x - touchPoint.coordinates.x, 0, -(reef.y - touchPoint.coordinates.y));
      cursor.transform.position += delta;

      _cursorRef = touchPoint.coordinates;
    }
  }
}