using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesSystem : MonoBehaviour
{
    public SpawnPoint[] SpawnPoints;

    [Header("Enemies")]
    [Tooltip("In the order of enemy type enum (Simple, Tank, Thief)")]
    public List<GameObject> prefabs = new List<GameObject>();

    [Header("Waves Data")]
    public List<WaveSO> waves;

    [Header("Global Spawn Delays")]
    public float initialDelay = 10.0f;
    public float globalDelayBetweenWaves = 10.0f; // todo: show on UI

    [Header("Options ")]
    public bool useDelaysInWavesData = true;

    // Used only if useDelaysInWavesData is false. 
    // Then we spawn quantityToSpawnAfterGlobalDelay monsters each globalDelay seconds
    [Tooltip("Used only if useDelaysInWavesData is false. Then we spawn quantityToSpawnAfterGlobalDelay monsters each globalDelay seconds")]
    public float globalDelayBetweenSpawn = 1.0f;
    [Tooltip("Used only if useDelaysInWavesData is false. Then we spawn quantityToSpawnAfterGlobalDelay monsters each globalDelay seconds")]
    public int quantityToSpawnAfterGlobalDelay = 5;

    Coroutine handleWavesCoroutine = null;
    Coroutine currentSpawnCoroutine = null;

    bool stop = false;

    public IEnumerator Start()
    {
        stop = false;
        SpawnPoints = FindObjectsOfType<SpawnPoint>();

        yield return new WaitForSeconds(initialDelay);

        handleWavesCoroutine = StartCoroutine(HandleWaves());

    }


    IEnumerator HandleWaves()
    {
        while (!stop)
        {
            for (int i = 0; i < waves.Count; i++)
            {
                currentSpawnCoroutine = StartCoroutine(SpawnWave(waves[i]));

                yield return new WaitWhile(() => currentSpawnCoroutine != null);

                yield return new WaitForSeconds(globalDelayBetweenWaves); // should be high to let the players explore a bit
            }

            stop = true;
        }
    }

    int currentSpawnPoint = 0;
    IEnumerator SpawnWave(WaveSO _waveToSpawn)
    {
        if (_waveToSpawn != null)
        {
            currentSpawnPoint = 0;
            if (useDelaysInWavesData)
            {
                foreach (var spawnData in _waveToSpawn.spawnDatas)
                {
                    if ((int)spawnData.enemyType >= prefabs.Count || prefabs[(int)spawnData.enemyType] == null)
                        continue;

                    SpawnPoints[currentSpawnPoint].Spawn(spawnData.quantity, prefabs[(int)spawnData.enemyType]);
                    currentSpawnPoint = (currentSpawnPoint + 1) % SpawnPoints.Length;

                    yield return new WaitForSeconds(spawnData.delay);
                }
            }
            else
            {
                // Osef ? pas fini
                //foreach (EnemyType type in _waveToSpawn.GetEnemyTypes())
                //{
                //    int total = _waveToSpawn.GetTotalEnemiesOfType(type);

                //    if (!prefabs.ContainsKey(type))
                //        continue;

                //    for (int i = 0; i < total; i += quantityToSpawnAfterGlobalDelay)
                //    {
                //        SpawnPoints[currentSpawnPoint].Spawn(quantityToSpawnAfterGlobalDelay, prefabs[type]);
                //        currentSpawnPoint = (currentSpawnPoint + 1) % SpawnPoints.Length;

                //        yield return new WaitForSeconds(globalDelayBetweenSpawn);
                //    }
                //}
            }
        }

        yield return new WaitForSeconds(0.1f);

        currentSpawnCoroutine = null;
    }

    private void OnDestroy()
    {
        stop = true;
    }
}

