using UnityEngine;

public class ZoneController : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject keyFlyPrefab;
    [SerializeField] private Door door;
    private bool isPlayerInZone = false;

    private void Update()
    {
        if (isPlayerInZone)
        {
            if (enemySpawner.AllEnemiesDead())
            {
                //Debug.Log("Player has cleared the zone!");
                Instantiate(keyFlyPrefab, transform.position, Quaternion.identity);
                door.setIsUnlocked();
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }
}
