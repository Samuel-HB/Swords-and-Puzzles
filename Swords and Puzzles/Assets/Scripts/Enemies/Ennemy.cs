using UnityEngine;

public class Ennemy : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject[] items;

    public int health = 0;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) {
            EnemyLose();
        }
    }

    private void EnemyLose()
    {
        GivesItem();
        Destroy(gameObject);
    }

    private void GivesItem()
    {
        if (items.Length == 0) return;

        if (items.Length == 1) {
            Instantiate(items[0], transform.position, Quaternion.identity);
        }
        else {
            Instantiate(items[Random.Range(0, items.Length)], transform.position, Quaternion.identity);
        }
    }
}
