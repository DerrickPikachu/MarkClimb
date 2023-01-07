using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Particle
{
    Squash,
    Place,
    Teleport,
    GetItem,
    Poke,
    Flash,
    DoubleJump,
    CollectPower,
    PowerJump,
    ProtectRing,
    StandIn,
    FireBallExplosion,
    FireBall,
    SelfExplosion
}
public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;
    [SerializeField] private GameObject[] particles = new GameObject[3];

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
            return;
        }
    }

    public GameObject SpawnParticle(Particle p, Vector3 pos, bool loop = false)
    {
        GameObject o = Instantiate(particles[(int)p], pos, Quaternion.identity);
        o.GetComponent<ParticleSystem>().loop = loop;
        o.SetActive(true);
        return o;
    }
}
