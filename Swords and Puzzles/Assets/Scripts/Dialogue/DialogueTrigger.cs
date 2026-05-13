using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    [TextArea(3, 10)]
    public string[] lines;

    public void Interact(Inventory inventory)
    {
        Locator.dialogueManager.GiveLines(lines);
    }
}
