using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RearMenu : MonoBehaviour {

    [SerializeField] private GameObject projection;
    
    public void TriggerRearCamera() {
        projection.SetActive(!projection.activeSelf);
    }

}
