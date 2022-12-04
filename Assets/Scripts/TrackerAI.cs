using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerAI : MonoBehaviour
{
    public GameObject player;
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
}
