using UnityEngine;
using UnityEngine.WSA;

public class ShowTouchPointsOnCanvas : MonoBehaviour {

    public GameObject pointer;
    public RectTransform canvas;
    
    private GameObject[] _touchPoints;
    private RectTransform[] _touchPointsRect;

    
    
    private void OnEnable() {
        _touchPoints = new GameObject[10]; // Assuming maximum of 10 touch points
        _touchPointsRect = new RectTransform[10];
        Touchpad.OnTouch += DrawTouchPoint ;
    }

    public void DrawTouchPoint(TouchPoint touchPoint) {
        Debug.Log("test");
        Vector2 canvasSize = canvas.sizeDelta; // canvasRect.sizeDelta
        Transform parentTransform = canvas.transform; // Parent the cubes to the canvas

        float touchX = (-0.5f + touchPoint.coordinates.x) * canvasSize.x / 5f;
        float touchY = (-0.5f + touchPoint.coordinates.y) * canvasSize.y / 5f;

        if (!touchPoint.isLast) {
            Debug.Log("salut 1");
            if (_touchPoints[touchPoint.id] == null) {
                var newPointer = Instantiate(pointer, parentTransform);
                _touchPoints[touchPoint.id] = newPointer;
                _touchPointsRect[touchPoint.id] = newPointer.GetComponent<RectTransform>();
            }
            _touchPointsRect[touchPoint.id].anchoredPosition = new Vector2(touchX, -touchY);
            Debug.Log(new Vector2(touchX, -touchY));
        }
        else {
            if (_touchPoints[touchPoint.id] != null) {
                Destroy(_touchPoints[touchPoint.id]);
                _touchPoints[touchPoint.id] = null;
            }
        }
    }
}
