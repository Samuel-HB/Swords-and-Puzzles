using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public Directions direction = new Directions();
    
    public int health = 0;


    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) {
            PlayerLose();
        }
    }

    private void PlayerLose()
    {
        print("player lose");
    }
}
