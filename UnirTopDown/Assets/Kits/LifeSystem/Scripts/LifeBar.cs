using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    [SerializeField] Image imageFill;

    public void HandleOnLifeChanged(float newLife)
    {
        imageFill.fillAmount = newLife;
    }
}
