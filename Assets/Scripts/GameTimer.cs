using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour {
    private bool started;
    private float timer;
    public event Action<float> OnTimeChange;
    public float EndTimer => timer;

    public void StartTimer() {
        started = true;
    }

    public void StopTimer() {
        started = false;
    }
        

    // Update is called once per frame
    void Update()
    {
        if (started) {
            timer += Time.deltaTime;
            OnTimeChange?.Invoke(timer);
            // Debug.Log($"Timer: {timer}");
        }
    }
}
