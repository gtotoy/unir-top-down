using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LifeBar : MonoBehaviour
{
    [SerializeField] Image imageFill;

    [Header("Avatar")]
    [SerializeField] Image avatarImage;

    [Header("Avatar Hit Effect")]
    [SerializeField] float flashDuration = 0.15f;
    [SerializeField] float shakeDuration = 0.3f;
    [SerializeField] float shakeMagnitude = 8f;

    private Vector3 avatarOriginPosition;
    private Coroutine shakeCoroutine;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        if (avatarImage != null)
            avatarOriginPosition = avatarImage.rectTransform.anchoredPosition;
    }

    public void HandleOnLifeChanged(float newLife)
    {
        if(imageFill.fillAmount > newLife)
        {
            TriggerHitEffect();
        }
        imageFill.fillAmount = newLife;
    }

    public void TriggerHitEffect()
    {
        if (avatarImage == null) return;

        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);

        shakeCoroutine = StartCoroutine(ShakeCoroutine());
        flashCoroutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float strength = shakeMagnitude * (1f - elapsed / shakeDuration);
            Vector2 offset = Random.insideUnitCircle * strength;
            avatarImage.rectTransform.anchoredPosition = avatarOriginPosition + (Vector3)offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        avatarImage.rectTransform.anchoredPosition = avatarOriginPosition;
    }

    private IEnumerator FlashCoroutine()
    {
        avatarImage.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        avatarImage.color = Color.white;
    }
}