using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using UnityEngine;
using System.Linq;

public class ElicitationServer : MonoBehaviour
{

  public TextAsset htmlFile;

  private byte[] fileBytes;
  private HttpListener listener;
  private Thread listenerThread;
  private bool isRunning;
  private List<GameObject> elicitationSample = new();
  private List<String> elicitationSampleNames = new();
  private int currentElicitationIndex = -1;
  private readonly Queue<Action> executionQueue = new();

  void Start()
  {
    fileBytes = htmlFile.bytes; // bytes cannot be accessed outside main thread
    InitElicitation();
    StartServer();
  }

  void OnDestroy()
  {
    StopServer();
  }

  private void Update()
  {
    lock (executionQueue)
    {
      while (executionQueue.Count > 0)
      {
        executionQueue.Dequeue().Invoke();
      }
    }
  }

  public void Enqueue(Action action)
  {
    lock (executionQueue)
    {
      executionQueue.Enqueue(action);
    }
  }


  private void InitElicitation()
  {
    foreach (Transform child in transform)
    {
      elicitationSample.Add(child.gameObject);
    }

    System.Random random = new System.Random();
    elicitationSample = elicitationSample.OrderBy(_ => random.Next()).ToList();
    elicitationSampleNames = elicitationSample.Select(x => x.name).ToList(); // name cannot be accessed outside main thread
  }

  private void ActivateElicitation(int index)
  {
    elicitationSample.ForEach(x => x.SetActive(false));
    if (index < elicitationSample.Count && index > -1)
      elicitationSample[index].SetActive(true);
  }

  private void StartServer()
  {
    listener = new HttpListener();
    listener.Prefixes.Add("http://*:8080/");
    listenerThread = new Thread(HandleRequests);
    listenerThread.Start();
    isRunning = true;
    Debug.Log("Open your browser and load http://localhost:8080/ to change the elicitation.");
  }

  private void StopServer()
  {
    isRunning = false;
    if (listener != null)
    {
      listener.Close();
      listener = null;
    }

    if (listenerThread != null)
    {
      listenerThread.Abort();
      listenerThread = null;
    }

    Debug.Log("Server stopped.");
  }

  private void HandleRequests()
  {
    listener.Start();
    while (isRunning)
    {
      HttpListenerContext context = listener.GetContext();
      HttpListenerRequest request = context.Request;

      if (request.HttpMethod == "GET")
        HandleGetRequest(context);
      else if (request.HttpMethod == "POST")
        HandlePostRequest(context);
    }

    listener.Stop();
  }

  private void HandleGetRequest(HttpListenerContext context)
  {
    context.Response.ContentType = "text/html";
    context.Response.ContentLength64 = fileBytes.Length;
    context.Response.OutputStream.Write(fileBytes, 0, fileBytes.Length);

    context.Response.OutputStream.Close();
  }

  private void HandlePostRequest(HttpListenerContext context)
  {
    string url = context.Request.Url.AbsolutePath;

    if (url == "/previous/")
    {
      currentElicitationIndex = currentElicitationIndex > 0 ? currentElicitationIndex - 1 : 0;
      Enqueue(() => ActivateElicitation(currentElicitationIndex));
    }
    else if (url == "/next/")
    {
      currentElicitationIndex = currentElicitationIndex < elicitationSample.Count
        ? currentElicitationIndex + 1
        : elicitationSample.Count;
      Enqueue(() => ActivateElicitation(currentElicitationIndex));
    }

    string response = JsonUtility.ToJson(
      new ElicitationData
      {
        Name = currentElicitationIndex >= elicitationSampleNames.Count
          ? ""
          : elicitationSampleNames[currentElicitationIndex],
        Index = currentElicitationIndex + 1,
        Max = elicitationSample.Count
      });
    Debug.Log(response);

    byte[] buffer = Encoding.UTF8.GetBytes(response);

    context.Response.ContentLength64 = buffer.Length;
    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
    context.Response.OutputStream.Close();
  }

  public class ElicitationData
  {
    public string Name;
    public int Index;
    public int Max;
  }
}