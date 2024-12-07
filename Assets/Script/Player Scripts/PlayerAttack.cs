using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour {
    public float attackRange = 10f;
    public float areaDamageRadius = 10f;
    public int damage = 5;
    public List<EnemyMovement> enemiesInRange = new List<EnemyMovement>();
    public GameObject player;
    public bool isAttacking = false;
    private int flickerStrikeCount = 3;
    private bool isCooldown = false;
    private float cooldownTime = 4f;
    private float cooldownTimer = 0f;

    public TextMeshProUGUI flickerStrikeCountText; // Text UI
    public TextMeshProUGUI cooldownTimerText; // Text UI
    

    private void Start()
    {
        player = transform.parent.gameObject;
    }

    void Update()
    {
        //Debug.Log(player.transform.position);
        Debug.Log("Inimigos na área: " + enemiesInRange.Count);
        Debug.Log("IsAttacking: " + isAttacking);

        if (Input.GetMouseButtonDown(0) && !isCooldown) // Clique do mouse
        {
            FlickerStrike();
        }

        flickerStrikeCountText.text = "Flicker Strike: " + flickerStrikeCount;
        if (isCooldown)
        {
            cooldownTimer -= Time.deltaTime;
            cooldownTimerText.text = "Cooldown: " + Mathf.Ceil(cooldownTimer).ToString() + "s";
        }
        else
        {
            cooldownTimerText.text = "";
        }

    }

    void FlickerStrike()
    {
        isAttacking = true;
        

        // Encontrar o inimigo mais próximo
        EnemyMovement closestEnemy = FindClosestEnemy();
        if (closestEnemy == null)
        {
            Debug.Log("Nenhum inimigo encontrado");
            return; // Nenhum inimigo encontrado
        }

        if (!isCooldown)
        {
            flickerStrikeCount--;
        }

        // Teleportar para a posição do inimigo mais próximo
        player.transform.position = closestEnemy.transform.position;

        closestEnemy.TakeDamage(damage);

        StartCoroutine(ResetIsAttacking());

        if (flickerStrikeCount <= 0)
        {
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator ResetIsAttacking()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }

    IEnumerator Cooldown()
    {
        isCooldown = true;
        cooldownTimer = cooldownTime;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
        flickerStrikeCount = 3;
    }

    EnemyMovement FindClosestEnemy()
    {
        EnemyMovement closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
