using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ThreeDimensionManipulation : GazeSelector {

  public Canvas canvas;
  
  private void OnEnable() {
    Touchpad.OnScale += Scale;
    Touchpad.OnRotate += Rotate;
  }

  private void OnSelectEntered(SelectEnterEventArgs args)
  {
    // Store the selected GameObject
    base.OnSelectEntered(args);
    canvas.gameObject.transform.position = base.Selection.transform.position;
  }

  private void Rotate(Vector2 direction, float delta) {
    if (!base.Selection) return;
    var velocity = 0.45f; // 0 > x < 1
    var growth = -1 + (float)Math.Pow(1 - velocity, -delta * 100);
    Vector2 rotation = direction * growth;
    // FIXME : Why are these axes inverted?
    base.Selection.transform.Rotate(rotation.y, rotation.x, 0, Space.World);
  }

  private void Scale(float delta) {
    if (!base.Selection) return;
    if (base.Selection.transform.localScale.x < 0.1f && delta < 0) return;
    var velocity = 0.65f; // 0 > x < 1
    var growth = -1 + (float)Math.Pow(1 - velocity, -delta);
    base.Selection.transform.localScale += new Vector3(growth, growth, growth);
  }

  public void SetSelection(GameObject go) {
    base.Selection = go;
  }
}