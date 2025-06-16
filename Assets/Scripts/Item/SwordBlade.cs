using UnityEngine;

public class SwordBlade : MonoBehaviour
{
    public Sword sword;

    private void OnTriggerEnter(Collider other)
    {
        if (!sword.IsHeld()) return;

        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(sword.GetDamage());
                Debug.Log($"적 체력 감소");
            }
        }
    }
}
