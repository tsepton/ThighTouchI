using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GazeThighSelector : MonoBehaviour {
  [SerializeField] public XRGazeInteractor gazeInteractor;

  [HideInInspector] public GameObject selection;

  private GameObject _selection;
  private bool _isTouching;

  private void OnEnable() {
    Touchpad.OnTouch += OnTouch;
    Touchpad.OnRelease += OnRelease;
  }

  private void Start() {
    gazeInteractor.selectEntered.AddListener(OnSelectEntered);
    gazeInteractor.hoverEntered.AddListener(OnHoverEntered);
    gazeInteractor.selectExited.AddListener(OnSelectExited);
    gazeInteractor.hoverExited.AddListener(OnHoverExited);
  }

  private void OnSelectEntered(SelectEnterEventArgs args) {
    _selection = args.interactableObject.transform.gameObject;
    if (_isTouching && selection != null && selection.activeSelf) return;
    selection = _selection;
  }

  private void OnHoverEntered(HoverEnterEventArgs args) {
    _selection = args.interactableObject.transform.gameObject;
    if (_isTouching && selection != null && selection.activeSelf) return;
    selection = _selection;
  }

  private void OnSelectExited(SelectExitEventArgs args) {
    _selection = null;
    if (!_isTouching) selection = _selection;
  }

  private void OnHoverExited(HoverExitEventArgs args) {
    _selection = null;
    if (!_isTouching) selection = _selection;
  }

  private void OnTouch(TouchPoint _) {
    _isTouching = true;
  }

  private void OnRelease(int _) {
    _isTouching = false;
    selection = _selection;
  }
}