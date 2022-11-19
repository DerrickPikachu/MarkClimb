using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerAI : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 5.0f;
    
    private Rigidbody rb;
    private Vector3 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = (player.transform.position - transform.position).normalized;
        // transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        // rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        rb.velocity = direction * moveSpeed;
    }
}
