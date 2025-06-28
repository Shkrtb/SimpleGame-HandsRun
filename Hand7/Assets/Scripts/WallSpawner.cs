using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallSpawner : MonoBehaviour
{
    public GameObject[] wallPrefabs; // 6つのプレハブをInspectorで設定
    [Range(0f, 1f)]
    public float[] spawnWeights = new float[6]; // 各プレハブの出現確率

    public float[] lanePositions = new float[] { -4.75f, -1.75f, 1.75f, 4.75f };
    public float spawnZ;

    public float initialSpawnInterval ;     // 最初のスポーン間隔
    public float minSpawnInterval ;       // 最小スポーン間隔
    public float intervalDecreaseRate ;  // 1回あたりの減少量
    public float decreaseIntervalEvery ;   // 間隔を減少させる頻度（秒）

    private float currentSpawnInterval;

    private List<int[]> spawnPatterns = new List<int[]>
    {
        new int[] { 0, 2 },
        new int[] { 0, 3 },
        new int[] { 1, 2 },
        new int[] { 1, 3 }
    };

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(SpawnLoop());
        StartCoroutine(DecreaseIntervalOverTime());
    }

    IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(2f); // 初期待機

        while (true)
        {
            SpawnWalls();
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    IEnumerator DecreaseIntervalOverTime()
    {
        while (currentSpawnInterval > minSpawnInterval)
        {
            yield return new WaitForSeconds(decreaseIntervalEvery);
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - intervalDecreaseRate);
        }
    }

    void SpawnWalls()
    {
        int[] pattern = spawnPatterns[Random.Range(0, spawnPatterns.Count)];

        foreach (int laneIndex in pattern)
        {
            float laneX = lanePositions[laneIndex];
            float randomZ = spawnZ;//+ Random.Range(0f, 1f); // ±1f ランダムなZ座標
            Vector3 spawnPos = new Vector3(laneX, 0, randomZ);

            GameObject prefab = ChooseRandomPrefab();
            Instantiate(prefab, spawnPos, Quaternion.identity);
        }
    }

    GameObject ChooseRandomPrefab()
    {
        float total = 0f;
        foreach (float weight in spawnWeights)
            total += weight;

        float rand = Random.Range(0, total);
        float cumulative = 0f;

        for (int i = 0; i < wallPrefabs.Length; i++)
        {
            cumulative += spawnWeights[i];
            if (rand <= cumulative)
                return wallPrefabs[i];
        }

        return wallPrefabs[wallPrefabs.Length - 1]; // Fallback
    }
}
