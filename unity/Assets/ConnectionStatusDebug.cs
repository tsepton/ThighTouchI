using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

public class ConnectionStatusDebug : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI text;
    
    void OnEnable() {
        Touchpad.OnConnectionStatusUpdate += UpdateStatus;
    }
    
    void OnDestroy() {
        Touchpad.OnConnectionStatusUpdate -= UpdateStatus;
    }

    private void UpdateStatus(string status) {
        text.text = status;
    }
}
