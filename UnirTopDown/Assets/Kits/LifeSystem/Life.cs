using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
    [SerializeField] float startLife = 1f;
    [SerializeField] float currentLife;

    public UnityEvent<float> OnLifeChanged;
    public UnityEvent OnDeath;

    private void Awake()
    {
        currentLife = startLife;
    }

    public void ReceiveDamage(float damage)
    {
        if (currentLife > 0f)
        {
            currentLife -= damage;
            OnLifeChanged.Invoke(currentLife);

            if (currentLife <= 0) {
                OnDeath.Invoke();
            }
        }
        else
        {
            Debug.LogError("Already dead!", gameObject);
        }
    }

    public void RecoverHealth(float healthRecovery)
    {
        if (currentLife > 0f)
        {
            currentLife += healthRecovery;
            currentLife = Mathf.Clamp01(currentLife);
            OnLifeChanged.Invoke(currentLife);
        }
        else
        {
            Debug.LogError("Already dead!", gameObject);
        }
    }
}
