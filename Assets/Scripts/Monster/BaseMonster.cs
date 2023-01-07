using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonster : MonoBehaviour
{
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.IndexOf("ProtectRingObj") != -1) { Destroy(gameObject); }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.IndexOf("ProtectRingObj") != -1) { Destroy(gameObject); }
    }
}
