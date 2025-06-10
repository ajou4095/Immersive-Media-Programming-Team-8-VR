using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] int damage = 2;

    private void OnCollisionEnter(Collision collision)
    {
        // Enemy �±׸� ���� ������Ʈ���� ����
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
