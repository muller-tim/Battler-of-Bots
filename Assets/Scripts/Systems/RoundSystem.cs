using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : MonoBehaviour {
    [SerializeField] private SpawnerSystem m_spawnerSystem;
    [SerializeField] private UpgradeSystem m_upgradeSystem;
    [SerializeField] public GameTimer m_gameTimer;
    [SerializeField] private GameObject m_spawnDecals;
    private int m_maxRounds;
    private int m_currentRound = 0;
    private int m_enemyCount = 0;
    private RoundStatus m_status = RoundStatus.UpgradePhase;
    [SerializeField] private int m_countdownStart = 3;
    private int m_countdown;
    private List<GameObject> m_decals = new List<GameObject>();

    public event Action OnFinish;
    public event Action<int> OnRoundStart;
    public event Action OnUpgradePhaseStart;
    public event Action OnCountdownStart;
    public event Action<int> OnCountdownChange;
    public event Action OnCountdownEnd;

    public AudioSource trainAudioClip;


    public RoundStatus Status {
        get => m_status;
        private set => m_status = value;
    }

    public int Round => m_currentRound;

    [ContextMenu("Start next Round")]
    public bool StartNextRound() {
        if (Status == RoundStatus.Started || Status == RoundStatus.Finished)
            return false;

        Status = RoundStatus.Started;

        if (m_currentRound == 0) {
            m_gameTimer.StartTimer();
        }
        if (m_currentRound == 8)
        {
            trainAudioClip.Play();
        }

        // Spawn wave and get enemies
        m_spawnerSystem.SpawnNextWave();
        m_enemyCount = m_spawnerSystem.WaveEnemies.Length;

        // Register to enemy dead delegate/event
        foreach (var enemy in m_spawnerSystem.WaveEnemies) {
            if (enemy.TryGetComponent<HealthComponent>(out HealthComponent enemyHealthComponent))
            {
                enemyHealthComponent.OnDeath += () => m_enemyCount--;
            }
        }

        OnRoundStart?.Invoke(m_currentRound);
        m_currentRound++;
        return true;
    }

    [ContextMenu("End current Round")]
    public bool EndRound() {
        if (Status == RoundStatus.UpgradePhase || Status == RoundStatus.Finished)
            return false;

        // Last Round is finished
        if (m_currentRound == m_maxRounds)
        {
            Status = RoundStatus.Finished;
            OnFinish?.Invoke();
            return true;
        }

        Status = RoundStatus.UpgradePhase;

        OnUpgradePhaseStart?.Invoke();

        // Enable Upgrade table
        m_upgradeSystem.Enable();

        return true;
    }

    public bool AllEnemiesDead() {
        return m_enemyCount == 0;
    }

    public void DestroyDeadEnemies() {
        foreach (var enemy in m_spawnerSystem.WaveEnemies)
        {
            Destroy(enemy);
        }
    }

    void Start()
    {
        m_maxRounds = m_spawnerSystem.WaveCount;
        OnUpgradePhaseStart += DestroyDeadEnemies;
        m_upgradeSystem.OnCloseWindow += () => StartCoroutine(StartCountdown());

        OnRoundStart += (x) => Debug.Log($"Round {x} started");
        OnUpgradePhaseStart += () => Debug.Log("Round finished");
        OnFinish += () => Debug.LogError("Finished");

        m_countdown = m_countdownStart;
        MenuManager.ShowMenu("Controls Splash");
        //StartNextRound();
    }

    void Update() {
        // Debug.Log($"Round: {m_currentRound}, Enemies: {m_enemyCount}");
        if (AllEnemiesDead()) {
            EndRound();
        }
    }

    public IEnumerator StartCountdown() {
        // Spawn Decals
        SpawnDecals();
        OnCountdownStart?.Invoke();


        while (m_countdown > -1) {
            
            OnCountdownChange?.Invoke(m_countdown);
            Debug.LogError($"Countdown: {m_countdown}");
            m_countdown--;
            yield return new WaitForSeconds(1);
        }

        // Despawn Decals
        DespawnDecals();
        OnCountdownEnd?.Invoke();

        StartNextRound();
        m_countdown = m_countdownStart;
    }

    private void SpawnDecals() {
        Vector3[] enemyPositions = m_spawnerSystem.GetNextWaveEnemyPositions();

        foreach (Vector3 position in enemyPositions) {
            m_decals.Add(Instantiate(m_spawnDecals, position, Quaternion.Euler(90.0f, 0.0f, 0.0f)));
        }
    }

    private void DespawnDecals() {
        foreach (GameObject decal in m_decals) {
            Destroy(decal);
        }

        m_decals.Clear();
    }
}

public enum RoundStatus {
    Started,
    UpgradePhase,
    Finished
}