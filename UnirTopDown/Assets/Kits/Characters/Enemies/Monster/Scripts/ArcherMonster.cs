using UnityEngine;

public class ArcherMonster : BaseMonster
{
    [SerializeField] float shootRange = 6f;
    [SerializeField] float fleeRange = 3f;
    [SerializeField] float attackCooldown = 2.5f;
    [SerializeField] GameObject arrowPrefab;

    private float lastAttackTime;

    protected override void UpdateBehavior()
    {
        var target = sight.GetClosestTargetInSight();
        if (target == null) { SetMovementDirection(Vector2.zero); return; }

        Vector2 toTarget = target.transform.position - transform.position;
        float dist = toTarget.magnitude;

        if (dist < fleeRange)
        {
            SetMovementDirection(-toTarget.normalized);
        }
        else if (dist <= shootRange)
        {
            SetMovementDirection(Vector2.zero);
            TryShoot(target, toTarget.normalized);
        }
        else
        {
            SetMovementDirection(toTarget.normalized);
        }
    }

    private void TryShoot(Collider2D target, Vector2 dir)
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        lastAttackTime = Time.time;
        PlayAttackSound();
        animator.SetTrigger("Attack1");

        if (arrowPrefab != null)
        {
            var arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            arrow.GetComponent<MonsterProjectile>()?.Launch(dir, gameObject);
        }
    }
}