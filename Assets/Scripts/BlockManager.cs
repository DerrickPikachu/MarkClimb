using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{

    public float spawningHeight;
    public float spawningRangeXMin;
    public float spawningRangeXMax;
    public float spawningRangeZMin;
    public float spawningRangeZMax;
    public float spawningTimeInterval;
    public float spawningSpaceXInterval;
    public float spawningSpaceZInterval;
    public GameObject block;
    private float passingTime = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        passingTime += Time.deltaTime;
        if (passingTime > spawningTimeInterval)
        {
            passingTime = 0;
            SpanwBlock();
        }
    }

    void SpanwBlock()
    {
        float x = (float)Math.Round(UnityEngine.Random.Range(spawningRangeXMin, spawningRangeXMax) / spawningSpaceXInterval) * spawningSpaceXInterval;
        float z = (float)Math.Round(UnityEngine.Random.Range(spawningRangeZMin, spawningRangeZMax) / spawningSpaceZInterval) * spawningSpaceZInterval;
        Instantiate(block, new Vector3(x, spawningHeight, z), Quaternion.identity);
    }

}
