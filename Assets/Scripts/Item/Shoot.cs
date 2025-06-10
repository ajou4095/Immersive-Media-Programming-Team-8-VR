using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private float impulseForce = 10;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform muzzle;

    private AudioSource audioSource;
    private bool shoot = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Fire()
    {
        shoot = true;
    }

    void FixedUpdate()
    {
        if (shoot)
        {
            GameObject go = Instantiate(bullet, muzzle.position, muzzle.localRotation);
            go.GetComponent<Rigidbody>().AddForce(impulseForce * muzzle.forward, ForceMode.Impulse);
            audioSource.Play();
            Destroy(go, 2);
            shoot = false;
        }
    }
}
