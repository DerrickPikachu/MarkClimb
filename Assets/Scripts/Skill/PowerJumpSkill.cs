using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PowerJumpSkill : BaseSkill
{
    public float powerJumpMaxTime = 0.5f;
    public float powerJumpForce = 40f;
    public Vector3 collectPowerEffectOffset = new Vector3(1, 1, 0);
    
    private float powerJumpPressTime;
    private GameObject CollectPowerParticle;

    // Start is called before the first frame update
    void Start()
    {
        coolDownTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandleKey();
    }

    void HandleKey()
    {
        if (coolDownTime == 0) {
            if (Input.GetKeyDown(key) && GetComponent<PlayerController>().isGrounded()) {
                powerJumpPressTime = 0;
                activate = true;
                CollectPowerParticle = ParticleManager.instance.SpawnParticle(
                    Particle.CollectPower, transform.position + collectPowerEffectOffset, true);
            }
            if (activate) {
                CollectPowerParticle.transform.position = transform.position + collectPowerEffectOffset;
                if (Input.GetKeyUp(key) || powerJumpPressTime > powerJumpMaxTime) {
                    PowerJump();
                    activate = false;
                    SoundManager.instance.PlaySound(SoundClip.BloodyPunch);
                    Destroy(CollectPowerParticle);
                    ParticleManager.instance.SpawnParticle(Particle.PowerJump, transform.position, false);
                    coolDownTime = coolDown;
                } else {
                    powerJumpPressTime += Time.deltaTime;
                }
            }
        } else {
            CoolingDown();
        }
    }

    void PowerJump()
    {
        powerJumpPressTime = Math.Min(powerJumpPressTime, powerJumpMaxTime);
        float forceFactor = powerJumpPressTime / powerJumpMaxTime;
        Vector3 jumpVector = new Vector3(0, powerJumpForce * forceFactor, 0);
        GetComponent<Rigidbody>().AddForce(jumpVector, ForceMode.Impulse);
    }
}
