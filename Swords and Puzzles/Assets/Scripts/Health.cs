using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 10;

    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;

        print("health:" + health);
    }
}
