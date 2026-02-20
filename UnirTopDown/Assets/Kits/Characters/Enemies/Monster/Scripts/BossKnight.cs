using UnityEngine;
using System.Collections;

public class BossKnight : BaseMonster
{
    [Header("Boss Stats")]
    [SerializeField] float dashSpeed = 12f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float attackRange = 1.2f;
    [SerializeField] float attackRadius = 0.6f;

    [Header("Boss Timings")]
    [SerializeField] float minActionDelay = 0.8f;
    [SerializeField] float maxActionDelay = 2f;
    [SerializeField] float blockDuration = 1f;
    [SerializeField] float attackCooldown = 1f;

    [Header("Boss Weights")]
    [SerializeField] float weightAttack = 0.5f;
    [SerializeField] float weightBlock = 0.2f;
    [SerializeField] float weightDash = 0.3f;
    [SerializeField] float attackDamage = 0.3f;

    private enum BossState { Idle, Deciding, Attacking, Blocking, Dashing }
    private BossState state = BossState.Idle;

    private float lastAttackTime;
    private Transform player;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void UpdateBehavior()
    {
        var target = sight.GetClosestTargetInSight();
        if (target != null) player = target.transform;

        if (state == BossState.Idle)
        {
            state = BossState.Deciding;
            StartCoroutine(DecideNextAction());
        }

        if (player != null && state != BossState.Dashing)
        {
            Vector2 toPlayer = player.position - transform.position;
            if (state != BossState.Attacking && state != BossState.Blocking)
                SetMovementDirection(toPlayer.normalized);
        }
    }

    private IEnumerator DecideNextAction()
    {
        yield return new WaitForSeconds(Random.Range(minActionDelay, maxActionDelay));

        if (player == null) { state = BossState.Idle; yield break; }

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        BossState next = PickAction(distToPlayer);

        switch (next)
        {
            case BossState.Attacking: StartCoroutine(DoAttack()); break;
            case BossState.Blocking: StartCoroutine(DoBlock()); break;
            case BossState.Dashing: StartCoroutine(DoDash()); break;
        }
    }

    private BossState PickAction(float distToPlayer)
    {
        float attack = weightAttack;
        float block = weightBlock;
        float dash = weightDash;

        if (distToPlayer > attackRange * 2.5f)
        {
            attack *= 0.2f;
            dash *= 2.5f;
        }
        else if (distToPlayer <= attackRange)
        {
            dash *= 0.2f;
        }

        float total = attack + block + dash;
        float roll = Random.value * total;

        if (roll < attack) return BossState.Attacking;
        else if (roll < attack + block) return BossState.Blocking;
        else return BossState.Dashing;
    }


    private IEnumerator DoAttack()
    {
        state = BossState.Attacking;
        SetMovementDirection(Vector2.zero);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.SetTrigger("Attack1");
            PlayAttackSound();

            yield return new WaitForSeconds(0.15f);

            if (player != null)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                var hits = Physics2D.CircleCastAll(transform.position, attackRadius, dir, attackRange);
                foreach (var hit in hits)
                {
                    var character = hit.collider.GetComponent<BaseCharacter>();
                    if (character != null && character != this)
                        character.NotifyAttack1(dir, attackDamage);
                }
            }
        }

        yield return new WaitForSeconds(0.4f);
        state = BossState.Idle;
    }

    private IEnumerator DoBlock()
    {
        state = BossState.Blocking;
        SetMovementDirection(Vector2.zero);

        IsBlocking = true;
        animator.SetBool("IsBlocking", true);

        yield return new WaitForSeconds(blockDuration);

        IsBlocking = false;
        animator.SetBool("IsBlocking", false);

        state = BossState.Idle;
    }

    private IEnumerator DoDash()
    {
        state = BossState.Dashing;

        Vector2 dashDir = PickDashDirection();
        animator.SetTrigger("Dash");

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            GetComponent<Rigidbody2D>().linearVelocity = dashDir * dashSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }

        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        state = BossState.Idle;
    }

    private Vector2 PickDashDirection()
    {
        if (player == null) return Random.insideUnitCircle.normalized;

        Vector2 toPlayer = (player.position - transform.position).normalized;
        float angleOffset = Random.Range(-45f, 45f);
        return Quaternion.Euler(0, 0, angleOffset) * toPlayer;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
