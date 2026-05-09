using System.Collections;
using UnityEngine;

public class ShotManager : MonoBehaviour
{

    private void Awake()
    {
        InstantiatedKeeper.shotManager = this;
    }

    public void StartShootOnPlayerTimer(Transform bullet, float bulletSpeed, float shootDuration, Vector3 shotDirection)
    {
        StartCoroutine(ShootOnPlayerTimer(bullet, bulletSpeed, shootDuration, shotDirection));
    }

    public IEnumerator ShootOnPlayerTimer(Transform bullet, float bulletSpeed, float shootDuration, Vector3 shotDirection)
    {
        float time = 0f;
        while (time < shootDuration)
        {
            time += Time.deltaTime; ;
            bullet.position += shotDirection * bulletSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
