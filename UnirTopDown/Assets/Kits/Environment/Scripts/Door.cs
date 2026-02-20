using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject canvasMessage;
    [SerializeField] ZoneController zoneController;
    private bool isLocked = true;

    private void Awake()
    {
        canvasMessage.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        canvasMessage.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        canvasMessage.SetActive(false);
    }

    public void setIsUnlocked()
    { 
        isLocked = false;
    }
}
