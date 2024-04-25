using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DemoMenu : MonoBehaviour {

    [SerializeField] private GameObject step1;
    [SerializeField] private GameObject step2;
    [SerializeField] private GameObject step3;
    [SerializeField] private GameObject projection;

    private int _nextStepIndex = 1;
    
    private void Start() {
        NextStep();
    }

    public void TriggerCamera() {
        projection.SetActive(!projection.activeSelf);
    }
    
    public void TriggerObjectVisibility(GameObject go) {
        go.SetActive(!go.activeSelf);
    }
    
    public void NextStep() {
        step1.SetActive(_nextStepIndex == 1);
        step2.SetActive(_nextStepIndex == 2);
        step3.SetActive(_nextStepIndex == 3);
        _nextStepIndex++;
        if (_nextStepIndex > 3) _nextStepIndex = 1;
    }

}
