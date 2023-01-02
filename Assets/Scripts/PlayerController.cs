
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum EffectType
{
    SpeedUp,
    SpeedDown,
    JumpUp,
    DoubleJump
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
    public float hitEnemyJumpForce = 25;
    public float upGravityScale = 5;
    public float downGravityScale = 10;
    public float runSpeedUpFactor = 2.0f;
    public float maxJumpButtonTime = 0.3f;
    public GameObject doubleJumpEffect;

    private Vector3 forwardZ = new Vector3(0, 0, 1.0f);
    private Vector3 backwardZ = new Vector3(0, 0, -1.0f);
    private Direction currentDirection = Direction.Right;
    private Quaternion rotateTarget = Quaternion.identity;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private float zeroThreshold = 0.01f;
    private float gravityScale;
    private Animator anim = null;
    private bool jumping = false;
    private bool doubleJump = false;
    private int jumpChance = 1;
    private float jumpTime = 0;
    private float jumpDownForce = 10f;
    private Dictionary<EffectType, float> effects = new Dictionary<EffectType, float>();
    private Vector3 allJumpForce;
    private Vector3 maxJumpForce = new Vector3(0, 30, 0);
    private float squashTime;
    private readonly float maxSquashTime = 3;
    private float scaleY;
    public int supportBlockCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        rotateTarget = transform.rotation;
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        gravityScale = upGravityScale;
        anim = GetComponentInChildren<Animator>();
        scaleY = transform.localScale.y;
        if (anim == null) {
            Debug.LogError("Animator is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleKey();
        UpdateJumpTime();
        UpdateGravityScale();
        ForceXAxis();
        UnSquash();
    }

    void FixedUpdate()
    {
        Jump();
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
        Move();
    }

    void OnCollisionEnter(Collision other)
    {
        // TODO: fix hard coded
        if (other.gameObject.name.IndexOf("Monster") != -1)
        {
            Ray positionRay = new Ray(transform.position, other.transform.position - transform.position);
            RaycastHit rayHit;
            Physics.Raycast(positionRay, out rayHit);
            Vector3 rayHitNormal = rayHit.normal;
            rayHitNormal = rayHit.transform.TransformDirection(rayHitNormal);

            if (rayHitNormal.y > 0.0f) {
                Destroy(other.gameObject);
                rb.AddForce(Vector3.up * hitEnemyJumpForce, ForceMode.Impulse);
            } else {
                // Destroy(gameObject);
            }
        }
    }

    private void HandleKey()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (isGrounded()) {
                jumpChance = (HasEffect(EffectType.DoubleJump) ? 2 : 1);
                jumping = true;
                jumpTime = 0;
            // } else if (HasEffect(EffectType.DoubleJump)) {
            } else if (jumpChance > 0) {
                doubleJump = true;
                jumpTime = 0;
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow)) {
            jumping = false;
            doubleJump = false;
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            // Debug.Log("flash");
            Vector3 moveDirection = Vector3.zero;
            if (Input.GetKey(KeyCode.RightArrow)) { moveDirection += new Vector3(0, 0, 1); }
            if (Input.GetKey(KeyCode.LeftArrow)) { moveDirection += new Vector3(0, 0, -1); }
            if (Input.GetKey(KeyCode.UpArrow)) { moveDirection += new Vector3(0, 1, 0); }
            if (Input.GetKey(KeyCode.DownArrow)) { moveDirection += new Vector3(0, -1, 0); }

            moveDirection.Normalize();
            if (moveDirection != Vector3.zero) {
                Vector3 target = DecideFlashDestination(moveDirection);
                transform.position = target;
            }
        }
    }

    private Vector3 DecideFlashDestination(Vector3 moveDirection)
    {
        float moveDistance = 10;
        float blockLength = 3;
        float bonusFactor = 0.7f;
        Vector3 right = new Vector3(0, 0, 1);
        Vector3 left = new Vector3(0, 0, -1);
        Vector3 targetPos = transform.position + moveDirection * moveDistance;
        Vector3 finalPos = transform.position;
        Vector3 topPos = targetPos + new Vector3(0, blockLength * bonusFactor, 0);
        Vector3 rightPos = targetPos + new Vector3(0, 0, blockLength * bonusFactor);
        Vector3 leftPos = targetPos + new Vector3(0, 0, -blockLength * bonusFactor);

        if (!isPositionHasBlock(targetPos)) {
            finalPos = targetPos;
        } else if (!isPositionHasBlock(topPos)) {
            finalPos = topPos;
            Ray downRay = new Ray(finalPos, Vector3.down);
            RaycastHit downRayHit;
            if (Physics.Raycast(downRay, out downRayHit, moveDistance)) {
                if (downRayHit.distance > 0.1f) {
                    finalPos += Vector3.down * (downRayHit.distance - 0.1f);
                }
            }
        } else if (!isPositionHasBlock(rightPos) && Vector3.Dot(moveDirection.normalized, right) > 0) {
            finalPos = rightPos;
        } else if (!isPositionHasBlock(leftPos) && Vector3.Dot(moveDirection.normalized, left) > 0) {
            finalPos = leftPos;
        } else {
            // The raycast from player and move to the flash
            Ray directRay = new Ray(transform.position, moveDirection);
            RaycastHit rayHit;
            if (Physics.Raycast(directRay, out rayHit, moveDistance)) {
                finalPos = transform.position + moveDirection * (rayHit.distance - blockLength / 2);
            }
        }

        return finalPos;
    }

    private bool isPositionHasBlock(Vector3 position)
    {
        Vector3 rayStartPos = position + new Vector3(10, 0, 0);
        Ray cubeRay = new Ray(rayStartPos, position - rayStartPos);
        RaycastHit rayHit;
        if (Physics.Raycast(cubeRay, out rayHit)) {
            if (rayHit.collider.gameObject.name.IndexOf("Cube") == -1) {
                return false;
            }
        }
        return true;
    }

    private void Jump()
    {
        if (jumpChance > 0) {
            if (jumping && isGrounded()) {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpChance--;
            }
            if (doubleJump) {
                Instantiate(doubleJumpEffect, transform.position, Quaternion.identity);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                doubleJump = false;
                jumpChance--;
            }
        }
        if (!jumping && jumpTime < maxJumpButtonTime) {
            rb.AddForce(-Vector3.up * jumpDownForce, ForceMode.Impulse);
            jumpTime = maxJumpButtonTime;
        }
    }

    private void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        float speed = moveSpeed * horizontalInput;
        if (Input.GetKey(KeyCode.LeftShift)) { speed *= runSpeedUpFactor; }
        
        if(HasEffect(EffectType.SpeedUp))
            speed *= 3f;
        if(HasEffect(EffectType.SpeedDown))
            speed *= 0.2f;
        if(HasEffect(EffectType.JumpUp))
            rb.AddForce(Vector3.up * jumpForce * 1f, ForceMode.Impulse);

        if (anim != null) {
            anim.SetFloat("Speed", Mathf.Abs(speed));
        }

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

    public bool isGrounded()
    {
        return supportBlockCount > 0 || transform.position.y < 0.2f;
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
    public void RemoveEffect(EffectType effectType)
    {
        effects.Remove(effectType);
    }

    private void ForceXAxis()
    {
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
    }
    public void Squash()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        squashTime = maxSquashTime;
        HealthManager hm = GetComponent<HealthManager>();
        hm.HurtByBlock();
    }
    private void UnSquash()
    {
        if(squashTime <= 0)
            return;
        squashTime -= Time.deltaTime;
        float size = (float)Math.Max(0, 1 - (maxSquashTime - squashTime) / 0.5);

        var scale = transform.localScale;
        transform.localScale = new Vector3(scale.x, scaleY * size, scale.z);
        
        if(squashTime <= 0)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            transform.localScale = new Vector3(scale.x, scaleY, scale.z);
            Vector3 pos = transform.position;
            pos.y += 10;
            transform.position = pos;
            SoundManager.instance.PlaySound(SoundClip.Portal);
            ParticleManager.instance.SpawnParticle(Particle.Teleport, pos);
        }
    }

    private void UpdateJumpTime()
    {
        if (jumping) {
            jumpTime += Time.deltaTime;
        }
    }
}
