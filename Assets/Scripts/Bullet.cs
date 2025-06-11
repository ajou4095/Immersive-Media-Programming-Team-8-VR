using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 1; 

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
