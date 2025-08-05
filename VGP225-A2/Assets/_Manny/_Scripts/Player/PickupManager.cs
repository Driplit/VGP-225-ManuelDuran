using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;  // For handling scenes (game over screen)
using UnityEngine.UI;
public class PickupManager : MonoBehaviour
{
    public int currentHealth = 100;
    public int maxHealth = 100;
    public int currentCoin;
    public int currentKey;
    const int KEYS = 3;
    
    public Text coinText;
    public Text healthText;
    public Text keyText;
    

    public GameObject EndGameDoor;  

    private void Update()
    {
        coinText.text = "Coins: " + currentCoin.ToString();
        healthText.text = "Health: " + currentHealth.ToString();
        keyText.text = "Key Fragments: " + currentKey.ToString() +" / 3";

        if (currentKey  == KEYS)
        {
            Destroy(EndGameDoor);
        }
        
    }
    // Method to apply damage to the player
    public void Hurt(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player hurt! Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0; // Ensure health doesn't go below 0
            Die();
        }
    }
    public void Heal(int heal)
    {
        currentHealth += heal;
        Debug.Log("Player hurt! Current health: " + currentHealth);
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    // Method to handle player death and trigger game over
    private void Die()
    {
        Debug.Log("Player Died! Restarting in 2 seconds...");
   
        Invoke(nameof(RestartGame), 2f);
    }

    // Optional: Method to reset the game if needed
    public void RestartGame()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the scene
    }

    public void AddCoin(int coin)
    {
        currentCoin += coin;
        Debug.Log("MONEY!!!!!: " + currentCoin);
    }
    public void AddKey(int key)
    {
        currentKey += key;
        Debug.Log("Key" +  currentKey);
    }
}