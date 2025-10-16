using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    [SerializeField] public List<Dialogue> dialogues; 
    [SerializeField] public List<string> currentDialogue;
    [SerializeField] public int currentQuest; // index for dialogues
    [SerializeField] public int currentLine;  // index for currentDialogue
    [SerializeField] public NPC nextNPC;

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
        QuestManager.Instance.activeNPC = nextNPC;
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
