using System.Linq;
using UnityEngine;

public class Sight2D : MonoBehaviour
{
    [SerializeField] float radius = 5f;
    [SerializeField] float checkFrequency = 1.0f;
    [SerializeField] StimuliSource.SourceSide[] perceivedSides;

    private Collider2D closestTarget;

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
            closestTarget = null;
            float distanceToClosestTarget = Mathf.Infinity;
            int priorityOfClosestTarget = -1;
            foreach (var collider in colliders) {
                StimuliSource stimuliSource = collider.GetComponent<StimuliSource>();
                if (stimuliSource && perceivedSides.Contains(stimuliSource.Side))
                {
                    float distanceToStimuliSource = Vector3.Distance(transform.position, collider.transform.position);
                    if (stimuliSource.Priority > priorityOfClosestTarget 
                        || (stimuliSource.Priority == priorityOfClosestTarget && distanceToStimuliSource < distanceToClosestTarget))
                    {
                        closestTarget = collider;
                        distanceToClosestTarget = distanceToStimuliSource;
                        priorityOfClosestTarget = stimuliSource.Priority;
                    }
                }
            }
        }
    }

    public Collider2D GetClosestTargetInSight()
    {
        return closestTarget;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
