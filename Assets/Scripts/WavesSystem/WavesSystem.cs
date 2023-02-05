using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesSystem : MonoBehaviour
{
    public SpawnPoint[] SpawnPoints;
    public static int cycleIndex = 1;

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
                if (i == waves.Count - 2)
                {
                    GlobalEvents.Instance.SendEvent(GlobalEventEnum.UpdateLootText, "Congrats! Here's some seeds' mage as a reward, then difficulty goes up!");
                    GlobalEvents.Instance.SendEvent(GlobalEventEnum.ShowHideLootAppeared, true);
                    yield return new WaitForSeconds(3.0f);
                }

                GlobalEvents.Instance.SendEvent(GlobalEventEnum.ShowHideNextWaveTimer, false);

                currentSpawnCoroutine = StartCoroutine(SpawnWave(waves[i]));

                yield return new WaitWhile(() => currentSpawnCoroutine != null);

                GlobalEvents.Instance.SendEvent(GlobalEventEnum.ShowHideNextWaveTimer, true);
                GlobalEvents.Instance.SendEvent(GlobalEventEnum.UpdateNextWaveTimer, (int)globalDelayBetweenWaves);
                shouldTrackTime = true;
                currentTimer = globalDelayBetweenWaves;
                lastUIUpdate = (int)globalDelayBetweenWaves;

                yield return new WaitForSeconds(globalDelayBetweenWaves); // should be high to let the players explore a bit
            }

            cycleIndex++;
        }
    }

    bool shouldTrackTime = false;
    float currentTimer = 0;
    int lastUIUpdate = 0;
    private void Update()
    {
        if (!shouldTrackTime)
            return;

        currentTimer -= Time.deltaTime;
        if ((int)currentTimer != lastUIUpdate && currentTimer >= 0.0f)
        {
            lastUIUpdate = (int)currentTimer;
            GlobalEvents.Instance.SendEvent(GlobalEventEnum.UpdateNextWaveTimer, lastUIUpdate);
        }

        if (currentTimer <= 0.0f)
            shouldTrackTime = false;
    }

    int currentSpawnPoint = 0;
    IEnumerator SpawnWave(WaveSO _waveToSpawn)
    {
        if (_waveToSpawn != null)
        {
            currentSpawnPoint = (currentSpawnPoint + 1) % SpawnPoints.Length;
            if (useDelaysInWavesData)
            {
                foreach (var spawnData in _waveToSpawn.spawnDatas)
                {
                    if ((int)spawnData.enemyType >= prefabs.Count || prefabs[(int)spawnData.enemyType] == null)
                        continue;

                    SpawnPoints[currentSpawnPoint].Spawn((int)(spawnData.quantity * GetQtyMultiplier()), prefabs[(int)spawnData.enemyType]);
                    currentSpawnPoint = (currentSpawnPoint + 1) % SpawnPoints.Length;

                    if (spawnData.enemyType == EnemyType.Treasure)
                    {
                        if (spawnData.quantity == 1)
                            GlobalEvents.Instance.SendEvent(GlobalEventEnum.UpdateLootText, "A mage with lot of seeds appeared in the forest!");
                        else
                            GlobalEvents.Instance.SendEvent(GlobalEventEnum.UpdateLootText, "Several mages with lot of seeds appeared in the forest!");
                        GlobalEvents.Instance.SendEvent(GlobalEventEnum.ShowHideLootAppeared, true);
                    }

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

    public static float GetHealthMultiplier()
    {
        // 1 + (0.5f * (cycleIndex - 1)) => x1 premier cycle, puis x1.5, x2 etc
        return 1 + (0.5f * (cycleIndex - 1));
    }

    public static float GetQtyMultiplier()
    {
        // 1 + (0.5f * (cycleIndex - 1)) => x1 premier cycle, puis x1.5, x2 etc
        return 1 + (0.5f * (cycleIndex - 1));
    }

    private void OnDestroy()
    {
        stop = true;
    }
}

