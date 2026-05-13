using System.Collections;
using UnityEngine;

public class ButtonSwitchWall : MonoBehaviour
{
    [SerializeField] private GameObject wall;

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

        CallTimer();
    }

    private void DetectPlayer()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius, playerLayerMask);

        if (hitCollider != null && hitCollider.TryGetComponent<Player>(out Player player))
        {
            if (wall != null) {
                Destroy(wall);
            }
        }
    }

    public void CallTimer()
    {
        timer = WaitToDetectTimer();
        StartCoroutine(timer);
    }

    IEnumerator WaitToDetectTimer() // trying to optimize by doing the maths only 5 times a second
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
