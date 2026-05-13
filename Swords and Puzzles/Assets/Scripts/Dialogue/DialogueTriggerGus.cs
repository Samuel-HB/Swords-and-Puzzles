using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueCharacter
{
    public string characterName;
    public Sprite characterSprite;
}

[Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(4, 10)]
    public string line;
}

[Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

//public class DialogueTriggerGus : MonoBehaviour : IInteractable
//{
//    public Dialogue dialogue;

//    public void Interact(Inventory inventory)
//    {
//        Locator.dialogueManager.StartDialogue();
//    }
//}