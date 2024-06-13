using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour
{
    public string[] lines;
    public float speedText;
    public TextMeshProUGUI dialogueTextArea;
    private int index;
    void Start()
    {
        dialogueTextArea.text = string.Empty;
        StartDialogue();
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            dialogueTextArea.text += c;
            yield return new WaitForSeconds(speedText);
        }
    }
    public void SkipTextOnClick()
    {
        if (dialogueTextArea.text == lines[index])
        {
            NextLines();
        }
        else
        {
            StopAllCoroutines();
            dialogueTextArea.text = lines[index];
        }
    }
    private void NextLines()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueTextArea.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
