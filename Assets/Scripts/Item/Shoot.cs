using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour
{
    [Header("Shoot Settings")]
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private int damage = 3;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private Transform muzzle;

    [Header("Prefabs & Effects")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private LineRenderer bulletLine;
    [SerializeField] private float lineDisplayTime = 0.05f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (bulletLine != null)
            bulletLine.enabled = false;
    }

    public void Fire()
    {
        // 1. 라인 이펙트 및 레이캐스트
        Ray ray = new Ray(muzzle.position, muzzle.forward);
        Vector3 endPoint = muzzle.position + muzzle.forward * maxDistance;

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            endPoint = hit.point;

            if (hit.collider.CompareTag("Enemy"))
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }

            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }

        if (bulletLine != null)
            StartCoroutine(ShowBulletLine(muzzle.position, endPoint));

        // 2. 총알 프리팹 생성 (시각적 효과용)
        if (bulletPrefab != null)
        {
            GameObject bulletInstance = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
            Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(muzzle.forward * bulletSpeed, ForceMode.Impulse);
            Destroy(bulletInstance, 2f);
        }

        // 3. 사운드
        if (audioSource != null)
            audioSource.Play();
    }

    private IEnumerator ShowBulletLine(Vector3 start, Vector3 end)
    {
        bulletLine.SetPosition(0, start);
        bulletLine.SetPosition(1, end);
        bulletLine.enabled = true;

        yield return new WaitForSeconds(lineDisplayTime);

        bulletLine.enabled = false;
    }
}
