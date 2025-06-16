using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private int damage = 2;
    private bool isHeld = false;

    public void SetHeldTrue() => isHeld = true;
    public void SetHeldFalse() => isHeld = false;

    public bool IsHeld() => isHeld;
    public int GetDamage() => damage;
}
