using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 8f;
    public float fallGravity = 1.5f;

    [Header("Sliding")]
    public float slideSpeed = 12f;
    public float slideDuration = 0.7f;

    private Rigidbody rb;
    private bool grounded;
    private bool isSliding;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftShift) && grounded && !isSliding)
        {
            StartCoroutine(Slide(move));
        }

        if (!isSliding)
        {
            rb.linearVelocity = new Vector3(
                move * speed,
                rb.linearVelocity.y,
                0
            );
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded && !isSliding)
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

    IEnumerator Slide(float move)
    {
        isSliding = true;

        if (move == 0)
            move = transform.localScale.x >= 0 ? 1 : -1;

        rb.linearVelocity = new Vector3(
            move * slideSpeed,
            rb.linearVelocity.y,
            0
        );

        yield return new WaitForSeconds(slideDuration);

        isSliding = false;
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
