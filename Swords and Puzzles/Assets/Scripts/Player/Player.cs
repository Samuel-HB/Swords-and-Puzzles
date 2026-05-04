using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [HideInInspector] public Directions direction = new Directions();
    [HideInInspector] public PlayerState state = new PlayerState();

    private IEnumerator timer;
    public int health = 0;

    public float bowDuration = 0.5f;
    public float bombDuration = 0.25f;
    public float swordDuration = 0.5f;


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

    public void CallStateTimer(PlayerState newState, float timeToWait)
    {
        StopTimer(); // swords attacks can be canceled by using items
        timer = StateTimer(newState, timeToWait);
        StartCoroutine(timer);
    }

    IEnumerator StateTimer(PlayerState newState, float timeToWait)
    {
        state = newState;
        yield return new WaitForSeconds(timeToWait);
        state = PlayerState.Idle;
        EventManager.BackToIdle();
    }

    public void StopTimer()
    {
        if (timer != null) {
            StopCoroutine(timer);
            timer = null;
        }
    }
}
