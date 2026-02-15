using UnityEngine;

[RequireComponent(typeof(Sight2D))]
public class BaseMonster : BaseCharacter
{
    Sight2D sight;

    protected override void Awake()
    {
        base.Awake();
        sight = GetComponent<Sight2D>();        
    }

    protected override void Update()
    {
        var closestTarget = sight.GetClosestTargetInSight();
        if (closestTarget != null) {
            base.SetMovementDirection((closestTarget.transform.position - transform.position).normalized);
        } else
        {
            base.SetMovementDirection(Vector2.zero);
        }

        base.Update();
    }
}
