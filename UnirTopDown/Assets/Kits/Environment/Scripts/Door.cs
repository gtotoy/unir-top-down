using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject canvasMessage;
    [SerializeField] ZoneController zoneController;
    private bool isLocked = true;
    private Animator anim;

    private void Awake()
    {
        canvasMessage.SetActive(false);
        anim = GetComponent<Animator>();
        anim.SetBool("isLocked", isLocked);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isLocked)
        {
            canvasMessage.SetActive(true);
        }
        else
        {
            canvasMessage.SetActive(false);
            StartCoroutine(OpenDoor());
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (isLocked)
        { 
            canvasMessage.SetActive(false);
        }
    }

    public void setIsUnlocked()
    { 
        isLocked = false;
    }

    private IEnumerator OpenDoor()
    {
        anim.SetBool("isLocked", isLocked);
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
