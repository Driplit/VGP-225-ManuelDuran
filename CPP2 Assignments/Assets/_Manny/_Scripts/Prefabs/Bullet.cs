using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifetime = 5f;  // Bullet lifetime before being destroyed
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, lifetime); // Destroy bullet after a few seconds to avoid it lingering
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Ensure the player has the "Player" tag
        {
            // Get the Health script attached to the player and call Hurt() to apply damage
            PickupManager playerHealth = other.GetComponent<PickupManager>();
            if (playerHealth != null)
            {
                playerHealth.Hurt(1);  // Adjust the damage as necessary (default is 10)
            }

            Destroy(gameObject);  // Destroy the bullet on impact
        }
        else if (other.CompareTag("Enemy") || other.CompareTag("Wall"))  // Handle other collisions (optional)
        {
            Destroy(gameObject);  // Destroy the bullet on impact with other objects
        }
    }
}