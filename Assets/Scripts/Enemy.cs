using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    private int health;

    [SerializeField]
    private int attack;

    public Slider healthBar;

    public int Health
    {
        get => health;
        set
        {
            health = value;
            if (health < 0)
                health = 0;

            healthBar.maxValue = maxHealth;
            healthBar.value = health;
        }
    }

    public bool IsDead => health < 0;
}
