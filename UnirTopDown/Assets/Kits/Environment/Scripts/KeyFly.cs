using UnityEngine;

public class KeyFly : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Player has collected the key!");
            AudioSource.PlayClipAtPoint(clip, transform.position);
            Destroy(gameObject);
        }
    }
}
