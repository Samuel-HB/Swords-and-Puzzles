using UnityEngine;
using System.Collections;

public class ShootOnPlayer : MonoBehaviour
{
    private bool canShoot = true;

    [SerializeField] private Transform bulletPrefab;
    private Transform bullet;
    public Transform target;

    private float bulletSpeed = 10f;
    private float timerDuration = 2f;

    public void Shoot()
    {
        if (canShoot)
        {
            bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            StartCoroutine(ShootOnPlayerTimer());
            canShoot = false;
        }
    }

    IEnumerator ShootOnPlayerTimer()
    {
        float time = 0f;
        Vector3 direction = (target.transform.position - transform.position).normalized;

        while (time < timerDuration)
        {
            time += Time.deltaTime; ;
            bullet.position += direction * bulletSpeed * Time.deltaTime;
            yield return null;
        }
        canShoot = true;
    }
}
