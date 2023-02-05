using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    Transform[] spawnPoints;
    int lastSpawn = -1;

    public float randomRadiusRange = 5.0f;

    void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    public void Spawn(int _quantityToSpawn, GameObject _toSpawn)
    {
        for (int i = 0; i < _quantityToSpawn; i++)
        {
            lastSpawn = Mathf.Max(1, (lastSpawn + 1) % spawnPoints.Length);
            
            Instantiate(_toSpawn, GetPositionInRange(spawnPoints[lastSpawn].position), spawnPoints[lastSpawn].rotation);
        }
    }

    Vector3 GetPositionInRange(Vector3 _origin)
    {
        Vector3 offset = Random.insideUnitSphere * randomRadiusRange;
        return _origin + new Vector3(offset.x, 0, offset.z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, randomRadiusRange);
    }
}
