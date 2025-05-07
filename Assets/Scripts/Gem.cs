using UnityEngine;

public class Gem : MonoBehaviour
{
    [SerializeField] int coinValue = 1;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.AddScore(coinValue);
                Destroy(gameObject);
            }
        }
    }
}
