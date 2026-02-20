using UnityEngine;

public class ZoneController : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private GameObject keyFly;
    private bool isPlayerInZone = false;

    private void Update()
    {
        if (isPlayerInZone)
        {
            if (enemySpawner.AllEnemiesDead())
            {
                //Debug.Log("Player has cleared the zone!");
                Instantiate(keyFly, transform.position, Quaternion.identity);
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
