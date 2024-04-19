using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoaderMenu : MonoBehaviour {
  public void LoadScene(string name) {
    SceneManager.LoadScene(name);
  }
}