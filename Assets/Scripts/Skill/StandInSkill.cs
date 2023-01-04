using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandInSkill : BaseSkill
{
    // Start is called before the first frame update
    void Start()
    {
        coolDownTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (activate) {
            Vector3 newPos = transform.position;
            newPos.y += 10;
            transform.position = newPos;
            activate = false;
            ParticleManager.instance.SpawnParticle(Particle.StandIn, transform.position, false);
            coolDownTime = coolDown;
        }
        if (coolDownTime > 0) { CoolingDown(); }
    }

    public bool SetActivate()
    {
        if (coolDownTime == 0) { activate = true; }
        return activate;
    }
}
