using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    SpeedUp,
    SpeedDown,
    JumpUp
}
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
    private Dictionary<EffectType, float> effects = new Dictionary<EffectType, float>();

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
        // TODO: character move should use rigidbody.MovePosition() or AddForce,
        // otherwise, use translate or directly set a position will ignore the 
        // rigidbody physic effect.
        horizontalInput = Input.GetAxis("Horizontal");
        float realSpeed = moveSpeed;
        if(HasEffect(EffectType.SpeedUp))
            realSpeed *= 3f;
        if(HasEffect(EffectType.SpeedDown))
            realSpeed *= 0.2f;
        if(HasEffect(EffectType.JumpUp))
            rb.AddForce(Vector3.up * jumpForce * 0.1f, ForceMode.Impulse);

        float moveDistance = realSpeed * Time.deltaTime * horizontalInput;
        if (Input.GetKey(KeyCode.LeftShift)) { moveDistance *= runSpeedUpFactor; }
        float newZValue = transform.position.z + moveDistance;

        if (NeedTurnAround(horizontalInput)) {
            currentDirection = (currentDirection == Direction.Right) ? Direction.Left : Direction.Right;
            rotateTarget = Quaternion.AngleAxis(
                (currentDirection == Direction.Right) ? 0 : 180,
                Vector3.up
            );
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, rotateTarget, rotateSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, newZValue);

        HandleKeyDown();
        UpdateGravityScale();
    }

    void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
    }

    private void HandleKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanJumpAgain()) {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void UpdateGravityScale()
    {
        if (rb.velocity.y > 0) {
            gravityScale = upGravityScale;
        } else if (rb.velocity.y < 0) {
            gravityScale = downGravityScale;
        }
    }

    private bool CanJumpAgain()
    {
        return Mathf.Abs(rb.velocity.y) < zeroThreshold;
    }

    private bool NeedTurnAround(float input)
    {
        return (input > 0 && currentDirection == Direction.Left) || (input < 0 && currentDirection == Direction.Right);
    }
    public void AddEffect(EffectType effectType, float sec)
    {
        float newDue = (float)Time.timeAsDouble + sec;
        if(!effects.ContainsKey(effectType) || effects[effectType] < newDue)
        {
            effects[effectType] = newDue;
        }
    }
    public bool HasEffect(EffectType effectType)
    {
        if(effects.ContainsKey(effectType) && effects[effectType] < Time.timeAsDouble)
        {
            effects.Remove(effectType);
        }

        return effects.ContainsKey(effectType);
    }
}
