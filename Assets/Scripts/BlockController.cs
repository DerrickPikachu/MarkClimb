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
    private bool isSupportingPlayer = false;

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

    void CollideWithPlayer(Collision other, bool isLeaving)
    {
        if (isLeaving)
        {
            if (isSupportingPlayer)
            {
                isSupportingPlayer = false;
                player.GetComponent<PlayerController>().supportBoxCount--;
            }
        }
        else
        {
            if (other.impulse.y > 0 && !isSupportingPlayer)
            {
                isSupportingPlayer = true;
                player.GetComponent<PlayerController>().supportBoxCount++;
            }
            if (other.impulse.y <= 0 && isSupportingPlayer)
            {
                isSupportingPlayer = false;
                player.GetComponent<PlayerController>().supportBoxCount--;
            }

            if (other.impulse.y < 0 && player.GetComponent<PlayerController>().isGrounded())
            {
                GameObject o = Instantiate(squashParticle, player.transform.position, Quaternion.identity);
                o.SetActive(true);
                audioSource.PlayOneShot(squashSound);
                player.GetComponent<PlayerController>().Squash();
            }

            if(isSupportingPlayer && isPortal)
            {
                Vector3 pos = player.transform.position;
                pos.y += 10;
                player.transform.position = pos;
                GameObject o = Instantiate(teleportParticle, pos, Quaternion.identity);
                o.SetActive(true);
                audioSource.PlayOneShot(teleportSound);
            }
        }
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == player)
        {
            CollideWithPlayer(other, false);
        }
    }
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject == player)
        {
            CollideWithPlayer(other, false);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject == player)
        {
            CollideWithPlayer(other, true);
        }
    }
}
