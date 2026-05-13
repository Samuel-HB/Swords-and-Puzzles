//using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour, IInteractable
{
    //public List<string> lines = new List<string>();
    [TextArea(3, 10)]
    public string[] lines;

    public void Interact(Inventory inventory)
    {
        //Locator.dialogueManager.StartDialogue();
        Locator.dialogueManager.GiveLines(lines);
    }
}
