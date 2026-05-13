using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    [SerializeField] private GameObject dialogueContainer;
    public float letterWrightingDuration = 0.1f;
    //public string[] lines;
    public List<string> lines;
    private int lineIndex;


    //public void GiveLines(List<string> newLines)
    public void GiveLines(string[] newLines)
    {
        EventManager.EnterDialogue();

        lines = new List<string>();

        foreach (string newline in newLines) {
            lines.Add(newline);
        }
        StartDialogue();
    }

    private void Awake()
    {
        Locator.dialogueManager = this;
    }

    private void Start()
    {
        textComponent.text = string.Empty;
        //StartDialogue(); //
        dialogueContainer.SetActive(false);
    }

    public void StartDialogue()
    {
        dialogueContainer.SetActive(true);
        textComponent.text = string.Empty; //
        lineIndex = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[lineIndex].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(letterWrightingDuration);
        }
    }

    public void ClickContinue()
    {
        if (textComponent.text == lines[lineIndex]) {
            NextLine();
        }
        else {
            StopAllCoroutines();
            textComponent.text = lines[lineIndex];
        }
    }

    private void NextLine()
    {
        //if (lineIndex < lines.Length - 1)
        if (lineIndex < lines.Count - 1)
        {
            lineIndex++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else {
            //gameObject.SetActive(false);
            dialogueContainer.SetActive(false);

            EventManager.ExitDialogue();
        }
    }
}
