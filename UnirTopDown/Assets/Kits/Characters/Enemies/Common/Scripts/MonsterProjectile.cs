using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class MonsterProjectile : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float maxRange = 8f;
    [SerializeField] bool rotateToDirection = true; 

    private Vector2 direction;
    private GameObject owner;
    private Vector2 startPosition;

    private Rigidbody2D body;
    private bool hasHit = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0f;
    }

    public void Launch(Vector2 dir, GameObject shooter)
    {
        direction = dir.normalized;
        owner = shooter;
        startPosition = transform.position;

        body.linearVelocity = direction * speed;

        if (rotateToDirection)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void Update()
    {
        if (Vector2.Distance(startPosition, transform.position) >= maxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        if (other.gameObject == owner) return;
        if (other.GetComponent<MonsterProjectile>() != null) return;

        var character = other.GetComponent<BaseCharacter>();
        if (character != null)
        {
            hasHit = true;
            character.NotifyAttack1(direction);
            DestroyProjectile();
        }
    }

    private void DestroyProjectile()
    {
        body.linearVelocity = Vector2.zero;
        body.simulated = false;
        Destroy(gameObject);
    }
}