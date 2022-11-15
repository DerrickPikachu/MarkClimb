using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{

    public float speed;
    public GameObject blockManager;
    private bool isOnFloor = false;

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
        }
    }

}
