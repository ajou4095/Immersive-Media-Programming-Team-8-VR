using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("HP Settings")]
    [SerializeField]
    int maxHp = 10;
    int hp;

    [SerializeField] Player player;

    [SerializeField] int damagePerAttack = 2;

    [Header("Damage Cooldown Settings")]
    [SerializeField] float damageCooldown = 0.5f;
    private bool canTakeDamage = true;

    private void Awake()
    {
        hp = maxHp;
        if (player == null)
        {
            player = FindAnyObjectByType(typeof(Player)) as Player;
        }
    }

    public void TakeDamage(int damage)
    {
        if (canTakeDamage)
        {
            hp -= damage;



            if (hp <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator DamageCooldownCoroutine()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }
}
