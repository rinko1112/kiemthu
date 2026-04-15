using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Config")]
    public int maxWave = 10;
    public int currentWave = 0;

    public float timeBetweenWaves = 5f;

    [Header("Enemy")]
    public GameObject enemyPrefab;

    [Header("Spawn Points")]
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;

    [Header("Scaling")]
    public int baseEnemyCount = 3;
    public int enemyIncreasePerWave = 2;

    public int hpIncreasePerWave = 2;
    public int atkIncreasePerWave = 1;
    public float spdIncreasePerWave = 0.2f;

    private List<GameObject> aliveEnemies = new List<GameObject>();

    private bool isSpawning = false;
    private bool gameEnded = false;

    // ===== UI =====
    public WaveUI waveUI;
    private int totalEnemiesThisWave = 0;

    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    [System.Obsolete]
    void Update()
    {
        // Update enemy count UI
        if (waveUI != null && currentWave > 0)
        {
            waveUI.SetEnemyCount(aliveEnemies.Count, totalEnemiesThisWave);
        }

        // Check clear wave
        if (!isSpawning && aliveEnemies.Count == 0 && currentWave > 0)
        {
            if (currentWave < maxWave)
            {
                StartCoroutine(StartNextWave());
            }
            else
            {
                Debug.Log("ALL WAVES CLEARED!");
            }
        }
        if (!isSpawning && aliveEnemies.Count == 0 && currentWave >= maxWave && !gameEnded)
{
    gameEnded = true;

    if (waveUI != null)
        waveUI.SetEnemyCount(0, totalEnemiesThisWave);

    GameResultUI ui = FindObjectOfType<GameResultUI>();
    if (ui != null)
    {
        ui.ShowWin();
    }
}
    }

    IEnumerator StartNextWave()
    {
        isSpawning = true;

        float timer = timeBetweenWaves;

        // ===== ĐẾM NGƯỢC =====
        while (timer > 0)
        {
            if (waveUI != null)
                waveUI.SetCountdown(timer);

            timer -= Time.deltaTime;
            yield return null;
        }

        if (waveUI != null)
            waveUI.ClearCountdown();

        currentWave++;

        Debug.Log("WAVE: " + currentWave);

        if (waveUI != null)
            waveUI.SetWave(currentWave);

        int enemyCount = baseEnemyCount + (currentWave * enemyIncreasePerWave);
        totalEnemiesThisWave = enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.3f);
        }

        isSpawning = false;
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = Random.value > 0.5f ? leftSpawnPoint : rightSpawnPoint;

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // ===== SCALE STAT =====
        EnemyStats stats = enemy.GetComponent<EnemyStats>();
        if (stats != null)
        {
            stats.maxHP += hpIncreasePerWave * currentWave;
            stats.currentHP = stats.maxHP;

            stats.attackDamage += atkIncreasePerWave * currentWave;

            stats.moveSpeed += spdIncreasePerWave * currentWave;
        }

        // ===== SKILL =====
        Enemy enemyScript = enemy.GetComponent<Enemy>();

        if (enemyScript != null)
        {
            float skillChance = 0f;

            if (currentWave <= 2)
                skillChance = 0f;
            else if (currentWave <= 5)
                skillChance = 0.2f;
            else
                skillChance = 0.5f;

            if (Random.value < skillChance)
            {
                enemyScript.SendMessage("EnableSkill", SendMessageOptions.DontRequireReceiver);
            }
        }

        aliveEnemies.Add(enemy);

        StartCoroutine(TrackEnemy(enemy));
    }

    IEnumerator TrackEnemy(GameObject enemy)
    {
        while (enemy != null)
        {
            yield return null;
        }

        aliveEnemies.Remove(enemy);
    }
}