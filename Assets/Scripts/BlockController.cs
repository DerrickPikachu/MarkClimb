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
    public GameObject light;
    private bool isOnFloor = false;
    private bool isSupportingPlayer = false;
    private PlayerController playerController;
    private BlockType blockType;
    private int height;
    private int width;
    private Vector3 size;
    private Vector3 originalScale;
    private readonly float maxTile = 5;

    public void Init(BlockType blockType, int h, int w)
    {
        this.blockType = blockType;
        height = h;
        width = w;
        gameObject.SetActive(true);
        GetComponent<MeshRenderer>().material = materials[(int)blockType];

        originalScale = transform.localScale;
        Scale(1, height, width);

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
            if (!GameManager.instance.IsInBound(x, y - 1, i) || GameManager.instance.blockMap[x, y - 1, i] != null)
            {
                hitFlag = true;
                break;
            }
        }

        if (hitFlag)
        {
            isOnFloor = true;
            transform.position = rightPos;
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

                pos.y += 1;
                GameObject o = Instantiate(light, pos, Quaternion.identity);
                o.SetActive(true);
            }

            var blockBelow = y > 0 ? GameManager.instance.blockMap[x, y - 1, z] : null;
            if (width == 1 && blockBelow != null && blockBelow.width == 1 && blockBelow.blockType == blockType && height + blockBelow.height <= maxTile)
            {
                var pos = blockBelow.transform.position;
                pos.y += GameManager.instance.yInterval / 2 * height;
                blockBelow.transform.position = pos;

                blockBelow.height += height;
                blockBelow.Scale(1, blockBelow.height, blockBelow.width);

                for (int j = y; j < y + height; ++j)
                    GameManager.instance.blockMap[x, j, z] = blockBelow;

                if (isSupportingPlayer)
                {
                    isSupportingPlayer = false;
                    playerController.supportBlockCount--;
                }
                Destroy(gameObject);
            }
            else
            {
                for (int i = z; i < z + width; ++i)
                    for (int j = y; j < y + height; ++j)
                        GameManager.instance.blockMap[x, j, i] = this;
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

    private void Scale(float x, float y, float z)
    {
        var scale = originalScale;
        scale.x *= x;
        scale.y *= y;
        scale.z *= z;
        transform.localScale = scale;

        var mf = GetComponent<MeshFilter>();
        var mesh = mf.mesh;
        var uvs = mesh.uv;

        x /= maxTile;
        y /= maxTile;
        z /= maxTile;
        
        // Front
        uvs[0] = new Vector2(0.0f, 0.0f);
        uvs[1] = new Vector2(x, 0.0f);
        uvs[2] = new Vector2(0.0f, y);
        uvs[3] = new Vector2(x, y);

        // Top
        uvs[8] = new Vector2(0.0f, 0.0f);
        uvs[9] = new Vector2(x, 0.0f);
        uvs[4] = new Vector2(0.0f, z);
        uvs[5] = new Vector2(x, z);

        // Back
        uvs[10] = new Vector2(x, y);
        uvs[11] = new Vector2(x, 0.0f);
        uvs[6] = new Vector2(0.0f, y);
        uvs[7] = new Vector2(0.0f, 0.0f);

        // Bottom
        uvs[12] = new Vector2(0.0f, 0.0f);
        uvs[14] = new Vector2(z, x);
        uvs[15] = new Vector2(0.0f, x);
        uvs[13] = new Vector2(z, 0.0f);

        // Left
        uvs[16] = new Vector2(0.0f, 0.0f);
        uvs[18] = new Vector2(y, z);
        uvs[19] = new Vector2(0.0f, z);
        uvs[17] = new Vector2(y, 0.0f);

        // Right (front)   
        uvs[20] = new Vector2(0.0f, 0.0f);
        uvs[22] = new Vector2(y, z);
        uvs[23] = new Vector2(0.0f, z);
        uvs[21] = new Vector2(y, 0.0f);

        mesh.uv = uvs;

    }
}
