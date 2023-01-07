using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerAI : BaseMonster
{
    public float moveSpeed = 5.0f;
    public bool fixedY = false;
    public float stopRange = 0.5f;
    
    private Rigidbody rb;
    private Vector3 direction;
    private float originalY;
    private float actualSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalY = transform.position.y;
        actualSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        direction = player.transform.position - transform.position;
        if (fixedY) { 
            direction = new Vector3(direction.x, 0, direction.z);
            if (Mathf.Abs(direction.z) < stopRange) {
                actualSpeed = 0.0f;
            } else {
                actualSpeed = moveSpeed;
            }
        }
        direction = direction.normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = direction * actualSpeed;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player") {
            Vector3 monsterToPlayer = other.gameObject.transform.position - transform.position;
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            if (Vector3.Dot(monsterToPlayer, Vector3.up) > 0) {
                Destroy(gameObject);
                rb.AddForce(Vector3.up * other.gameObject.GetComponent<PlayerController>().hitEnemyJumpForce, ForceMode.Impulse);
            }
        }
    }
}
