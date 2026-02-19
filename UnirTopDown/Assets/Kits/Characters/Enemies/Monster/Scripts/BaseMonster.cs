using UnityEngine;

[RequireComponent(typeof(Sight2D))]
public class BaseMonster : BaseCharacter
{
    protected Sight2D sight;

    protected override void Awake()
    {
        base.Awake();
        sight = GetComponent<Sight2D>();        
    }

    protected override void Update()
    {
        UpdateBehavior();
        base.Update();
    }

    protected virtual void UpdateBehavior()
    {
        var target = sight.GetClosestTargetInSight();
        SetMovementDirection(target != null
            ? (target.transform.position - transform.position).normalized
            : Vector2.zero);
    }
}
