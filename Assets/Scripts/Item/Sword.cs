using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] int damage = 2;

    private bool isHeld = false;
    public void SetHeldTrue()
    {
        isHeld = true;
    }

    public void SetHeldFalse()
    {
        isHeld = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isHeld) return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
