using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectRingSkill : BaseSkill
{
    public GameObject ringObj;
    public float duration = 5f;

    private GameObject realRing;
    private float countingTime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!activate) {
            if (Input.GetKeyDown(key)) {
                activate = true;
                CreateRing();
                SoundManager.instance.PlaySound(SoundClip.Metal17);
                countingTime = 0f;
            }
        } else {
            countingTime += Time.deltaTime;
            if (countingTime > duration) {
                Destroy(realRing);
                activate = false;
            }
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
        Debug.Log("in copy");
    }
}
