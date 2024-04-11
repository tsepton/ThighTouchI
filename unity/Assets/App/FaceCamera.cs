using UnityEngine;

public class FaceCamera : MonoBehaviour {
  private Transform _camera;

  void Start() {
    if (Camera.main != null) _camera = Camera.main.transform;
  }

  void Update() {
    transform.LookAt(_camera);
  }
}