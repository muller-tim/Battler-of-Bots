using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerSystem : MonoBehaviour {
    [SerializeField] private Wave[] m_enemyWaves;

    private int m_currentWave = 0;
    private GameObject[] m_currentWaveEnemies;

    public GameObject[] WaveEnemies => m_currentWaveEnemies;
    public int WaveCount => m_enemyWaves.Length;

    public void SpawnNextWave() {
        Wave currentWave = m_enemyWaves[m_currentWave];
        m_currentWaveEnemies = new GameObject[currentWave.Enemies.Length];

        // Loop over every enemy in the wave
        for (int i = 0; i < currentWave.Enemies.Length; i++) {
            GameObject currentEnemy = Instantiate(currentWave.Enemies[i]);
            m_currentWaveEnemies[i] = currentEnemy;
            currentEnemy.transform.position = currentWave.EnemyPosition[i];
        }

        m_currentWave++;
    }

    public Vector3[] GetNextWaveEnemyPositions() {
        int nextWave = m_currentWave  < m_enemyWaves.Length ? m_currentWave : -1;

        if (nextWave == -1) return null;

        return m_enemyWaves[nextWave].EnemyPosition;
    }
}

[Serializable]
class Wave {
    [SerializeField] private GameObject[] m_enemies;
    [SerializeField] private Vector3[] m_enemyPositions;

    public GameObject[] Enemies {
        get => m_enemies;
        private set => m_enemies = value;
    }

    public Vector3[] EnemyPosition {
        get => m_enemyPositions;
        private set => m_enemyPositions = value;
    }
}
