using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ThreeDimensionManipulation : MonoBehaviour {
  // FIXME - this has not been updated since the prototype upgrade - WON'T WORK 

  [SerializeField] public Canvas canvas;
  
  [SerializeField] public XRGazeInteractor gazeInteractor;

  [SerializeField] public GazeThighSelector gazeThigh;

  private void OnEnable() {
    Touchpad.OnScale += Scale;
    Touchpad.OnRotate += Rotate;
  }

  private void Start() {
    gazeInteractor.selectEntered.AddListener(OnSelectEntered);
  }

  private void OnSelectEntered(SelectEnterEventArgs args) {
    gazeInteractor.selectEntered.AddListener(OnSelectEntered);
    canvas.gameObject.transform.position = gazeThigh.selection.transform.position;
  }

  private void Rotate(Vector2 direction, float delta) {
    if (!gazeThigh.selection) return;
    var velocity = 0.45f; // 0 > x < 1
    var growth = -1 + (float)Math.Pow(1 - velocity, -delta * 100);
    Vector2 rotation = direction * growth;
    // FIXME : Why are these axes inverted?
    gazeThigh.selection.transform.Rotate(rotation.y, rotation.x, 0, Space.World);
  }

  private void Scale(float delta) {
    if (!gazeThigh.selection) return;
    if (gazeThigh.selection.transform.localScale.x < 0.1f && delta < 0) return;
    var velocity = 0.65f; // 0 > x < 1
    var growth = -1 + (float)Math.Pow(1 - velocity, -delta);
    gazeThigh.selection.transform.localScale += new Vector3(growth, growth, growth);
  }
  
}