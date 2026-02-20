using UnityEngine;

public class LancerMonster : BaseMonster
{
    [SerializeField] float preferredRange = 2f;
    [SerializeField] float attackRange = 2.5f;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] float attackDamage = 0.1f;

    private float lastAttackTime;

    protected override void UpdateBehavior()
    {
        var target = sight.GetClosestTargetInSight();
        if (target == null) { SetMovementDirection(Vector2.zero); return; }

        Vector2 toTarget = target.transform.position - transform.position;
        float dist = toTarget.magnitude;

        if (dist > attackRange)
        {
            SetMovementDirection(toTarget.normalized);
        }
        else if (dist < preferredRange)
        {
            SetMovementDirection(-toTarget.normalized);
        }
        else
        {
            SetMovementDirection(Vector2.zero);
            TryAttack(target, toTarget.normalized);
        }
    }

    private void TryAttack(Collider2D target, Vector2 dir)
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        lastAttackTime = Time.time;
        PlayAttackSound();
        animator.SetTrigger("Attack1");

        var hits = Physics2D.CircleCastAll(transform.position, 0.3f, dir, attackRange);
        foreach (var hit in hits)
        {
            var character = hit.collider.GetComponent<BaseCharacter>();
            if (character != null && character != this)
                character.NotifyAttack1(dir, attackDamage);
        }
    }
}