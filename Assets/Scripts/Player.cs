using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] 
    private int maxHp = 10;

    [SerializeField]
    private int currentHp;

    [SerializeField] 
    private Slider healthSlider;

    private void Awake()
    {
        currentHp = maxHp;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateUI();

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void UpdateUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHp / maxHp;
        }
    }

    private void Die()
    {
        Debug.Log("Gameover");

        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.ShowGameOverUI();
        }
    }
}
