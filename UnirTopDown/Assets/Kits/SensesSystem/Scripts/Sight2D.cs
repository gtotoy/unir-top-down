using UnityEngine;

public class Sight2D : MonoBehaviour
{
    [SerializeField] float radius = 5f;
    [SerializeField] float checkFrequency = 1.0f;

    private Collider2D closestPlayer;

    private float lasCheckTime;

    private void Awake()
    {
        lasCheckTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > lasCheckTime)
        {
            lasCheckTime = Time.time + checkFrequency;

            var colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            closestPlayer = null;
            float distanceToClosestPlayer = Mathf.Infinity;
            foreach (var collider in colliders) {
                if (collider.CompareTag("Player"))
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, collider.transform.position);
                    if (distanceToPlayer < distanceToClosestPlayer)
                    {
                        closestPlayer = collider;
                        distanceToClosestPlayer = distanceToPlayer;
                    }
                }
            }
        }
    }

    public Collider2D GetClosestTargetInSight()
    {
        return closestPlayer;
    }
}
