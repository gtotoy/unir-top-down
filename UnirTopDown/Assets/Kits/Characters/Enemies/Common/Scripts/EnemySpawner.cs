using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyEntry
{
    public GameObject prefab;
    public int count = 1;
}

[System.Serializable]
public class EnemyGroup
{
    public string groupName;
    public List<EnemyEntry> enemies;
    public Transform spawnPoint;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Groups")]
    [SerializeField] List<EnemyGroup> groups;

    [Header("Settings")]
    [SerializeField] bool spawnOnStart = true;
    [SerializeField] int defaultGroupIndex = 0;
    [SerializeField] float spawnScatterRadius = 1f;

    private List<GameObject> activeEnemies = new();

    private void Start()
    {
        if (spawnOnStart)
            SpawnAllGroupsSequential();
    }

    public void SpawnGroup(int groupIndex)
    {
        if (groupIndex < 0 || groupIndex >= groups.Count)
        {
            Debug.LogWarning($"EnemySpawner: group index {groupIndex} out of range.");
            return;
        }
        StartCoroutine(SpawnGroupCoroutine(groups[groupIndex]));
    }

    public void SpawnAllGroupsSequential(float delayBetweenGroups = 1f)
    {
        StartCoroutine(SpawnAllGroupsCoroutine(delayBetweenGroups));
    }

    private IEnumerator SpawnAllGroupsCoroutine(float delay)
    {
        foreach (var group in groups)
        {
            yield return StartCoroutine(SpawnGroupCoroutine(group));
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator SpawnGroupCoroutine(EnemyGroup group)
    {
        if (group.spawnPoint == null)
        {
            Debug.LogWarning($"EnemySpawner: group '{group.groupName}' has no spawn point assigned.");
            yield break;
        }

        var toSpawn = new List<GameObject>();
        foreach (var entry in group.enemies)
        {
            if (entry.prefab == null)
            {
                Debug.LogWarning($"EnemySpawner: null prefab in group '{group.groupName}', skipping.");
                continue;
            }
            for (int i = 0; i < entry.count; i++)
                toSpawn.Add(entry.prefab);
        }

        Shuffle(toSpawn);

        foreach (var prefab in toSpawn)
        {
            SpawnOne(prefab, group.spawnPoint);
        }
    }

    private void SpawnOne(GameObject prefab, Transform point)
    {
        Vector2 scatter = Random.insideUnitCircle * spawnScatterRadius;
        Vector3 spawnPos = point.position + new Vector3(scatter.x, scatter.y, 0);

        var enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        activeEnemies.Add(enemy);

        var life = enemy.GetComponent<Life>();
        if (life != null)
            life.OnDeath.AddListener(() => activeEnemies.Remove(enemy));
    }

    public bool AllEnemiesDead() => activeEnemies.Count == 0;

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private void OnDrawGizmos()
    {
        if (groups == null) return;
        foreach (var group in groups)
        {
            if (group.spawnPoint == null) continue;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(group.spawnPoint.position, 0.4f);
            Gizmos.DrawWireSphere(group.spawnPoint.position, spawnScatterRadius);
            Gizmos.DrawLine(transform.position, group.spawnPoint.position);
        }
    }

    public int getActiveEnemies() => activeEnemies.Count;
}