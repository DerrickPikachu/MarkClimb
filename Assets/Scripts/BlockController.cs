using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    Normal,
    Portal,
    Spike
}
public class BlockController : MonoBehaviour
{

    public float speed;
    public GameObject player;
    public Material[] materials = new Material[3];
    private bool isOnFloor = false;
    private bool isSupportingPlayer = false;
    private PlayerController playerController;
    private BlockType blockType;
    private int height;
    private int width;
    private Vector3 size;

    public void Init(BlockType blockType, int h, int w)
    {
        this.blockType = blockType;
        height = h;
        width = w;
        gameObject.SetActive(true);
        GetComponent<MeshRenderer>().material = materials[(int)blockType];

        var scale = transform.localScale;
        scale.y *= height;
        scale.z *= width;
        transform.localScale = scale;

        var pos = transform.position;
        pos.y += GameManager.instance.yInterval / 2 * (height - 1);
        pos.z += GameManager.instance.zInterval / 2 * (width - 1);
        transform.position = pos;
    }
    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        size = Vector3.Scale(GetComponent<BoxCollider>().size, transform.localScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnFloor)
            return;

        Vector3 newPos = transform.position;
        newPos.y += speed * Time.deltaTime;
        if (isSupportingPlayer && playerController.supportBlockCount == 1)
        {
            var playerPos = player.transform.position;
            playerPos.y += speed * Time.deltaTime;
            player.transform.position = playerPos;
        }

        var offset = new Vector3(0, GameManager.instance.yInterval / 2 * (height - 1), GameManager.instance.zInterval / 2 * (width - 1));
        int[] index = GameManager.instance.PosToIndex(transform.position - offset);
        int x = index[0];
        int y = index[1];
        int z = index[2];
        Vector3 rightPos = GameManager.instance.IndexToPos(x, y, z) + offset;
        transform.position = newPos;

        if (!GameManager.instance.IsInBound(x, y, z))
        {
            Debug.LogError("out of bound");
        }

        if (newPos.y >= rightPos.y)
            return;

        bool hitFlag = false;
        for (int i = z; i < z + width; ++i)
        {
            if (!GameManager.instance.IsInBound(x, y - 1, i) || GameManager.instance.blockMap[x, y - 1, i])
            {
                hitFlag = true;
                break;
            }
        }

        if (hitFlag)
        {
            isOnFloor = true;
            transform.position = rightPos;
            for (int i = z; i < z + width; ++i)
                for (int j = y; j < y + height; ++j)
                    GameManager.instance.blockMap[x, j, i] = true;
            var spawnPos = transform.position;
            spawnPos.y -= size.y / 2;
            ParticleManager.instance.SpawnParticle(Particle.Place, spawnPos);
            SoundManager.instance.PlaySound(SoundClip.Place);
            GameManager.instance.maxHeight = Math.Max(y + height - 1, GameManager.instance.maxHeight);

            if (blockType == BlockType.Portal)
            {
                var pos = transform.position;
                pos.y += size.y / 2;
                ParticleManager.instance.SpawnParticle(Particle.Teleport, pos, true);
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
                playerController.supportBlockCount--;
            }
        }
        else
        {
            if (other.impulse.y > 0 && !isSupportingPlayer)
            {
                isSupportingPlayer = true;
                playerController.supportBlockCount++;
            }
            if (other.impulse.y <= 0 && isSupportingPlayer)
            {
                isSupportingPlayer = false;
                playerController.supportBlockCount--;
            }

            if (other.impulse.y < 0 && playerController.isGrounded())
            {
                ParticleManager.instance.SpawnParticle(Particle.Squash, player.transform.position);
                SoundManager.instance.PlaySound(SoundClip.Hurt);
                playerController.Squash();
            }

            if (isSupportingPlayer && blockType == BlockType.Portal && IsInSamePlace())
            {
                Vector3 pos = player.transform.position;
                pos.y += 10;
                player.transform.position = pos;
                ParticleManager.instance.SpawnParticle(Particle.Teleport, pos);
                SoundManager.instance.PlaySound(SoundClip.Portal);
            }

            if (blockType == BlockType.Spike)
            {
                player.GetComponent<HealthManager>().health -= 0.1f;
                ParticleManager.instance.SpawnParticle(Particle.Poke, other.contacts[0].point);
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

    private bool IsInSamePlace()
    {
        Vector3 diff = player.transform.position - transform.position;
        return Math.Abs(diff.x) <= size.x / 2 && Math.Abs(diff.z) <= size.z / 2;
    }
}
