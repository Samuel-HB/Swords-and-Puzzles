using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public int health = 0;

    public void TakeDamage()
    {
        health -= 1;
    }
}
