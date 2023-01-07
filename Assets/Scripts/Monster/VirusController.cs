using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusController : BaseMonster
{
    private enum VirusStatus {
        Move,
        Attack
    }

    public float attackRange = 10;
    public float moveSpeed = 5;
    public float attackInterval = 2;
    public GameObject fireBall;

    private float attackCoolDown;
    private VirusStatus status;
    private Vector3 direction;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        status = VirusStatus.Move;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = player.transform.position - transform.position;
        if (status == VirusStatus.Attack) {
            attackCoolDown -= Time.deltaTime;
            if (attackCoolDown <= 0) {
                Attack();
                attackCoolDown = attackInterval;
            }
        }
        UpdateStatus();
    }

    void FixedUpdate()
    {
        if (status == VirusStatus.Move) {
            rb.velocity = direction.normalized * moveSpeed;
        } else if (status == VirusStatus.Attack) {
            rb.velocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player") {
            Vector3 monsterToPlayer = other.gameObject.transform.position - transform.position;
            Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
            if (Vector3.Dot(monsterToPlayer, Vector3.up) > 0) {
                Destroy(gameObject);
                playerRb.AddForce(Vector3.up * other.gameObject.GetComponent<PlayerController>().hitEnemyJumpForce, ForceMode.Impulse);
            }
        } else if (other.gameObject.name.IndexOf("Block") != -1) {
            Vector3 monsterToBlock = other.gameObject.transform.position - transform.position;
            if (Vector3.Dot(monsterToBlock, Vector3.up) > 0) { Explosion(); }
        }
    }

    void UpdateStatus()
    {
        Vector3 distance = player.transform.position - transform.position;
        if (status == VirusStatus.Move && distance.magnitude <= attackRange) {
            status = VirusStatus.Attack;
            attackCoolDown = attackInterval; 
        } else if (status == VirusStatus.Attack && distance.magnitude > attackRange) {
            status = VirusStatus.Move;
        }
    }

    void Attack()
    {
        GameObject attackObj = Instantiate(fireBall);
        attackObj.transform.position = transform.position;
        attackObj.GetComponent<FireBallController>().SetDirection(direction);
        attackObj.GetComponent<FireBallController>().player = player;
    }

    void Explosion()
    {
        GameObject[] attackObj = new GameObject[3];
        Vector3[] offsets = {
            Vector3.zero,
            new Vector3(0, -1, 1),
            new Vector3(0, -1, -1)
        };
        Vector3[] directions = {
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 1),
            new Vector3(0, -1, -1),
        };
        for (int i = 0; i < attackObj.Length; i++) {
            attackObj[i] = Instantiate(fireBall);
            attackObj[i].transform.position = transform.position + offsets[i];
            attackObj[i].GetComponent<FireBallController>().SetDirection(directions[i]);
            attackObj[i].GetComponent<FireBallController>().player = player;
        }
        Destroy(gameObject);
    }
}
