using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public CharacterMove playerStats;
    public float healthPack = 20f;

    void Start()
    {
        if ((playerStats == null) && (GetComponent<CharacterMove>() != null))
        {
            playerStats = GetComponent<CharacterMove>();
        }
        else
        {
            Debug.LogWarning("Missing CharacterMove component. Please add one");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        { 
            if (playerStats.health < playerStats.maxHealth)
            {
                Destroy(gameObject);
                playerStats.health = playerStats.health + healthPack;
                playerStats.playerHealthBar.fillAmount = playerStats.health / 100f;
                if (playerStats.health > playerStats.maxHealth)
                {
                    playerStats.health = playerStats.maxHealth;
                    playerStats.playerHealthBar.fillAmount = playerStats.health / 100f;
                }
            }
        }
    }
}
