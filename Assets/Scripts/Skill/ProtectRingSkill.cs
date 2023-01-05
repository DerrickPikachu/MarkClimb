using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectRingSkill : BaseSkill
{
    public GameObject ringObj;
    public float duration = 5f;
    public Vector3 particleOffset = new Vector3(1, 1, 0);

    private GameObject realRing;
    private float countingTime;
    private GameObject protectRingParticle;

    // Start is called before the first frame update
    void Start()
    {
        coolDownTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDownTime == 0) {
            if (!activate) {
                if (Input.GetKeyDown(key)) {
                    activate = true;
                    CreateRing();
                    countingTime = 0f;
                    protectRingParticle = ParticleManager.instance.SpawnParticle(Particle.ProtectRing, transform.position + particleOffset, true);
                }
            } else {
                protectRingParticle.transform.position = transform.position + particleOffset;
                countingTime += Time.deltaTime;
                if (countingTime > duration) {
                    Destroy(realRing);
                    Destroy(protectRingParticle);
                    activate = false;
                    coolDownTime = coolDown;
                }
            }
        } else {
            CoolingDown();
        }
    }

    void CreateRing()
    {
        realRing = Instantiate(ringObj);
        realRing.transform.position = transform.position;
        realRing.transform.parent = transform;
    }

    public override void Copy(ref BaseSkill other)
    {
        ProtectRingSkill otherRing = other as ProtectRingSkill;
        otherRing.key = key;
        otherRing.ringObj = ringObj;
    }
}
