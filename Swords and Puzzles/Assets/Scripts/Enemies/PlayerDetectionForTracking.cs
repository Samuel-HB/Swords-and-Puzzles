using System.Collections;
using UnityEngine;

public class PlayerDetectionForTracking : MonoBehaviour
{
    [SerializeField] private PathFinder pathFinder;

    [SerializeField] private float radius = 10f;
    private int playerLayerMask = 0;

    [SerializeField] private float durationBeforeFirstDetection = 0.2f; 
    private float secondsToWait = 0.4f;
    private IEnumerator timer;


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.25f, 0.25f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Start()
    {
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");

        CallStartTimer();
        //StartCoroutine(WaitToFindPathToPlayer());
    }





    // to test
    //IEnumerator WaitToFindPathToPlayer() // trying to optimize by doing the maths only 2 times a second
    //{
    //    yield return new WaitForSeconds(1f);
    //    pathFinder.StartFindPath();
    //}









    private void TryToDetectPlayer()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(transform.position, radius, playerLayerMask);
        if (hitCollider != null)
        {
            if (TryToDetectPlayerByRaycast(hitCollider.transform.position)) {
                print("gus !!!");
                pathFinder.StartFindPath();
            }
        }
    }

    private bool TryToDetectPlayerByRaycast(Vector3 targetPosition)
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, targetPosition - transform.position);
        if (ray.collider != null)
        {
            if (ray.collider.TryGetComponent<Player>(out Player player)) {
                //Debug.DrawRay(transform.position, targetPosition - transform.position, Color.black);
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return false;
        }
    }

    public void CallStartTimer()
    {
        StartCoroutine(WaitBeforeFirstDetect());
    }

    IEnumerator WaitBeforeFirstDetect() // avoid ennemy instantly attacking player at the start of the level
    {
        yield return new WaitForSeconds(durationBeforeFirstDetection);
        CallTimer();
    }

    public void CallTimer()
    {
        timer = WaitToDetectTimer();
        StartCoroutine(timer);
    }

    IEnumerator WaitToDetectTimer() // trying to optimize by doing the maths only 5 times a second
    {
        TryToDetectPlayer();
        yield return new WaitForSeconds(secondsToWait);
        CallTimer();
    }

    //public void StopTimer()
    //{
    //    if (timer != null) {
    //        StopCoroutine(timer);
    //        timer = null;
    //    }
    //}
}
