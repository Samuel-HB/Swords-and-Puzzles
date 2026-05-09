using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour, IUsable
{
    private Player player;
    private Inventory inventory;

    private bool canShoot = true;

    private Transform[] bombs = new Transform[10];
    private int bombIndex = 0;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform bombPrefab;

    [SerializeField] private float throwDistance = 7;
    private float shootDuration = 0.4f;


    public void UseItem()
    {
        if (canShoot) 
        {
            ActivateBomb(bombs[bombIndex]);

            bombIndex = bombIndex < bombs.Length - 1 ?            
                bombIndex += 1 : bombIndex = 0;

            player.CallStateTimer(PlayerState.UsingItem, player.bombDuration);
            inventory.RemoveItem(ref inventory.bombsCount, inventory.bomb);
            EventManager.ThrowBomb();
        }
    }

    private void Start()
    {
        player = GetComponent<Player>();
        inventory = GetComponent<Inventory>();

        for (int i = 0; i < bombs.Length; i++)
        {
            bombs[i] = Instantiate(bombPrefab, shootPoint.position, Quaternion.identity);

            bombs[i].GetComponent<SpriteRenderer>().enabled = false;
            if (bombs[i].TryGetComponent<BombProjectile>(out BombProjectile bomb)) {
                bomb.enabled = false;
            }
        }
    }

    private void ActivateBomb(Transform bomb)
    {
        bomb.position = shootPoint.position;

        bomb.GetComponent<SpriteRenderer>().enabled = true;

        if (bomb.TryGetComponent<BombProjectile>(out BombProjectile bombProjectile)) {
            bombProjectile.enabled = true;
        }
        StartCoroutine(ShootTimer(bomb));
    }

    IEnumerator ShootTimer(Transform bomb)
    {
        Vector3 direction = new Vector3(0, 0);
        direction = GetDirection();
        Vector3 shootStartPoint = new Vector3(0, 0);
        shootStartPoint = shootPoint.position;

        float time = 0f;
        while (time < shootDuration)
        {
            time += Time.deltaTime; ;
            bomb.position = Vector3.Lerp(shootStartPoint + direction / 2,
                                         shootStartPoint + direction * throwDistance,
                                         Mathf.SmoothStep(0f, 1f, time / shootDuration));
            yield return null;
        }
        if (bomb.TryGetComponent<BombProjectile>(out BombProjectile bombProjectile)) {
            bombProjectile.StartWaitBeforeExplodeTimer();
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
