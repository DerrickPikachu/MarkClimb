
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
        {
            rb.AddForce(Vector3.up * jumpForce * 2f, ForceMode.Impulse);
            RemoveEffect(EffectType.JumpUp);
        }

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
