using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandInSkill : BaseSkill
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activate) {
            SoundManager.instance.PlaySound(SoundClip.Bullet21);
            Vector3 newPos = transform.position;
            newPos.y += 10;
            transform.position = newPos;
            activate = false;
        }        
    }
}
