using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PowerJumpSkill : BaseSkill
{
    public float powerJumpMaxTime = 0.5f;
    public float powerJumpForce = 40f;
    
    private float powerJumpPressTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleKey();
    }

    void HandleKey()
    {
        if (Input.GetKeyDown(key) && GetComponent<PlayerController>().isGrounded()) {
            powerJumpPressTime = 0;
            activate = true;
        }
        if (activate) {
            if (Input.GetKeyUp(key) || powerJumpPressTime > powerJumpMaxTime) {
                PowerJump();
                activate = false;
            } else {
                powerJumpPressTime += Time.deltaTime;
            }
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
