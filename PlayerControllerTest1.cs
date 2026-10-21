using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private void Update()
    {
        Vector2 moveInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
                moveInput.y += 1;

            if (Keyboard.current.sKey.isPressed)
                moveInput.y -= 1;

            if (Keyboard.current.dKey.isPressed)
                moveInput.x += 1;

            if (Keyboard.current.aKey.isPressed)
                moveInput.x -= 1;
        }

        Vector3 movement = new Vector3(
            moveInput.x, 0f, moveInput.y
        ).normalized;

        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}