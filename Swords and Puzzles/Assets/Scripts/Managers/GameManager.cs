using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Player player;
    private Vector2 playerSavedPosition = new Vector2();


    private void Start()
    {
        EventManager.playerLoosing += CallRestartPlayerTimer;
        EventManager.savingPlayerPosition += GetPlayerSavedPosition;

        player = Locator.player;
        GetPlayerSavedPosition();
    }

    private void CallRestartPlayerTimer()
    {
        StartCoroutine(RestartPlayerTimer(0.45f));
    }

    IEnumerator RestartPlayerTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        EventManager.RestartPlayer();
        RestartPlayer();
    }

    private void GetPlayerSavedPosition()
    {
        playerSavedPosition = player.transform.position;
    }

    private void RestartPlayer()
    {
        player.transform.position = playerSavedPosition;
    }

    private void OnDestroy()
    {
        EventManager.playerLoosing -= CallRestartPlayerTimer;
        EventManager.savingPlayerPosition -= GetPlayerSavedPosition;
    }
}
