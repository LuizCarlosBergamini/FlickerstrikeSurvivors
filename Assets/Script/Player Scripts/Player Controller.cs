using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameOverScreen gameOver;
    public PlayerScriptableObject playerStats;
    private float hp;

    public Image healthBar; // Image UI

    void Start()
    {

        if (playerStats != null)
        {
            hp = playerStats.MaxHealth;
        }
        else
        {
            Debug.LogError("PlayerStats is not assigned in the Inspector");
        }
    }

    public void TakeDamage(float damage)
    {
        if (gameObject != null)
        {
            if (healthBar != null && playerStats != null)
            {
                hp -= damage;
                healthBar.fillAmount = hp / playerStats.MaxHealth;

                if (hp <= 0)
                {
                    gameOver.Setup();
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogError("HealthBar or PlayerStats is not assigned in the Inspector");
            }
        }
    }


}
