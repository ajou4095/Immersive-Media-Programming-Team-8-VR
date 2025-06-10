using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] int damage = 2;

    private void OnCollisionEnter(Collision collision)
    {
        // Enemy 태그를 가진 오브젝트에만 반응
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
