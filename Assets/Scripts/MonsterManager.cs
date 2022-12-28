using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject batObject; 
    public GameObject player;
    public GameObject gameManager;
    public float generateHeightFromPlayer;
    public float generateInterval;

    private float timeCounter;

    // Start is called before the first frame update
    void Start()
    {
        timeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.N)) {
        //     GenerateMonster();
        // }
        timeCounter += Time.deltaTime;
        if (timeCounter > generateInterval) {
            GenerateMonster();
            timeCounter = 0;
        }
    }

    private void GenerateMonster()
    {
        GameObject monster = Instantiate(batObject);
        monster.transform.position = RandomPosition();
        monster.GetComponent<TrackerAI>().player = player;
    }

    private Vector3 RandomPosition()
    {
        Vector3 randPos = new Vector3(0, 0, 0);
        float zMin = gameManager.GetComponent<GameManager>().zMin;
        float zMax = gameManager.GetComponent<GameManager>().zMax;
        
        randPos.z = Random.Range(zMin, zMax);
        randPos.y = player.transform.position.y + generateHeightFromPlayer;
        return randPos;
    }
}
