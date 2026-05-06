using System;
using System.Collections;
using UnityEngine;

public class ShootOnPlayer : PlayerDetection
{

    private bool canShoot = true;

    private Transform[] bullets = new Transform[100];
    private int bulletIndex = 0;
    [HideInInspector] public Transform target;
    [SerializeField] private Transform bulletPrefab;

    [SerializeField] private float bulletSpeed = 20f;
    private float shootDuration = 2f;
    private float durationBetweenShoots = 0.05f;
    private int bulletsGap = 0;

    // maxBulletsGap can be (maxBulletByShootBurst / 2) to have natural shooting shape (but became more unpredictable)
    private int maxBulletsGap = 10; // sould be equal to maxBulletByShootBurst,
                                    // unless there should be several waves for a single detection
    private int maxBulletByShootBurst = 10;
    private int bulletAngle = 1;
    private float spacingMultiplier = 2f;

    [NonSerialized] public Vector3 shotDirection = new Vector3(0, 0);


    protected void InstantiateBullets()
    {
        bulletsGap = maxBulletsGap;

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            bullets[i].GetComponent<SpriteRenderer>().enabled = false;

            if (bullets[i].TryGetComponent<EnnemyBullet>(out EnnemyBullet ennemyBullet)) {
                ennemyBullet.enabled = false;
            }
        }
    }

    protected void Shoot()
    {
        if (canShoot)
        {
            target = transformDetected;
            StartCoroutine(BulletBurst());
        }
    }

    IEnumerator BulletBurst()
    {
        canShoot = false;

        int bulletShootedAmount = 0;
        while (bulletShootedAmount < maxBulletByShootBurst)
        {
            bulletShootedAmount++;
            ActivateBullet(bullets[bulletIndex]);

            bulletIndex = bulletIndex < bullets.Length - 1 ?
                bulletIndex += 1 : bulletIndex = 0;

            yield return new WaitForSeconds(durationBetweenShoots);
        }
        canShoot = true;
    }

    private void ActivateBullet(Transform bullet)
    {
        bullet.position = transform.position;

        bullet.GetComponent<SpriteRenderer>().enabled = true;

        if (bullet.TryGetComponent<EnnemyBullet>(out EnnemyBullet ennemyBullet)) {
            ennemyBullet.enabled = true;
        }
        StartCoroutine(ShootOnPlayerTimer(bullet));
    }


    IEnumerator ShootOnPlayerTimer(Transform bullet)
    {
        float time = 0f;
        shotDirection = target.transform.position - transform.position;
        bulletAngle *= -1;

        EventManager.ShootOnPlayer();

        // new variable to have "shotDirection" outside the scope and public while "direction" stays in this scope
        // and unafacted by the modifications on "shotDirection" (so remains the same during the while loop below)
        Vector3 direction = (Quaternion.AngleAxis(bulletsGap * spacingMultiplier * bulletAngle, Vector3.forward)
                            * shotDirection).normalized;

        bulletsGap--;
        if (bulletsGap <= 0) {
            bulletsGap = maxBulletsGap;
        }

        while (time < shootDuration)
        {
            time += Time.deltaTime; ;
            bullet.position += direction * bulletSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
