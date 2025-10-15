using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    [SerializeField] List<Dialogue> dialogues; 
    [SerializeField] List<string> currentDialogue;
    [SerializeField] public int currentQuest; // index for dialogues
    [SerializeField] public int currentLine;  // index for currentDialogue

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentDialogue = dialogues[currentQuest].currentDialogue;
    }

    public void NextQuest()
    {
        // sets next NPC to talk to
        QuestManager.Instance.activeNPC = dialogues[currentQuest].nextQuest;
        // changes dialogue for current NPC (if any)
        if (currentQuest < dialogues.Count)
            currentQuest++;
        else
            currentQuest = -1;
    }
    public void NextLine()
    {
        if (currentLine < currentDialogue.Count)
            currentLine++;
        else
            currentLine = -1;
    }
}
