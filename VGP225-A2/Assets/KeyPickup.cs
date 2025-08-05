using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PickupManager key = other.GetComponent<PickupManager>();
            if (key != null)
            {
                key.AddKey(1);  // Adjust the damage as necessary (default is 10)
            }

            Destroy(gameObject);  // Destroy the bullet on impact
        }
    }
}