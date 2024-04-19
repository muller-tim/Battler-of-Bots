using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : MonoBehaviour {
    #region Public Events

    /// <summary>
    /// Event is called when all Rounds are finished
    /// </summary>
    public event Action OnFinish;

    /// <summary>
    /// Event is called on every round start
    /// </summary>
    public event Action<int> OnRoundStart;

    /// <summary>
    /// Event is called on every upgrade phase start
    /// </summary>
    public event Action OnUpgradePhaseStart;

    /// <summary>
    /// Event is called when the countdown starts
    /// </summary>
    public event Action OnCountdownStart;

    /// <summary>
    /// Event is called when the countdown changes, so on every count down
    /// </summary>
    public event Action<int> OnCountdownChange;

    /// <summary>
    /// Event is called when the countdown reaches 0
    /// </summary>
    public event Action OnCountdownEnd;

    #endregion

    #region Public Properties

    /// <summary>
    /// Current Status of the system
    /// </summary>
    public RoundStatus Status
    {
        get => m_status;
        private set => m_status = value;
    }

    /// <summary>
    /// Current round
    /// </summary>
    public int Round => m_currentRound;

    #endregion

    #region Public Methods

    /// <summary>
    /// Starts the next Round 
    /// </summary>
    /// <returns></returns>
    [ContextMenu("Start next Round")]
    public bool StartNextRound()
    {
        // Round can only start, when the status is UpgradePhase
        if (Status == RoundStatus.Started || Status == RoundStatus.Finished)
            return false;

        Status = RoundStatus.Started;

        if (m_currentRound == 0)
        {
            m_gameTimer.StartTimer();
        }
        if (m_currentRound == 8)
        {
            m_trainAudioClip.Play();
        }

        // Spawn wave and get enemies
        m_spawnerSystem.SpawnNextWave();
        m_enemyCount = m_spawnerSystem.WaveEnemies.Length;

        // Register to enemy dead delegate/event
        foreach (var enemy in m_spawnerSystem.WaveEnemies)
        {
            if (enemy.TryGetComponent<HealthComponent>(out HealthComponent enemyHealthComponent))
            {
                enemyHealthComponent.OnDeath += () => m_enemyCount--;
            }
        }

        OnRoundStart?.Invoke(m_currentRound);
        m_currentRound++;
        return true;
    }

    /// <summary>
    /// Ends the current round and starts UpgradePhase. Round can only be ended when a round was started
    /// </summary>
    /// <returns></returns>
    [ContextMenu("End current Round")]
    public bool EndRound()
    {
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

    /// <summary>
    /// Check if all enemies are dead
    /// </summary>
    /// <returns>True when all enemies are dead.</returns>
    public bool AllEnemiesDead()
    {
        return m_enemyCount == 0;
    }

    /// <summary>
    /// Destroys the dead enemies
    /// </summary>
    public void DestroyDeadEnemies()
    {
        foreach (var enemy in m_spawnerSystem.WaveEnemies)
        {
            Destroy(enemy);
        }
    }

    /// <summary>
    /// Starts the countdown
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartCountdown()
    {
        // Spawn Decals
        SpawnDecals();
        OnCountdownStart?.Invoke();


        while (m_countdown > -1)
        {

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

    #endregion


    #region Unity Methods

    void Start()
    {
        m_maxRounds = m_spawnerSystem.WaveCount;
        OnUpgradePhaseStart += DestroyDeadEnemies;
        m_upgradeSystem.OnCloseWindow += () => StartCoroutine(StartCountdown());

        //OnRoundStart += (x) => Debug.Log($"Round {x} started");
        //OnUpgradePhaseStart += () => Debug.Log("Round finished");
        //OnFinish += () => Debug.LogError("Finished");

        m_countdown = m_countdownStart;
        MenuManager.ShowMenu("Controls Splash");
        //StartNextRound();
    }

    void Update()
    {
        // Debug.Log($"Round: {m_currentRound}, Enemies: {m_enemyCount}");
        if (AllEnemiesDead())
        {
            EndRound();
        }
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Spawns decals on every enemy spawn position
    /// </summary>
    private void SpawnDecals()
    {
        Vector3[] enemyPositions = m_spawnerSystem.GetNextWaveEnemyPositions();

        foreach (Vector3 position in enemyPositions)
        {
            m_decals.Add(Instantiate(m_spawnDecals, position, Quaternion.Euler(90.0f, 0.0f, 0.0f)));
        }
    }

    /// <summary>
    /// Destroys all Decals which were spawned from <see cref="SpawnDecals"/>
    /// </summary>
    private void DespawnDecals()
    {
        foreach (GameObject decal in m_decals)
        {
            Destroy(decal);
        }

        m_decals.Clear();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Reference to a spawner system
    /// </summary>
    [SerializeField] private SpawnerSystem m_spawnerSystem;

    /// <summary>
    /// Reference to an upgrade system
    /// </summary>
    [SerializeField] private UpgradeSystem m_upgradeSystem;

    /// <summary>
    /// Reference to a GameTimer
    /// </summary>
    [SerializeField] public GameTimer m_gameTimer;

    /// <summary>
    /// Decals to be spawned at enemy spawn positions
    /// </summary>
    [SerializeField] private GameObject m_spawnDecals;

    /// <summary>
    /// Special audio
    /// </summary>
    [SerializeField] private AudioSource m_trainAudioClip;

    /// <summary>
    /// Max rounds of the game
    /// </summary>
    private int m_maxRounds;

    /// <summary>
    /// Current round 
    /// </summary>
    private int m_currentRound = 0;

    /// <summary>
    /// Current alive enemycount
    /// </summary>
    private int m_enemyCount = 0;

    /// <summary>
    /// Status of the system
    /// </summary>
    private RoundStatus m_status = RoundStatus.UpgradePhase;

    /// <summary>
    /// Countdown Start
    /// </summary>
    [SerializeField] private int m_countdownStart = 3;

    /// <summary>
    /// Current countdown
    /// </summary>
    private int m_countdown;

    /// <summary>
    /// Spawned decals
    /// </summary>
    private List<GameObject> m_decals = new List<GameObject>();

    #endregion

}

public enum RoundStatus {
    Started,
    UpgradePhase,
    Finished
}