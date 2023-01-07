using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfExplosionController : BaseMonster
{
    private enum SelfExplosionStatus {
        Search,
        Attack
    }

    public float warningRange = 15f;
    public float searchMoveSpeed = 5f;
    public float attackMoveSpeed = 15f;
    public float timeToTurnAround = 3f;
    public float explosionRange = 8f;
    public float damage = 5f;
    public float underExplosionRange = -0.2f;

    private Vector3 direction;
    private SelfExplosionStatus status;
    private Rigidbody rb;
    private float turnAroundCounter = 0f;

    // Start is called before the first frame update
    void Start()
    {
        status = SelfExplosionStatus.Search;
        rb = GetComponent<Rigidbody>();
        direction = new Vector3(0, 0, 1);
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        turnAroundCounter += Time.deltaTime;
        if (turnAroundCounter >= timeToTurnAround) {
            turnAroundCounter = 0;
            TurnAround();
        }
        UpdateStatus();
    }

    void FixedUpdate()
    {
        if (status == SelfExplosionStatus.Search) {
            rb.velocity = direction * searchMoveSpeed;
        } else if (status == SelfExplosionStatus.Attack) {
            rb.velocity = direction * attackMoveSpeed;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (status == SelfExplosionStatus.Search) {
            Vector3 contactNormal = transform.TransformDirection(other.contacts[0].normal);
            Debug.Log(contactNormal);
            if (contactNormal == Vector3.forward || contactNormal == Vector3.back) {
                TurnAround();
                turnAroundCounter = 0;
            } else if (contactNormal == Vector3.up) {
                ParticleManager.instance.SpawnParticle(Particle.SelfExplosion, transform.position, false);
                Destroy(gameObject);
            }
        } else if (status == SelfExplosionStatus.Attack) {
            ParticleManager.instance.SpawnParticle(Particle.SelfExplosion, transform.position, false);
            Vector3 distance = player.transform.position - transform.position;
            Debug.Log(distance);
            if (distance.magnitude <= explosionRange && Vector3.Dot(distance, Vector3.up) >= underExplosionRange ||
                other.gameObject.name == "Player") {
                HealthManager hm = player.GetComponent<HealthManager>();
                hm.HurtByMonster(damage);
                ParticleManager.instance.SpawnParticle(Particle.Squash, player.transform.position, false);
            }
            Destroy(gameObject);
        }
    }

    void OnCollisionStay(Collision other)
    {
        // Debug.Log("in collision stay");
    }

    void UpdateStatus()
    {
        Vector3 distance = player.transform.position - transform.position;
        if (status == SelfExplosionStatus.Search && distance.magnitude <= warningRange) {
            direction = distance.normalized;
            status = SelfExplosionStatus.Attack;
        }
    }

    void TurnAround()
    {
        direction = -direction;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    float CollideDegree(Vector3 monsterToObject)
    {
        // The degree between the vector from monster to object and Vector3.forward
        float cosine = Vector3.Dot(monsterToObject.normalized, Vector3.forward);
        // return Mathf.Acos(cosine) * Mathf.Rad2Deg;
        float degree = Mathf.Acos(cosine) * Mathf.Rad2Deg;
        Debug.Log(degree);
        return degree;
    }
}
