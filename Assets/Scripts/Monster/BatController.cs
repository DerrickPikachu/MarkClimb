using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : BaseMonster
{
    private enum BatStatus
    {
        Track,
        Stay,
        Attack
    }

    public float moveSpeed = 5;
    public float attackSpeed = 10;
    public float attackRange = 10;
    public float waitToAttack = 1;
    public float pushUpFactor = 0.5f;
    public float pushBackForce = 10;
    public float attackDamage = 3;
    
    private Rigidbody rb;
    private Vector3 direction;
    private Vector3 attackDirection;
    private float timeToAttack;
    private BatStatus status;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        status = BatStatus.Track;
    }

    // Update is called once per frame
    void Update()
    {
        direction = player.transform.position - transform.position;
        direction.Normalize();
        if (status == BatStatus.Stay) { timeToAttack -= Time.deltaTime; }
        UpdateStatus();
    }

    void FixedUpdate()
    {
        if (status == BatStatus.Track) {
            rb.velocity = direction * moveSpeed;
        } else if (status == BatStatus.Stay) {
            rb.velocity = Vector3.zero;
        } else if (status == BatStatus.Attack) {
            rb.velocity = attackDirection * attackSpeed;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player") {
            Vector3 monsterToPlayer = other.gameObject.transform.position - transform.position;
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            if (Vector3.Dot(monsterToPlayer, Vector3.up) > 0) {
                // Player hit the head of bat.
                Destroy(gameObject);
                playerRb.AddForce(Vector3.up * other.gameObject.GetComponent<PlayerController>().hitEnemyJumpForce, ForceMode.Impulse);
            } else {
                // Player is hit by bat.
                // Vector3 pushDirection = Vector3.forward * (Vector3.Dot(Vector3.forward, monsterToPlayer));
                // pushDirection = pushDirection.normalized + Vector3.up * pushUpFactor;
                // Debug.Log(pushDirection);
                // rb.AddForce(pushDirection.normalized * pushBackForce, ForceMode.Impulse);
                if (status == BatStatus.Attack) {
                    HealthManager hm = other.gameObject.GetComponent<HealthManager>();
                    hm.HurtByMonster(attackDamage);
                    ParticleManager.instance.SpawnParticle(Particle.Squash, other.gameObject.transform.position, false);
                }
            }
        }
    }

    void UpdateStatus()
    {
        if (status == BatStatus.Track && isInAttackRange()) {
            status = BatStatus.Stay;
            timeToAttack = waitToAttack;
        } else if (status == BatStatus.Stay && timeToAttack <= 0) {
            status = BatStatus.Attack;
            attackDirection = direction;
        } else if (!isInAttackRange()) {
            status = BatStatus.Track;
        }
    }

    bool isInAttackRange()
    {
        Vector3 different = player.transform.position - transform.position;
        float distance = different.magnitude;
        return distance < attackRange;
    }
}
