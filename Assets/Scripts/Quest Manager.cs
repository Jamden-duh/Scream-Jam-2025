using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public NPC activeNPC; // SET STARTING NPC
    public bool startQuest;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        // dialogue triggered
        if (startQuest)
        {
            while (activeNPC.currentLine != -1 && activeNPC.currentQuest != -1)
            {
                // goes through dialogue
                if (Input.GetMouseButtonDown(0))
                {
                    activeNPC.NextLine();
                }
            }
            // set new dialogue
            activeNPC.NextQuest();
            startQuest = false;
        }

        // end of quests
        if (activeNPC == null)
        {
            SceneManager.LoadScene("EndScene");
        }
    }
}
