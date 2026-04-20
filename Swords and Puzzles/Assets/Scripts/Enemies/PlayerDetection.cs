using System.Collections;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] private ShootOnPlayer shootOnPlayer;

    [SerializeField] private float radius = 10f;
    private int playerLayerMask = 0;

    private IEnumerator timer;
    private float secondsToWait = 0.2f;


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.25f, 0.25f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Start()
    {
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");

        CallTimer();
    }

    private void TryToDetectPlayer()
    {
        if (Physics2D.CircleCast(transform.position, radius, Vector2.zero, 0f, playerLayerMask))
        {
            print("CircleCast detect player");
        }

        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius, playerLayerMask);
        if (hitCollider != null)
        {
            print("OverlapCircle detect player");
            shootOnPlayer.target = hitCollider.transform;
            shootOnPlayer.Shoot();
        }
    }

    public void CallTimer()
    {
        timer = WaitToDetectTimer();
        StartCoroutine(timer);
    }

    IEnumerator WaitToDetectTimer()
    {
        TryToDetectPlayer();
        yield return new WaitForSeconds(secondsToWait);
        CallTimer();
    }

    public void StopTimer()
    {
        if (timer != null) {
            StopCoroutine(timer);
            timer = null;
        }
    }
}
