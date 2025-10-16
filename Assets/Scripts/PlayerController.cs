using UnityEditor.Build.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Player player;
    Vector2 moveInput;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // move player based on input
        this.transform.position += new Vector3(moveInput.x, moveInput.y, 0);
        // update saved position
        player.X = this.gameObject.transform.position.x;
        player.Y = this.gameObject.transform.position.y;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>() * speed;
    }
}
