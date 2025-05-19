using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Inst { get; private set; }

    [SerializeField] int maxHp;
    [SerializeField] int hp;

    private void Awake()
    {
        if (maxHp > 0)
        {
            hp = maxHp;
        }
    }

    public void TakeDamage(int damage)
    {
        this.hp -= damage;
    }
}
