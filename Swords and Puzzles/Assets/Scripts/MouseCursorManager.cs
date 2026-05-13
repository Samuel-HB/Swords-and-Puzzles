using UnityEngine;

public class MouseCursorManager : MonoBehaviour
{
    private void Start()
    {
        print("start");
        EventManager.enteringDialogue -= ActivateMouseCursor;
        EventManager.exitingDialogue -= DeactivateMouseCursor;
        EventManager.pausingGame -= ActivateMouseCursor;
        EventManager.resumingGame -= DeactivateMouseCursor;
    }

    public void ActivateMouseCursor()
    {
        print("active");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void DeactivateMouseCursor()
    {
        print("not active");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        print("destroy");
        EventManager.enteringDialogue -= ActivateMouseCursor;
        EventManager.exitingDialogue -= DeactivateMouseCursor;
        EventManager.pausingGame -= ActivateMouseCursor;
        EventManager.resumingGame -= DeactivateMouseCursor;
    }
}
