using UnityEngine;

public class Ennemy : MonoBehaviour, IDamageable
{
    private int health = 0;

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    //private void OnTriggerEnter2D(Collider2D coll)
    //{
    //    if (TryGetComponent<IDamageable>(out IDamageable iDamageable))
    //    {
    //        iDamageable.TakeDamage();
    //    }

    //    //IDamageable gus = GetComponent<IDamageable>();
    //    //if (gus != null) {
    //    //    gus.TakeDamage();
    //    //}

    //}
}
