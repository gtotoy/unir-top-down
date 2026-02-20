using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseCharacter : MonoBehaviour
{
    protected SpriteRenderer spriteRend;
    protected Rigidbody2D body;
    protected Animator animator;
    protected Life life;

    [SerializeField] float movementSpeed = 5;

    private Vector2 lookDir;
    private Vector2 movementDir;

    [Header("Hit Effect")]
    [SerializeField] float recoilForce = 5f;
    [SerializeField] float recoilDuration = 0.15f;
    [SerializeField] float tintDuration = 0.2f;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip attackSound;

    private bool isRecoiling = false;
    private bool isBlocking = false;
    protected bool IsBlocking
    {
        get => isBlocking;
        set
        {
            isBlocking = value;
            animator.SetBool("IsBlocking", value);
        }
    }

    protected virtual void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        life = GetComponent<Life>();
    }

    protected virtual void Update()
    {
        if (!isRecoiling)
        {
            if (movementDir != Vector2.zero) {
                lookDir = movementDir;
            }

            body.position += movementSpeed * movementDir * Time.deltaTime;
        }
        

        animator.SetFloat("HorizontalVelocity", movementDir.x);
        animator.SetFloat("VerticalVelocity", movementDir.y);

        if (lookDir.x != 0) {
            spriteRend.flipX = lookDir.x < 0 ? true : false;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        var drop = other.GetComponent<Drop>();
        if (drop) {
            life.RecoverHealth(drop.DropDefinition.HealthRecovery);
            drop.NotifyPickedUp();
        }
    }

    public virtual void HandleOnDeath()
    {
        Destroy(gameObject);
    }

    protected void SetMovementDirection(Vector2 dir)
    {
        movementDir = dir;
    }

    protected void PlayAttackSound()
    {
        if (audioSource != null && attackSound != null)
            audioSource.PlayOneShot(attackSound);
    }

    internal void NotifyAttack1(Vector2 hitDirection)
    {
        if (isBlocking)
        {
            StartCoroutine(RecoilCoroutine(hitDirection, 0.5f));
            StartCoroutine(BlockTintCoroutine());
        }
        else
        {
            life.ReceiveDamage(0.1f);
            StartCoroutine(RecoilCoroutine(hitDirection, 1f));
            StartCoroutine(RedTintCoroutine());
        }
    }

    private IEnumerator RecoilCoroutine(Vector2 direction, float forceMultiplier)
    {
        isRecoiling = true;
        body.linearVelocity = Vector2.zero;
        body.AddForce(direction * recoilForce * forceMultiplier, ForceMode2D.Impulse);
        yield return new WaitForSeconds(recoilDuration);
        body.linearVelocity = Vector2.zero;
        isRecoiling = false;
    }

    private IEnumerator RedTintCoroutine()
    {
        spriteRend.color = Color.red;
        yield return new WaitForSeconds(tintDuration);
        spriteRend.color = Color.white;
    }

    private IEnumerator BlockTintCoroutine()
    {
        spriteRend.color = Color.lightSkyBlue;
        yield return new WaitForSeconds(tintDuration);
        spriteRend.color = Color.white;
    }
}
