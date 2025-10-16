using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    private int health;

    [SerializeField]
    private int attack;

    public Slider healthBar;

    [SerializeField]
    private Sprite highAwarenessSprite;
    [SerializeField]
    private Sprite medAwarenessSprite;
    [SerializeField]
    private Sprite lowAwarenessSprite;

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

    public void UpdateSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // if low awarenessm, etc.
            if (Player.LowAwareness)
            {
                spriteRenderer.sprite = lowAwarenessSprite;
            }
            else if (Player.MedAwareness)
            {
                spriteRenderer.sprite = medAwarenessSprite;
            }
            else if (Player.HighAwareness)
            {
                spriteRenderer.sprite = highAwarenessSprite;
            }
        }
    }
}
