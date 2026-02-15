using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseCharacter : MonoBehaviour
{
    SpriteRenderer spriteRend;
    Rigidbody2D body;
    protected Animator animator;

    [SerializeField] float movementSpeed = 5;

    private Vector2 lookDir;
    private Vector2 movementDir;

    protected virtual void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (movementDir != Vector2.zero) {
            lookDir = movementDir;
        }

        body.position += movementSpeed * movementDir * Time.deltaTime;

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

    internal void NotifyAttack1()
    {
        Destroy(gameObject);    
    }
}
