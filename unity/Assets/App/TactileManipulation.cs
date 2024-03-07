using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TactileManipulation : MonoBehaviour {

  public XRGazeInteractor gazeInteractor;
  
  private GameObject _selection;

  private void OnEnable() {
    Touchpad.OnScale += Scale;
    Touchpad.OnRotate += Rotate;
  }

  private void Start() {
    gazeInteractor.selectEntered.AddListener(OnSelectEntered);
  }
  
  private void OnSelectEntered(SelectEnterEventArgs args)
  {
    // Store the selected GameObject
    Debug.Log("OnSelectEntered");
    _selection = args.interactableObject.transform.gameObject;
    Debug.Log("Selected: " + _selection.name);
  }
  
  private void Rotate(Vector2 direction, float delta) {
    if (!_selection) return;
    var velocity = 0.85f; // 0 > x < 1
    var growth = -1 + (float)Math.Pow(1 - velocity, -delta * 100);
    Vector2 rotation = direction * growth;
    // FIXME : Why are these axes inverted?
    _selection.transform.Rotate(rotation.y, rotation.x, 0, Space.World);
  }

  private void Scale(float delta) {
    if (!_selection) return;
    if (_selection.transform.localScale.x < 0.1f && delta < 0) return;
    var velocity = 0.85f; // 0 > x < 1
    var growth = -1 + (float)Math.Pow(1 - velocity, -delta);
    _selection.transform.localScale += new Vector3(growth, growth, growth);
  }

  public void SetSelection(GameObject go) {
    _selection = go;
  }
}