using System.Collections;
using UnityEngine;
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

    [Header("Health Bar UI")]
    [SerializeField] GameObject healthBarCanvas;   
    [SerializeField] Slider healthSlider;          
    [SerializeField] float uiDisplayDuration = 3f; 

    private Coroutine hideUICoroutine;

    private void Awake()
    {
        hp = maxHp;

        if (player == null)
        {
            player = FindAnyObjectByType(typeof(Player)) as Player;
        }

        animator = GetComponent<Animator>();

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHp;
            healthSlider.value = hp;
        }

        if (healthBarCanvas != null)
        {
            healthBarCanvas.SetActive(true); 
        }
    }

    public void TakeDamage(int damage)
    {
        if (!canTakeDamage) return;

        hp -= damage;
        hp = Mathf.Clamp(hp, 0, maxHp);

        if (healthSlider != null)
            healthSlider.value = hp;

        if (healthBarCanvas != null)
        {
            healthBarCanvas.SetActive(true);

            if (hideUICoroutine != null)
                StopCoroutine(hideUICoroutine);

            hideUICoroutine = StartCoroutine(HideHealthBarAfterDelay());
        }

        if (hp <= 0)
        {
            Die();
        }

        StartCoroutine(DamageCooldownCoroutine());
    }

    private IEnumerator HideHealthBarAfterDelay()
    {
        yield return new WaitForSeconds(uiDisplayDuration);
        if (healthBarCanvas != null)
            healthBarCanvas.SetActive(false);
    }

    private void Die()
    {
        animator.SetBool("isDead", true);
        Destroy(gameObject, 3.5f);
    }

    private IEnumerator DamageCooldownCoroutine()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }
}
