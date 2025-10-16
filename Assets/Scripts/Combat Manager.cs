using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    private Player Player => Player.Instance;

    [SerializeField]
    private Transform playerCombatObject;
    [SerializeField]
    private Transform enemyCombatObject;

    [SerializeField]
    private Enemy enemy;
    [SerializeField]
    private Slider enemyHealthBar;

    [SerializeField]
    private GameObject hitIndicatorPrefab;
    private List<Transform> hitTimingIndicators;
    private List<float> hitTimingOffsets;

    [SerializeField]
    private Transform hitTimer;

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
    [SerializeField]
    private float[] damageMultipliers;

    [SerializeField]
    private Color[] hitColors;

    private float damageSum;

    private bool playerTurn;

    private bool disableInput;

    [SerializeField]
    private GameObject inputWindow;

    private void Start()
    {
        StartBattle(enemy);
    }

    private void Update()
    {
        if (playerTurn)
        {
            timerTime += Time.deltaTime * indicatorSpeed;

            for (int i = 0; i < hitTimingIndicators.Count; i++)
            {
                hitTimingIndicators[i].position = hitTimer.position + hitTimerWidth * Mathf.Sin(timerTime + hitTimingOffsets[i]) * Vector3.right;
            }

            if (!disableInput && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                // janky solution but it should work
                if (hitTimingIndicators.Count == 6)
                {
                    foreach (Transform indicator in hitTimingIndicators)
                    {
                        int hitStrength = EvaluateHit(indicator);

                        damageSum += damageMultipliers[hitStrength];
                    }

                    hitTimingIndicators.Clear();
                    hitTimingOffsets.Clear();

                    if (hitTimingIndicators.Count == 0)
                    {
                        playerTurn = false;
                        StartCoroutine(EndPlayerTurn());
                    }
                }
                else if (hitTimingIndicators.Count != 0)
                {
                    int hitStrength = EvaluateHit(hitTimingIndicators[0]);

                    hitTimingIndicators.RemoveAt(0);
                    hitTimingOffsets.RemoveAt(0);

                    if (hitTimingIndicators.Count == 0)
                    {
                        playerTurn = false;
                        StartCoroutine(EndPlayerTurn());
                    }

                    damageSum += hitStrength;
                }
            }
        }
    }

    private int EvaluateHit(Transform indicator)
    {
        if (IsInHitRange(indicator.position.x, strongCrit))
        {
            StartCoroutine(StopIndicator(indicator, 3));
            return 3;
        }
        else if (IsInHitRange(indicator.position.x, weakCrit))
        {
            StartCoroutine(StopIndicator(indicator, 2));
            return 2;
        }
        else if (IsInHitRange(indicator.position.x, normalHit))
        {
            StartCoroutine(StopIndicator(indicator, 1));
            return 1;
        }
        else
        {
            StartCoroutine(StopIndicator(indicator, 0));
            return 0;
        }
    }

    public void StartBattle(Enemy enemy)
    {
        hitTimingOffsets = new();
        if (hitTimingIndicators != null)
            RemoveHitIndicators();
        else
            hitTimingIndicators = new();

        enemy.healthBar = enemyHealthBar;
        enemy.Health = enemy.maxHealth; // this sets the health bar
        enemy.UpdateSprite();
        StartTurn();
    }

    public void StartTurn()
    {
        inputWindow.SetActive(true);
        hitTimer.gameObject.SetActive(false);
        playerTurn = true;
        damageSum = 0;
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

    private IEnumerator StopIndicator(Transform indicator, int hitType)
    {
        Image sprite = indicator.GetComponent<Image>();
        sprite.color = hitColors[hitType];
        
        yield return new WaitForSeconds(0.2f);

        Destroy(indicator.gameObject);
    }

    private IEnumerator EndPlayerTurn()
    {
        yield return new WaitForSeconds(0.2f);

        hitTimer.gameObject.SetActive(false);
        enemy.Health -= (int)damageSum;

        if (enemy.IsDead)
        {
            WinCombat();
            yield break;
        }

        Debug.Log("enemy turn");
        Vector3 enemyPosition = enemyCombatObject.position;
        Vector3 playerPosition = playerCombatObject.position;

        // move towards player
        int towardsFrames = 30;
        for (int i = 0; i < towardsFrames; i++)
        {
            yield return new WaitForSeconds(1 / 60f);
            enemyCombatObject.position = Vector3.LerpUnclamped(enemyPosition, enemyPosition + (playerPosition - 
                enemyPosition) / 2, EaseSine(i / (towardsFrames - 1f)));
        }

        // attack and move back
        int awayFrames = 70;
        for (int i = 0; i < awayFrames; i++)
        {
            yield return new WaitForSeconds(1 / 60f);
            enemyCombatObject.position = Vector3.LerpUnclamped(enemyPosition + (playerPosition - enemyPosition) / 2, 
                enemyPosition, EaseInOutBack(i / (awayFrames - 1f)));
        }
        enemyCombatObject.position = enemyPosition;

        yield return new WaitForSeconds(1f);
        StartTurn();
    }

    /// <summary>
    /// https://easings.net/#easeInSine
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private float EaseSine(float t)
    {
        return 1 - Mathf.Cos((t * Mathf.PI) / 2);
    }

    /// <summary>
    /// https://easings.net/#easeOutBack
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    private float EaseInOutBack(float t)
    {
        float c1 = 1.70158f;
        float c2 = c1 * 1.525f;

        return t < 0.5
          ? (Mathf.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2
          : (Mathf.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2;
    }

    private void WinCombat()
    {
        Debug.Log("Combat end");
    }

    private void LoseCombat()
    {
        Debug.Log("Combat loss");
        SceneManager.LoadScene("EndScene");
    }

    /// <summary>
    /// Called by all attack functions
    /// </summary>
    private void DoAttack()
    {
        timerTime = 0;
        RemoveHitIndicators();
        damageSum = 0;
        hitTimer.gameObject.SetActive(true);
        inputWindow.SetActive(false);
    }

    public void BasicAttack()
    {
        DoAttack();
        // 1x100%
        hitTimingIndicators.Add(SpawnIndicator(0));
    }

    public void TripleAttack()
    {
        DoAttack();
        // 3x40% : 120% max
        hitTimingIndicators.Add(SpawnIndicator(0));
        hitTimingIndicators.Add(SpawnIndicator(-0.5f));
        hitTimingIndicators.Add(SpawnIndicator(-1f));
    }

    public void SpreadAttack()
    {
        DoAttack();
        // 6x30% : 180% max
        for (int i = 0; i < 6; i++)
        {
            hitTimingIndicators.Add(SpawnIndicator(Random.Range(0, 20f)));
        }
    }

    private void RemoveHitIndicators()
    {
        for (int i = hitTimingIndicators.Count - 1; i >= 0; i--)
        {
            Destroy(hitTimingIndicators[i].gameObject);
        }
        hitTimingIndicators.Clear();
    }

    private Transform SpawnIndicator(float offset)
    {
        hitTimingOffsets.Add(offset);
        GameObject indicator = Instantiate(hitIndicatorPrefab, hitTimer.position - (1 + offset) * hitTimerWidth * Vector3.right, Quaternion.identity, hitTimer);
        return indicator.transform;
    }
}
