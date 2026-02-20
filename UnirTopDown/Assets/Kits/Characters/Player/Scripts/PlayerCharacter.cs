using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : BaseCharacter
{
    [SerializeField] InputActionReference moveIARef;
    [SerializeField] InputActionReference attack1IARef;
    [SerializeField] InputActionReference blockIARef;

    [Header("Attack 1 data")]
    [SerializeField] float attack1Radius = 0.5f;
    [SerializeField] float attack1Range = 0.5f;
    [SerializeField] float attack1Damage = 0.1f;

    [Header("Drops")]
    [SerializeField] Drop[] dropPrefabsToSpawn;
    [SerializeField] float minSpawnTime = 1f;
    [SerializeField] float maxSpawnTime = 5f;
    [SerializeField] float minSpawnRadius = 2;
    [SerializeField] float maxSpawnRadius = 7;

    private void OnEnable()
    {
        moveIARef.action.Enable();
        moveIARef.action.started += HandleMoveInputAction;
        moveIARef.action.performed += HandleMoveInputAction;
        moveIARef.action.canceled += HandleMoveInputAction;

        attack1IARef.action.Enable();
        attack1IARef.action.performed += HandleAttack1InputAction;

        blockIARef.action.Enable();
        blockIARef.action.started += HandleBlockInputAction;
        blockIARef.action.canceled += HandleBlockInputAction;
    }

    private void OnDisable()
    {
        moveIARef.action.Disable();
        moveIARef.action.started -= HandleMoveInputAction;
        moveIARef.action.performed -= HandleMoveInputAction;
        moveIARef.action.canceled -= HandleMoveInputAction;

        attack1IARef.action.Disable();
        attack1IARef.action.performed -= HandleAttack1InputAction;

        blockIARef.action.Disable();
        blockIARef.action.started -= HandleBlockInputAction;
        blockIARef.action.canceled -= HandleBlockInputAction;
    }

    private async void Start()
    {
        while(!destroyCancellationToken.IsCancellationRequested)
        {
            await Awaitable.WaitForSecondsAsync(Random.Range(minSpawnTime, maxSpawnTime));
            var dropPrefab = dropPrefabsToSpawn[Random.Range(0, dropPrefabsToSpawn.Length)];
            var dropPosition = transform.position + Random.Range(minSpawnRadius, maxSpawnRadius) * (Vector3)Random.insideUnitCircle;
            GameObject.Instantiate(dropPrefab, dropPosition, Quaternion.identity);
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, attack1Dir * attack1Range);
        Gizmos.DrawSphere(transform.position + (Vector3)attack1Dir * attack1Range, attack1Radius);
    }

    private void HandleMoveInputAction(InputAction.CallbackContext obj)
    {
        if (GameUIManager.IsPaused) return;

        var dir = obj.action.ReadValue<Vector2>();
        base.SetMovementDirection(dir);
        if (dir.magnitude > 0) {
            attack1Dir = dir.normalized;
        }
    }

    private void HandleAttack1InputAction(InputAction.CallbackContext obj)
    {
        if (GameUIManager.IsPaused) return;
        PlayAttackSound();
        DoAttack1();
        base.animator.SetTrigger("Attack1");
    }

    private void HandleBlockInputAction(InputAction.CallbackContext obj)
    {
        if (GameUIManager.IsPaused) return;

        IsBlocking = obj.started;
    }

    private Vector2 attack1Dir = Vector2.right;
    private void DoAttack1()
    {
        var hits = Physics2D.CircleCastAll(transform.position, attack1Radius, attack1Dir, attack1Range);
        foreach (var hit in hits) {
            var baseCharacter = hit.collider.GetComponent<BaseCharacter>();
            if (baseCharacter && baseCharacter != this) {
                Vector2 recoilDir = (hit.transform.position - transform.position).normalized;
                baseCharacter.NotifyAttack1(recoilDir, attack1Damage);
            }
        }
    }


}
