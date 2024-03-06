using System;
using UnityEngine;

public class Example : MonoBehaviour {
  public GameObject temp;

  private void OnEnable() {
    Touchpad.OnScale += Scale;
    Touchpad.OnRotate += Rotate;
  }

  private void Rotate(Vector2 direction, float delta) {
    var velocity = 0.85f; // 0 > x < 1
    var growth = -1 + (float)Math.Pow(1 - velocity, -delta * 100);
    Vector2 rotation = direction * growth;
    // FIXME : Why are these axes inverted?
    temp.transform.Rotate(rotation.y, rotation.x, 0, Space.World);
  }

  private void Scale(float delta) {
    if (temp.transform.localScale.x < 0.1f && delta < 0) return;
    var velocity = 0.85f; // 0 > x < 1
    var growth = -1 + (float)Math.Pow(1 - velocity, -delta);
    temp.transform.localScale += new Vector3(growth, growth, growth);
  }
}