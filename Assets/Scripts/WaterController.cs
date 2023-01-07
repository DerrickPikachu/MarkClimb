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
    public GameObject splashParticle;
    public GameObject bubbleParticle;
    public GameObject rainParticle;
    bool aboveWater;
    public int changeFactor ;
    void Start()
    {
        splashParticle.SetActive(false);
        bubbleParticle.SetActive(false);
        rainParticle.SetActive(true);
        aboveWater =false;
        StartCoroutine(ChangeRaiseSpeed());
    }

    // Update is called once per frame
    void Update()
    {
        rainParticle.transform.position= player.transform.position+new Vector3(10,5,0);
        surface.transform.Translate(Vector3.up*raiseSpeed*Time.deltaTime);
        body.transform.localScale=new Vector3(100,surface.transform.position.y,100);
        body.transform.Translate(Vector3.up*raiseSpeed*Time.deltaTime/2);
        if ( player.transform.position.y+2<surface.transform.position.y){
            player.GetComponent<HealthManager>().health -= 0.1f;
            bubbleParticle.transform.position= player.transform.position;
            bubbleParticle.SetActive(true);
        }
        // if (player.active==false){
        //         bubbleParticle.SetActive(false);
        // }
        if ( player.transform.position.y<surface.transform.position.y){
            if (aboveWater){
                aboveWater = false;
                splashParticle.transform.position=player.transform.position;
                splashParticle.SetActive(true);
            }
        }
        else{
            aboveWater = true;
            splashParticle.SetActive(false);
        }
    }

    IEnumerator ChangeRaiseSpeed(){
        for (int i=0; i< 9; i++) {
            yield return new WaitForSeconds(8+4*i);
            raiseSpeed += 0.1f;
        }
    }

}
