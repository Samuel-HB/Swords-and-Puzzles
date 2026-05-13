using System.Collections;
using UnityEngine;

public class ButtonSwitchWall : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite buttonPressedSprite;

    private IEnumerator timer;
    private float secondsToWait = 0.2f;

    private float radius = 0.5f;
    private int playerLayerMask = 0;


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Start()
    {
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");

        spriteRenderer = GetComponent<SpriteRenderer>();

        CallTimer();
    }

    private void DetectPlayer()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius, playerLayerMask);

        if (hitCollider != null && hitCollider.TryGetComponent<Player>(out Player player))
        {
            if (wall != null)
            {
                Destroy(wall);
                spriteRenderer.sprite = buttonPressedSprite;
            }
        }
    }

    public void CallTimer()
    {
        timer = WaitToDetectTimer();
        StartCoroutine(timer);
    }

    IEnumerator WaitToDetectTimer()
    {
        DetectPlayer();
        yield return new WaitForSeconds(secondsToWait);
        CallTimer();
    }

    public void StopTimer()
    {
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
    }
}
