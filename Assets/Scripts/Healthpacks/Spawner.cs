using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    [SerializeField] private GameObject m_prefabToSpawn;
    [SerializeField] private float m_respawnTimer;
    [SerializeField] private bool m_spawnOnStart;

    private GameObject m_spawnedObject;
    private bool m_timerStarted = false;
    private float m_timer;

    void Start() {
        if(m_spawnOnStart)
            Spawn();
        else
            StartRespawnTimer();
    }

    void Update() {
        if (m_timer > 0.0f) {
            m_timer -= Time.deltaTime;
        }

        if (m_timer <= 0.0f && m_timerStarted) {
            Spawn();
            m_timerStarted = false;
            m_timer = m_respawnTimer;
        }

        if (m_spawnedObject == null) {
            StartRespawnTimer();
        }
    }

    private void Spawn() {
        m_spawnedObject = Instantiate(m_prefabToSpawn, transform.position, Quaternion.identity);
        // StartRespawnTimer();
    }

    private void StartRespawnTimer() {
        m_timerStarted = true;
    }
}
