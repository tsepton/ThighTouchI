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

    public void OpenCamera() {
        projection.SetActive(true);
    }

    public void CloseCamera() {
        projection.SetActive(false);
    }

    public void NextStep() {
        Debug.Log("NextStep");
        step1.SetActive(_nextStepIndex == 1);
        step2.SetActive(_nextStepIndex == 2);
        step3.SetActive(_nextStepIndex == 3);
        _nextStepIndex++;
        if (_nextStepIndex > 3) _nextStepIndex = 1;
    }

}
