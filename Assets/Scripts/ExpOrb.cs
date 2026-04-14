    using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int expValue = 30;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                player.AddExp(expValue);
            }

            Destroy(gameObject);
        }
    }
}