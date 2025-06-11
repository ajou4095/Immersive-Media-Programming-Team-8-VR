using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    int maxHp = 10;
    int hp;

    [SerializeField] Player player;

    [SerializeField] int damagePerAttack = 2;

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
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

}
