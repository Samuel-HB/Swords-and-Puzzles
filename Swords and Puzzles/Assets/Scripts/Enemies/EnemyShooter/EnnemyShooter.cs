using System.Collections;
using UnityEngine;

public class EnnemyShooter : ShootOnPlayer
{

    private float durationBeforeFirstDetection = 0.2f;
    private float secondsToWait = 0.2f;
    private IEnumerator timer;

    private void Start()
    {
        SetPlayerLayerMask();
        InstantiateBullets();

        CallStartTimer();
    }

    public void CallStartTimer()
    {
        StartCoroutine(WaitBeforeFirstDetect());
    }

    IEnumerator WaitBeforeFirstDetect() // avoid enemy instantly attacking player at the start of the level
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
        if (TryToDetectPlayer()) {
            Shoot();
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
