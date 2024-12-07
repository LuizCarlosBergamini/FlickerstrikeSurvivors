using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{

    public EnemyScriptableObject enemyStats;
    private bool isPlayerInRange = false;
    private Coroutine damageCoroutine;
    private PlayerAttack playerAttack;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("PlayerAttack Variabel: " + playerAttack);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackRange"))
        {
            Debug.Log("entered Attack Range");
            other.gameObject.GetComponent<PlayerAttack>().enemiesInRange.Add(gameObject.GetComponent<EnemyMovement>()); playerAttack = other.gameObject.GetComponent<PlayerAttack>();
            playerAttack = other.gameObject.GetComponent<PlayerAttack>();
        }

        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (damageCoroutine == null && playerAttack != null && !playerAttack.isAttacking && playerController != null)
            {
                damageCoroutine = StartCoroutine(DealDamageOverTime(playerController));
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackRange"))
        {
            Debug.Log("entered Attack Range");
            other.gameObject.GetComponent<PlayerAttack>().enemiesInRange.Remove(gameObject.GetComponent<EnemyMovement>());
        }

        if (other.gameObject.CompareTag("Player"))
        {
            

            isPlayerInRange = false;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DealDamageOverTime(PlayerController playerController)
    {
        while (isPlayerInRange)
        {
            if (playerController != null)
            {
                Debug.Log("Dealing damage to player");
                playerController.TakeDamage(enemyStats.Damage);
            }
            yield return new WaitForSeconds(1f);
        }
    }

}
