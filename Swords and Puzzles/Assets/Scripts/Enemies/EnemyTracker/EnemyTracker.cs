using System.Collections;
using UnityEngine;

public class EnemyTracker : TrackOnPlayer
{
    private float durationBeforeFirstDetection = 0.2f;
    private float secondsToWait = 0.2f;
    private IEnumerator timer;

    private void Start()
    {
        state = EnemyState.Patrol;
        swordCollider.enabled = false;

        SetPlayerLayerMask();

        CallStartTimer();
        CallCheckDirectionTimer();
    }

    private void Update()
    {
        switch (state)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Tracking:
                TrackPlayer();
                break;
            case EnemyState.Attack:
                AttackPlayer();
                break;
            case EnemyState.GoesBack:
                GoesBackToInitialPosition();
                break;
            default:
                break;
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

    IEnumerator WaitToDetectTimer()
    {
        switch (state)
        {
            case EnemyState.Patrol:
                TryTrackPlayer();
                break;
            default:
                break;
        }
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
