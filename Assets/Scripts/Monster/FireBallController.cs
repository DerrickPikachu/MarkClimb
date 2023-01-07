using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : MonoBehaviour
{
    public float speed = 10f;
    public GameObject player;
    public float explosionRange = 5;
    public float damage = 3;
    public Vector3 effectOffset = new Vector3(3, 0, 0);

    private Vector3 moveDirection = -Vector3.up;
    private Rigidbody rb;
    private GameObject fireBallEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fireBallEffect = ParticleManager.instance.SpawnParticle(Particle.FireBall, transform.position, true);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = transform.position + moveDirection * speed * Time.deltaTime;
        fireBallEffect.transform.position = transform.position;
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection.normalized * speed;
    }

    void OnCollisionEnter(Collision other)
    {
        CollisionHandle(other);
    }

    void OnCollisionStay(Collision other)
    {
        CollisionHandle(other);
    }

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
    }

    void CollisionHandle(Collision other)
    {
        if (other.gameObject.name.IndexOf("CovidMonster") != -1) { return; }
        Vector3 distance = player.transform.position - transform.position;
        if (distance.magnitude <= explosionRange) {
            HealthManager hm = player.GetComponent<HealthManager>();
            hm.HurtByMonster(damage);
            ParticleManager.instance.SpawnParticle(Particle.Squash, player.transform.position, false);
        }
        ParticleManager.instance.SpawnParticle(Particle.FireBallExplosion, transform.position, false);
        Destroy(fireBallEffect);
        Destroy(gameObject);
    }
}
