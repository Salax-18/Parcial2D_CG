using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class DialogoManager : MonoBehaviour
{
    public static DialogoManager Instance;

    public GameObject dialogoPanel;
    public TMP_Text dialogoText;
    public TMP_Text npcNameText;

    private Queue<string> sentences = new Queue<string>();
    private bool isDialogueActive = false;

    void Awake()
    {
        Instance = this;
        dialogoPanel.SetActive(false);
    }

    public void StartDialogue(string npcName, string[] lines)
    {
        Debug.Log("StartDialogue llamado!");
        isDialogueActive = true;
        dialogoPanel.SetActive(true);

        isDialogueActive = true;
        dialogoPanel.SetActive(true);
        npcNameText.text = npcName;

        sentences.Clear();
        foreach (string line in lines)
            sentences.Enqueue(line);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        dialogoText.text = sentences.Dequeue();
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialogoPanel.SetActive(false);
    }

    void Update()
    {
        if (isDialogueActive && Keyboard.current.eKey.wasPressedThisFrame)
            DisplayNextSentence();

    }
}

