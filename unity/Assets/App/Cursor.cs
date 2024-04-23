using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Cursor : MonoBehaviour {
  public delegate void SelectionUpdateEventHandler([CanBeNull] GameObject maybeNewSelection,
    [CanBeNull] GameObject maybeOldSelection);

  public event SelectionUpdateEventHandler OnSelectionUpdate;

  private GameObject _selection;

  public GameObject Selection {
    get => _selection;
    private set {
      if (_selection != value) {
        if (OnSelectionUpdate != null)
          OnSelectionUpdate(value, _selection);
        _selection = value;
      }
    }
  }

  private void OnTriggerEnter(Collider other) {
    if (other.gameObject == gameObject) return;
    Selection = other.gameObject;
  }

  private void OnTriggerExit(Collider other) {
    Selection = null;
  }

  private void OnDisable() {
    Selection = null;
  }
}