using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{

    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public float zMin;
    public float zMax;
    public float timeInterval;
    public float xInterval;
    public float yInterval;
    public float zInterval;
    public GameObject block;
    public bool[,,] blockMap;
    public static BlockManager instance { get; private set; } = null;
    private float passingTime = 0;
    private int xCount, yCount, zCount;
    private System.Random random;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
            return;
        }

        int[] index = PosToIndex(new Vector3(xMax, yMax, zMax));
        xCount = index[0] + 1;
        yCount = index[1] + 1;
        zCount = index[2] + 1;
        blockMap = new bool[xCount, yCount, zCount];
        random = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        passingTime += Time.deltaTime;
        if (passingTime > timeInterval)
        {
            passingTime = 0;
            SpawnBlock();
        }
    }

    void SpawnBlock()
    {
        int x = random.Next(0, xCount - 1);
        int z = random.Next(0, zCount - 1);
        GameObject o = Instantiate(block, IndexToPos(x, yCount - 1, z), Quaternion.identity);
        o.SetActive(true);
        o.GetComponent<BlockController>().Init(random.NextDouble() > 0.8);
    }

    public int[] PosToIndex(Vector3 v)
    {
        int x = (int)((v.x - xMin) / xInterval);
        int y = (int)((v.y - yMin) / yInterval);
        int z = (int)((v.z - zMin) / zInterval);
        return new int[] { x, y, z };
    }

    public Vector3 IndexToPos(int x, int y, int z)
    {
        return new Vector3(xMin + x * xInterval, yMin + y * yInterval, zMin + z * zInterval);
    }

    public bool IsInBound(int x, int y, int z)
    {
        return x >= 0 && y >= 0 && z >= 0 && x < xCount && y < yCount && z < zCount;
    }
}
