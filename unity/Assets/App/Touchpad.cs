using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using WebSocketSharp;

public class Touchpad : MonoBehaviour {
  /// ////////////////////////////////////
  /// /// //////////// API ///////////////  
  /// ////////////////////////////////////
  // THIS NEEDS SOME REFACTORING...
  public string uri;

  public delegate void Touch(TouchPoint t);

  public static event Touch OnTouch;

  // FIXME - THIS IS TEMPORARY 
  public delegate void OneTouch(TouchPoint t);

  public static event OneTouch OnOneTouch;

  public delegate void Release(int touchPointId);

  public static event Release OnRelease;

  public delegate void Scale(float delta);

  public static event Scale OnScale;

  public delegate void Translate(Vector2 delta);

  public static event Translate OnTranslate;

  public delegate void Rotate(Vector2 direction, float delta);

  public static event Rotate OnRotate;

  /// ////////////////////////////////////
  /// //////////// END API ///////////////
  /// ////////////////////////////////////
  private readonly Queue<SimultaneousTouchPoints> _touchPoints = new();

  private WebSocket _ws;

  private Coroutine _inputHandler;

  private void Start() {
    _ws = new WebSocket($"ws://{uri}");
    
    _ws.OnOpen += (sender, args) => {
      Debug.Log("Connected.");
      _inputHandler = StartCoroutine(SemanticHandler());
    };
    
    _ws.OnClose += (sender, args) => {
      Debug.Log("Disconnected.");
      StopCoroutine(_inputHandler);
    };
    
    _ws.OnMessage += (sender, e) => {
      var rawData = e.Data.Trim('[', ']').Split(',');
      var data = new int[rawData.Length];
      for (int i = 0; i < rawData.Length; i++) {
        data[i] = int.Parse(rawData[i]);
      }

      _touchPoints.Enqueue(new SimultaneousTouchPoints(data));
    };

    _ws.Connect();
  }


  private IEnumerator SemanticHandler() {
    Dictionary<int, long> previousPoints = new Dictionary<int, long>();
    bool firstTouch = true;
    float initialDistanceFingers = 0;
    Vector2 initialCenter = Vector2.zero;

    var loadIndex = 0;
    while (true) {
      // Performance handling
      // Max 5 inputs per iteration 
      if (loadIndex >= 5) {
        loadIndex = 0;
        yield return null;
      }
      else loadIndex++;

      var currentPoints = _touchPoints.Count != 0 ? _touchPoints.Dequeue() : null;

      // Release handling
      var news = currentPoints?.GetTouchPoints() ?? Array.Empty<TouchPoint>();
      var idsToRemove = new List<int>();
      foreach (var (id, time) in previousPoints.Where(value => !news.Select(t => t.id).Contains(value.Key))) {
        if (time + 50L > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()) continue;
        OnRelease?.Invoke(id);
        idsToRemove.Add(id);
        firstTouch = true;
      }
      idsToRemove.ForEach(i => previousPoints.Remove(i));
      foreach (var touchPoint in news) {
        previousPoints[touchPoint.id] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
      }

      // Touch handling
      for (var i = 0; i < 10; i++) {
        var currentP = currentPoints?.Data[i];
        if (currentP == null) break;
        OnTouch?.Invoke((TouchPoint)currentP);
      }

      // FIXME - This is temporary - this entire interface shall be refactored
      if (currentPoints?.Data[0] != null && currentPoints?.Data[1] == null) {
        OnOneTouch?.Invoke((TouchPoint)currentPoints.Data[0]);
      }
      if (currentPoints?.Data[0] == null || currentPoints?.Data[1] == null) continue;
      if (currentPoints.Data[2] != null) { // If more than 2 touchpoints, nothing should happen
        firstTouch = true;
        continue;
      }

      var t1 = (TouchPoint)currentPoints.Data[0];
      var t2 = (TouchPoint)currentPoints.Data[1];
      if (firstTouch) {
        firstTouch = false;
        initialDistanceFingers = Vector2.Distance(t1.coordinates, t2.coordinates);
        initialCenter = (t1.coordinates + t2.coordinates) / 2f;
      }

      // Scale handling
      var distanceFingers = Vector2.Distance(t1.coordinates, t2.coordinates);
      var diffDistance = distanceFingers - initialDistanceFingers;
      if (diffDistance != 0) OnScale?.Invoke(diffDistance);
      initialDistanceFingers = distanceFingers;

      // Rotate handling
      var currentCenter = (t1.coordinates + t2.coordinates) / 2f;
      var directionCenter = (initialCenter - currentCenter).normalized;
      if (directionCenter != Vector2.zero) {
        var delta = Vector2.Distance(currentCenter, initialCenter);
        OnRotate?.Invoke(directionCenter, delta);
      }

      initialCenter = currentCenter;

      // Translate handling
      // TODO 
    }
  }
}

public class SimultaneousTouchPoints {
  public readonly TouchPoint?[] Data = new TouchPoint?[10];

  public TouchPoint[] GetTouchPoints() {
    return Data.NotNull().Select(tp => tp.Value).ToArray();
  }

  public SimultaneousTouchPoints(int[] dataFromHid) {
    for (int index = 1, count = 0; index < dataFromHid.Length; index += 6, count++) {
      var touchPointData = new int[6];
      Array.Copy(dataFromHid, index, touchPointData, 0, 6);
      if (touchPointData.Length < 6) break;
      if (touchPointData.Sum() == 0) break;
      Data[count] = new TouchPoint(touchPointData);
    }
  }
}

public readonly struct TouchPoint {
  public int id { get; }

  // WARNING
  // consider using OnRelease as the raspberry does not always emit all touches
  // and thus last points may be lost along the way.
  public bool isLast { get; }

  public Vector2 coordinates { get; }

  public TouchPoint(IReadOnlyList<int> raw) {
    id = raw[1];
    isLast = raw[0] == 4;
    var x = ((raw[3] + raw[2] / 256.0f) / 16f);
    var y = ((raw[5] + raw[4] / 256.0f) / 16f);
    coordinates = new Vector2(x, y);
  }

  public override string ToString() {
    return $"Touch {id} on ({coordinates.x}, {coordinates.y})";
  }
}