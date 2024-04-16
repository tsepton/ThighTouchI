using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenMenu : MonoBehaviour {

    [SerializeField] private AudioSource audio;
    [SerializeField] private GameObject playBtn;
    [SerializeField] private GameObject pauseBtn;
    [SerializeField] private AudioClip[] songs;

    private int _index= 0;

    public void Play() {
        audio.Play();
        playBtn.SetActive(false);
        pauseBtn.SetActive(true);
    }

    public void Pause() {
        audio.Pause();
        pauseBtn.SetActive(false);
        playBtn.SetActive(true);
    }

    public void Stop() {
        audio.Stop();
        pauseBtn.SetActive(false);
        playBtn.SetActive(true);
    }

    public void Next() {
        if (_index == songs.Length) _index = 0;
        audio.clip = songs[_index];
        _index += 1;
        audio.Play();
    }

    public void Previous() {
        _index -= 1;
        if (_index < 0) _index = songs.Length - 1;
        audio.clip = songs[_index];
        audio.Play();
    }
}
