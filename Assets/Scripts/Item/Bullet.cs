using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Settings")]
    public int damage = 5;

    [SerializeField] private float lifeTime = 5f; 

    private void Start()
    {
        Destroy(gameObject, lifeTime); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }
}
