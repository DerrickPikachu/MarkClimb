using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpSkill : BaseSkill
{
    public float jumpForce = 25f;

    private bool jumpFlag;

    // Start is called before the first frame update
    void Start()
    {
        jumpFlag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key) && !GetComponent<PlayerController>().isGrounded() && jumpFlag) {
            Vector3 jumpDirection = new Vector3(0, jumpForce, 0);
            Quaternion particleRotation = Quaternion.AngleAxis(-90, Vector3.right);
            GameObject particle = ParticleManager.instance.SpawnParticle(Particle.DoubleJump, transform.position, false);
            particle.transform.rotation = particleRotation;
            GetComponent<Rigidbody>().AddForce(jumpDirection, ForceMode.Impulse);
            jumpFlag = false;
        }

        if (!jumpFlag && GetComponent<PlayerController>().isGrounded()) { jumpFlag = true; }
    }
}
