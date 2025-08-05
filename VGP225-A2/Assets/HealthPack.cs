using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Ensure the player has the "Player" tag
        {
            // Get the Health script attached to the player and call Hurt() to apply damage
            PickupManager playerHealth = other.GetComponent<PickupManager>();
            if (playerHealth != null)
            {
                playerHealth.Heal(10);  // Adjust the damage as necessary (default is 10)
            }

            Destroy(gameObject);  // Destroy the bullet on impact
        }
    }
}
