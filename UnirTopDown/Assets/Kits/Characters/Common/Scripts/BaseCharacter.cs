using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseCharacter : MonoBehaviour
{
    SpriteRenderer spriteRend;
    Rigidbody2D body;
    protected Animator animator;

    [SerializeField] float movementSpeed = 5;

    private Vector2 lookDir;
    private Vector2 movementDir;

    [Header("Hit Effect")]
    [SerializeField] float recoilForce = 5f;
    [SerializeField] float recoilDuration = 0.15f;
    [SerializeField] float tintDuration = 0.2f;

    private bool isRecoiling = false;

    protected virtual void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

    protected void SetMovementDirection(Vector2 dir)
    {
        movementDir = dir;
    }

    internal void NotifyAttack1(Vector2 hitDirection)
    {
        StartCoroutine(RecoilCoroutine(hitDirection));
        StartCoroutine(RedTintCoroutine());
    }

    private IEnumerator RecoilCoroutine(Vector2 direction)
    {
        isRecoiling = true;
        body.linearVelocity = Vector2.zero;
        body.AddForce(direction * recoilForce, ForceMode2D.Impulse);
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
}
