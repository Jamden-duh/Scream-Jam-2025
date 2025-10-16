using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    // map position
    public float X;
    public float Y;

    private int awareness;
    public static int Awareness
    {
        get
        {
            if (Instance != null)
                return Instance.awareness;
            return -1;
        }
        set
        {
            Instance.awareness = value;
        }
    }

    public static bool LowAwareness => Awareness < GameManager.MediumAwarenessThreshold;
    public static bool MedAwareness => Awareness >= GameManager.MediumAwarenessThreshold && Awareness < GameManager.HighAwarenessThreshold;
    public static bool HighAwareness => Awareness >= GameManager.HighAwarenessThreshold;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
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

    }
}
