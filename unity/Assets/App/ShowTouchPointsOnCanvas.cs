using System;
using UnityEngine;

public class ShowTouchPointsOnCanvas : MonoBehaviour {
    public GameObject pointer;
    public RectTransform canvas;
    
    private GameObject[] _touchPoints;
    private RectTransform[] _touchPointsRect;
 // Utilisez Collider pour 3D

    private Renderer[] _touchPointsRenderers;
    
    private void OnEnable() {
        _touchPoints = new GameObject[10]; // Suppose un maximum de 10 points de contact
        _touchPointsRect = new RectTransform[10];
        _touchPointsRenderers = new Renderer[10];

        Touchpad.OnTouch += DrawTouchPoint;
    }

    public void Start() {
        Transform parentTransform = canvas.transform; // Parent les pointeurs au canvas
        for (var i = 0; i < 10; i++) {
            var newPointer = Instantiate(pointer, parentTransform);
            _touchPoints[i] = newPointer;
            _touchPointsRect[i] = newPointer.GetComponent<RectTransform>();
            _touchPointsRenderers[i] = newPointer.GetComponent<Renderer>();

            // Ajoutez un SphereCollider et activez isTrigger

        }
    }

    private void DrawTouchPoint(TouchPoint touchPoint) {
        
        Vector2 canvasSize = canvas.sizeDelta; // canvasRect.sizeDelta
        
        float touchX = (-0.5f + touchPoint.coordinates.x) * canvasSize.x / 5f;
        float touchY = (-0.5f + touchPoint.coordinates.y) * canvasSize.y / 5f;

        var coord = Rotation90Degrees(touchX,-touchY);


        if (!touchPoint.isLast) {
            if (!_touchPointsRenderers[touchPoint.id].enabled)
                _touchPointsRenderers[touchPoint.id].enabled = true;
            _touchPointsRect[touchPoint.id].anchoredPosition = new Vector2(coord.Item1, coord.Item2);
        }
        else {
            _touchPointsRenderers[touchPoint.id].enabled = false;
        }
    }
    public static Tuple<float, float> Rotation90Degrees(float x, float y)
    {
        float newX = -y;
        float newY = x;
        return Tuple.Create(newX, newY);
    }

}
