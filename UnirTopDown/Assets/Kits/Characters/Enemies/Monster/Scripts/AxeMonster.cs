using UnityEngine;
using System.Collections;

public class AxeSoldier : BaseMonster
{
    [SerializeField] float attackRange = 1.2f;
    [SerializeField] float attackCooldown = 1.5f;
    [SerializeField] float attackRadius = 0.6f;

    private float lastAttackTime;

    protected override void UpdateBehavior()
    {
        var target = sight.GetClosestTargetInSight();
        if (target == null) { SetMovementDirection(Vector2.zero); return; }

        Vector2 toTarget = target.transform.position - transform.position;
        float dist = toTarget.magnitude;

        if (dist <= attackRange)
        {
            SetMovementDirection(Vector2.zero);
            TryAttack(target);
        }
        else
        {
            SetMovementDirection(toTarget.normalized);
        }
    }

    private void TryAttack(Collider2D target)
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        lastAttackTime = Time.time;
        PlayAttackSound();
        animator.SetTrigger("Attack1");

        Vector2 dir = (target.transform.position - transform.position).normalized;
        var hits = Physics2D.CircleCastAll(transform.position, attackRadius, dir, attackRange);
        foreach (var hit in hits)
        {
            var character = hit.collider.GetComponent<BaseCharacter>();
            if (character != null && character != this)
                character.NotifyAttack1(dir);
        }
    }
}