using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] int damage = 2;

    private bool isHeld = false;

    // Inspector���� ���� ������ �޼���
    public void SetHeldTrue()
    {
        isHeld = true;
    }

    public void SetHeldFalse()
    {
        isHeld = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isHeld) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
