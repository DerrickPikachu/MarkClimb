using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatManager : MonoBehaviour
{
    public GameObject batObject; 
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) {
            GameObject bat = Instantiate(batObject);
            bat.GetComponent<TrackerAI>().player = player;
        }
    }
}
