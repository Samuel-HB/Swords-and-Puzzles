using System.Collections;
using UnityEngine;

public class PlayerRemoveCollider : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private float colliderRemoveDuration = 0.1f;


    void Start()
    {
        EventManager.playerRemovingCollider += CallRemoveColliderTimer;

        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void CallRemoveColliderTimer()
    {
        StartCoroutine(RemoveColliderTimer(colliderRemoveDuration));
    }

    public IEnumerator RemoveColliderTimer(float duration)
    {
        boxCollider.enabled = false;
        yield return new WaitForSeconds(duration);
        boxCollider.enabled = true;
    }

    private void OnDestroy()
    {
        EventManager.playerRemovingCollider -= CallRemoveColliderTimer;
    }
}
