using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 8f;
    public float fallGravity = 1.5f;

    private Rigidbody rb;
    private bool grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector3(
            move * speed,
            rb.linearVelocity.y,
            0
        );

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(Vector3.up * jumpForce,
                ForceMode.Impulse);

            grounded = false;
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * fallGravity * Time.deltaTime;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = false;
    }
}