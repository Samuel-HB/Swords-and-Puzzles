using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [HideInInspector] public Directions direction = new Directions();
    [HideInInspector] public PlayerState state = new PlayerState();

    private IEnumerator timer;
    [NonSerialized] public int health = 3;
    [NonSerialized] public int maxHealth = 3;

    [NonSerialized] public float bowDuration = 0.5f;
    [NonSerialized] public float bombDuration = 0.25f;
    [NonSerialized] public float swordDuration = 0.4f;

    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    public bool isPlayerActivated = true;
    public bool isInvulnerable = false;


    private void Awake()
    {
        Locator.player = this;
    }

    private void Start()
    {
        EventManager.playerLoosing += DeactivatePlayer;
        EventManager.restartingPlayer += ActivatePlayer;
        EventManager.restartingPlayer += SetHealthAfterRestart;

        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        health -= damage;
        EventManager.HitOnPlayer();

        if (health <= 0) {
            PlayerLose();
        }
    }

    private void PlayerLose()
    {
        EventManager.PlayerLose();
    }

    private void DeactivatePlayer()
    {
        boxCollider.enabled = false;
        spriteRenderer.enabled = false;

        isPlayerActivated = false;
    }

    private void ActivatePlayer()
    {
        boxCollider.enabled = true;
        spriteRenderer.enabled = true;

        isPlayerActivated = true;
    }

    private void SetHealthAfterRestart()
    {
        health = 2;
        EventManager.SetPlayerHealth();
    }

    public void AddHeart()
    {
        if (health < maxHealth)
        {
            health++;
            EventManager.SetPlayerHealth();
        }
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

    private void OnDestroy()
    {
        EventManager.playerLoosing -= DeactivatePlayer;
        EventManager.restartingPlayer -= ActivatePlayer;
        EventManager.restartingPlayer -= SetHealthAfterRestart;
    }
}
