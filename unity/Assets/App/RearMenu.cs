using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RearMenu : MonoBehaviour {

    [SerializeField] private GameObject projection;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenRearCamera() {
        projection.SetActive(true);
    }

    public void CloseRearCamera() {
        projection.SetActive(false);
    }

}
