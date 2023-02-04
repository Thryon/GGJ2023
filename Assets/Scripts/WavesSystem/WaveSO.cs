using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "ScriptableObjects/Wave", order = 1)]
public class WaveSO : ScriptableObject
{
    public List<SpawnData> spawnDatas;

    public int GetTotalEnemiesOfType(EnemyType _enemyType)
    {
        int total = 0;

        foreach (var spawnData in spawnDatas)
        {
            if (spawnData.enemyType == _enemyType)
                total += spawnData.quantity;
        }


        return total;
    }

    public List<EnemyType> GetEnemyTypes()
    {
        List<EnemyType> types = new List<EnemyType>();

        foreach (var spawnData in spawnDatas)
        {
            if (!types.Contains(spawnData.enemyType))
                types.Add(spawnData.enemyType);
        }

        return types;
    }
}

[System.Serializable]
public struct SpawnData
{
    public EnemyType enemyType;
    public int quantity;
    public float delay;
}
