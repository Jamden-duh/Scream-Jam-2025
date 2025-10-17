using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public NPC activeNPC; // SET STARTING NPC
    public bool startQuest;

    [SerializeField] public GameObject dialoguePrefab;
    [SerializeField] public GameObject npcPrefab;
    GameObject npcGO;
    GameObject dialogueGO;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            // If another instance already exists, destroy this one.
            Destroy(gameObject);
        }
        else
        {
            // Otherwise, set this as the instance.
            Instance = this;
            // Prevents from being destroyed on scene load
            DontDestroyOnLoad(gameObject);
        }

        activeNPC.active = true;
    }

    // Update is called once per frame
    void Update()
    {
        // dialogue triggered
        if (startQuest)
        {
            // only activates once
            startQuest = false;

            // instantiates dialogue popups
            npcGO = Instantiate(npcPrefab);
            npcGO.GetComponent<Image>().sprite = activeNPC.npcSprite;
            dialogueGO = Instantiate(dialoguePrefab);
            dialogueGO.GetComponent<Text>().text = activeNPC.currentDialogue[activeNPC.currentLine];

            while (activeNPC.currentLine != -1 && activeNPC.currentQuest != -1)
            {
                // goes through dialogue
                if (Input.GetMouseButtonDown(0))
                {
                    activeNPC.NextLine();
                    dialogueGO.GetComponent<Text>().text = activeNPC.currentDialogue[activeNPC.currentLine];
                }
            }

            Destroy(npcGO);
            Destroy(dialogueGO);

            // set new dialogue
            activeNPC.NextQuest();
        }

        // end of quests
        if (activeNPC == null)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}
