using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{

    public float speed;
    public GameObject blockManager;
    public GameObject player;
    public GameObject particle;
    public Material[] Materials = new Material[2];
    private bool isOnFloor = false;
    private bool isPortal = false;

    public void Init(bool isPortal)
    {
        this.isPortal = isPortal;
        GetComponent<MeshRenderer>().material = Materials[isPortal ? 1 : 0];
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
        int[] index = BlockManager.instance.PosToIndex(transform.position);
        int x = index[0];
        int y = index[1];
        int z = index[2];
        Vector3 rightPos = BlockManager.instance.IndexToPos(x, y, z);
        transform.position = newPos;

        if (!BlockManager.instance.IsInBound(x, y, z))
        {
            Debug.LogError("out of bound");
        }

        if (newPos.y >= rightPos.y)
            return;

        if (!BlockManager.instance.IsInBound(x, y - 1, z) || BlockManager.instance.blockMap[x, y - 1, z])
        {
            isOnFloor = true;
            transform.position = rightPos;
            BlockManager.instance.blockMap[x, y, z] = true;

            if (isPortal)
            {
                var pos = transform.position;
                pos.y += GetComponent<BoxCollider>().size.y;
                GameObject o = Instantiate(particle, pos, Quaternion.identity);
                o.GetComponent<ParticleSystem>().loop = true;
                o.SetActive(true);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!isPortal)
            return;

        if (other.gameObject == player)
        {
            Vector3 size = GetComponent<BoxCollider>().size;
            Vector3 diff = player.transform.position - transform.position;
            if (diff.y < 0 || diff.x > size.x || diff.x < -size.x || diff.z > size.z || diff.z < -size.z)
                return;

            Vector3 pos = player.transform.position;
            pos.y += 10;
            player.transform.position = pos;
            GameObject o = Instantiate(particle, pos, Quaternion.identity);
            o.SetActive(true);
        }
    }
}
