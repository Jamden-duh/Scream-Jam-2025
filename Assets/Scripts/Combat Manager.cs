using UnityEngine;
using UnityEngine.InputSystem;

public class CombatManager : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Enemy enemy;

    [SerializeField]
    private Transform hitTimingIndicator;
    [SerializeField]
    private Transform hitTimer;
    private Vector2 hitTimerPosition;

    [SerializeField]
    private float indicatorSpeed;
    [SerializeField]
    private float hitTimerWidth;
    private float timerTime; // not using application time incase we add pausing

    [SerializeField]
    private RectTransform strongCrit;
    [SerializeField]
    private RectTransform weakCrit;
    [SerializeField]
    private RectTransform normalHit;
    [SerializeField]
    private RectTransform weakHit;

    public bool playerTurn;

    private void Start()
    {
        hitTimerPosition = hitTimer.position;
        StartBattle();
    }

    private void Update()
    {
        timerTime += Time.deltaTime;

        hitTimingIndicator.position = 
            hitTimerPosition - hitTimerWidth
            * Mathf.Sin(timerTime * indicatorSpeed)
            * Vector2.right;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (IsInHitRange(hitTimingIndicator.position.x, strongCrit))
            {
                Debug.Log("Strong crit");
            }
            else if (IsInHitRange(hitTimingIndicator.position.x, weakCrit))
            {
                Debug.Log("Weak crit");
            }
            else if (IsInHitRange(hitTimingIndicator.position.x, normalHit))
            {
                Debug.Log("Normal hit");
            }
            else if (IsInHitRange(hitTimingIndicator.position.x, weakHit))
            {
                Debug.Log("Weak hit");
            }
            else
            {
                Debug.Log("No collision");
            }
        }
    }

    public void StartBattle()
    {
        
    }

    private bool IsInHitRange(float pos, RectTransform range)
    {
        if (pos > range.position.x - range.sizeDelta.x &&
            pos < range.position.x + range.sizeDelta.x)
        {
            return true;
        }
        return false;
    }
}
