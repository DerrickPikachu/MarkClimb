using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    // Start is called before the first frame update
    public float raiseSpeed ;
    public GameObject surface;
    public GameObject body;
    public GameObject player;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        surface.transform.Translate(Vector3.up*raiseSpeed*Time.deltaTime);
        body.transform.localScale=new Vector3(100,surface.transform.position.y,100);
        body.transform.Translate(Vector3.up*raiseSpeed*Time.deltaTime/2);
        if ( player.transform.position.y+2<surface.transform.position.y){
            player.GetComponent<HealthManager>().health -= 0.1f;
        }
    }
}
