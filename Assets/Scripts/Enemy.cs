using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("HP Settings")]
    [SerializeField] int maxHp = 10;
    private int hp;

    [Header("Damage Cooldown Settings")]
    [SerializeField] float damageCooldown = 0.5f;
    private bool canTakeDamage = true;

    [Header("Other Settings")]
    [SerializeField] Player player;
    [SerializeField] int damagePerAttack = 2;

    Animator animator;

    private AudioSource audioSource;
    [SerializeField] private AudioClip dieClip;

    private NavMeshAgent agent;

    private Coroutine hideUICoroutine;

    public bool isDead = false;

    private void Awake()
    {
        hp = maxHp;

        if (player == null)
        {
            player = FindAnyObjectByType(typeof(Player)) as Player;
        }

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damage)
    {
        if (!canTakeDamage) return;

        hp -= damage;
        hp = Mathf.Clamp(hp, 0, maxHp);


        if (hp <= 0 && !isDead)
        {
            Die();
        }

        StartCoroutine(DamageCooldownCoroutine());
    }

    

    private void Die()
    {
        animator.SetTrigger("isDead");
        agent.isStopped = true;
        audioSource.PlayOneShot(dieClip);
        isDead = true;
        Destroy(gameObject, 3.5f);
    }

    private IEnumerator DamageCooldownCoroutine()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }
}
