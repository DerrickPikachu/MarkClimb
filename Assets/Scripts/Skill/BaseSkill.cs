using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BaseSkill : MonoBehaviour
{

    public KeyCode key;
    public bool activate;
    public float coolDown = 0;

    protected float coolDownTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Copy(ref BaseSkill other)
    {
        other.key = key;
        other.coolDown = coolDown;
    }

    protected void CoolingDown()
    {
        coolDownTime -= Time.deltaTime;
        coolDownTime = Math.Max(coolDownTime, 0);
    }
}
