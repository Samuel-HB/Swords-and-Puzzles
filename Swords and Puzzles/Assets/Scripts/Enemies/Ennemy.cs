using UnityEngine;

public class Ennemy : MonoBehaviour, IDamageable
{

    public int health = 0;

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
