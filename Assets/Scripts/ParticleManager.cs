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
    Poke
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

    public void SpawnParticle(Particle p, Vector3 pos, bool loop = false)
    {
        GameObject o = Instantiate(particles[(int)p], pos, Quaternion.identity);
        o.GetComponent<ParticleSystem>().loop = loop;
        o.SetActive(true);
    }
}
