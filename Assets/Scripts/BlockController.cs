using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{

    public float speed;
    public GameObject player;
    public GameObject teleportParticle;
    public GameObject squashParticle;
    public GameObject placeParticle;
    public AudioSource audioSource;
    public AudioClip teleportSound;
    public AudioClip placeSound;
    public AudioClip squashSound;
    public Material[] materials = new Material[2];
    private bool isOnFloor = false;
    private bool isPortal = false;
    private bool isCollidedWithPlayer = false;

    public void Init(bool isPortal)
    {
        this.isPortal = isPortal;
        gameObject.SetActive(true);
        GetComponent<MeshRenderer>().material = materials[isPortal ? 1 : 0];
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnFloor)
            return;

        Vector3 newPos = transform.position;
        newPos.y += speed * Time.deltaTime;
        int[] index = GameManager.instance.PosToIndex(transform.position);
        int x = index[0];
        int y = index[1];
        int z = index[2];
        Vector3 rightPos = GameManager.instance.IndexToPos(x, y, z);
        transform.position = newPos;

        if (!GameManager.instance.IsInBound(x, y, z))
        {
            Debug.LogError("out of bound");
        }

        if (newPos.y >= rightPos.y)
            return;

        if (!GameManager.instance.IsInBound(x, y - 1, z) || GameManager.instance.blockMap[x, y - 1, z])
        {
            isOnFloor = true;
            transform.position = rightPos;
            GameManager.instance.blockMap[x, y, z] = true;
            Instantiate(placeParticle, rightPos, Quaternion.identity).SetActive(true);
            audioSource.PlayOneShot(placeSound);

            if (isCollidedWithPlayer && IsInSamePlace())
            {
                GameObject o = Instantiate(squashParticle, rightPos, Quaternion.identity);
                o.SetActive(true);
                audioSource.PlayOneShot(squashSound);
            }

            if (isPortal)
            {
                var pos = transform.position;
                pos.y += GetComponent<BoxCollider>().size.y;
                GameObject o = Instantiate(teleportParticle, pos, Quaternion.identity);
                o.GetComponent<ParticleSystem>().loop = true;
                o.SetActive(true);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == player)
        {
            isCollidedWithPlayer = true;
            if (!IsInSamePlace() || !isPortal)
                return;

            Vector3 pos = player.transform.position;
            pos.y += 10;
            player.transform.position = pos;
            GameObject o = Instantiate(teleportParticle, pos, Quaternion.identity);
            o.SetActive(true);
            audioSource.PlayOneShot(teleportSound);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject == player)
        {
            isCollidedWithPlayer = false;
        }
    }

    private bool IsInSamePlace()
    {
        Vector3 size = GetComponent<BoxCollider>().size;
        Vector3 diff = player.transform.position - transform.position;
        return diff.x <= size.x && diff.x >= -size.x && diff.z <= size.z && diff.z >= -size.z;
    }
}
