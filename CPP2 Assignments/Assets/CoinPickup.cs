using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PickupManager coin = other.GetComponent<PickupManager>();
            if (coin != null)
            {
                coin.AddCoin(10);  // Adjust the damage as necessary (default is 10)
            }

            Destroy(gameObject);  // Destroy the bullet on impact
        }
    }
}
