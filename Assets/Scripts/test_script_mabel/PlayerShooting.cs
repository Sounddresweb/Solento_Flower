using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public BulletPool bulletPool;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletPool == null || firePoint == null)
        {
            Debug.LogWarning("Falta asignar BulletPool o FirePoint en PlayerShooting.");
            return;
        }

        GameObject bullet = bulletPool.GetBullet();

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
        bullet.SetActive(true);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.linearVelocity = firePoint.right * bulletSpeed;
    }
}