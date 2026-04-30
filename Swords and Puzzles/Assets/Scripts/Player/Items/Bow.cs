using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour, IUsable
{
    private Player player;

    private bool canShoot = true;

    private Transform[] arrows = new Transform[15];
    private int arrowIndex = 0;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform arrowPrefab;

    [SerializeField] private float arrowSpeed = 20;
    private float shootDuration = 2f;
    //private float durationBetweenShoots = 0.05f;

    public void UseItem()
    {
        if (canShoot)
        {
            ActivateArrow(arrows[arrowIndex]);

            if (arrowIndex < arrows.Length - 1)
            {
                arrowIndex++;
            }
            else {
                arrowIndex = 0;
            }
        }
    }

    private void Start()
    {
        player = GetComponent<Player>();

        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i] = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);

            arrows[i].GetComponent<SpriteRenderer>().enabled = false;

            if (arrows[i].TryGetComponent<ArrowProjectile>(out ArrowProjectile arrow)) {
                arrow.enabled = false;
            }
        }
    }

    private void ActivateArrow(Transform arrow)
    {
        arrow.position = shootPoint.position;

        arrow.GetComponent<SpriteRenderer>().enabled = true;

        if (arrow.TryGetComponent<ArrowProjectile>(out ArrowProjectile arrowProjectile)) {
            arrowProjectile.enabled = true;
        }
        StartCoroutine(ShootTimer(arrow));
    }

    IEnumerator ShootTimer(Transform arrow)
    {
        Vector3 direction = new Vector3(0, 0);
        direction = GetDirection();

        float time = 0f;
        while (time < shootDuration)
        {
            time += Time.deltaTime; ;
            arrow.position += direction * arrowSpeed * Time.deltaTime;
            yield return null;
        }
    }

    private Vector2 GetDirection()
    {
        switch (player.direction)
        {
            case Directions.North:
                return new Vector3(0, 1);
            case Directions.South:
                return new Vector3(0, -1);
            case Directions.East:
                return new Vector3(1, 0);
            case Directions.West:
                return new Vector3(-1, 0);
            default:
                return new Vector3(0, 1);
        }
    }
}
