using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Direction
    {
        Right,
        Left
    }

    public float horizontalInput = 0;
    public float moveSpeed = 20.0f;
    public float rotateSpeed = 1;
    public float jumpForce = 1;
    public float upGravityScale = 5;
    public float downGravityScale = 10;
    public float runSpeedUpFactor = 2.0f;

    private Vector3 forwardZ = new Vector3(0, 0, 1.0f);
    private Vector3 backwardZ = new Vector3(0, 0, -1.0f);
    private Direction currentDirection = Direction.Right;
    private Quaternion rotateTarget = Quaternion.identity;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private float zeroThreshold = 0.01f;
    private float gravityScale;

    // Start is called before the first frame update
    void Start()
    {
        rotateTarget = transform.rotation;
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        gravityScale = upGravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyDown();
        UpdateGravityScale();
    }

    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
        Move();
    }

    private void HandleKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded()) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        float speed = moveSpeed * horizontalInput;
        if (Input.GetKey(KeyCode.LeftShift)) { speed *= runSpeedUpFactor; }


        if (NeedTurnAround(horizontalInput)) {
            currentDirection = (currentDirection == Direction.Right) ? Direction.Left : Direction.Right;
            rotateTarget = Quaternion.AngleAxis(
                (currentDirection == Direction.Right) ? 0 : 180,
                Vector3.up
            );
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, rotateTarget, rotateSpeed * Time.fixedDeltaTime);
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speed);
    }

    private void UpdateGravityScale()
    {
        if (rb.velocity.y > 0) {
            gravityScale = upGravityScale;
        } else if (rb.velocity.y < 0) {
            gravityScale = downGravityScale;
        }
    }

    private bool isGrounded()
    {
        return Mathf.Abs(rb.velocity.y) < zeroThreshold;
    }

    private bool NeedTurnAround(float input)
    {
        return (input > 0 && currentDirection == Direction.Left) || (input < 0 && currentDirection == Direction.Right);
    }
}
