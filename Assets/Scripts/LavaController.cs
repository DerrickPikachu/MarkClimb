using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    // Start is called before the first frame update
    public float raiseSpeed ;
    public GameObject surface;
    public GameObject body;
    public GameObject player;
    public GameObject fireParticle;
    public GameObject smokePaticle;
    private GameObject[] smokeArray = new GameObject[10];
    public GameObject sweatParticle;
    public int changeFactor ;
    // public GameObject 
    void Start()
    {
        fireParticle.SetActive(false);
        StartCoroutine(RandomSweatPosition());
        StartCoroutine(MoveSmoke());
        StartCoroutine(ChangeRaiseSpeed());
        smokePaticle.SetActive(false);
        Vector3 firstSmokePos=smokePaticle.transform.position;
        for ( int i = 0; i <10; i++ ){
            Vector3 pos = firstSmokePos+i*new Vector3(0,0,2);
            smokeArray[i] = Instantiate(smokePaticle,pos,Quaternion.identity);
            smokeArray[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        surface.transform.Translate(Vector3.up*raiseSpeed*Time.deltaTime);
        body.transform.localScale=new Vector3(100,surface.transform.position.y,100);
        body.transform.Translate(Vector3.up*raiseSpeed*Time.deltaTime/2);
        if ( player.transform.position.y+1.5<surface.transform.position.y){
            player.GetComponent<HealthManager>().health -= 0.1f;
            Vector3 parPos = player.transform.position;
            parPos.y = surface.transform.position.y;
            fireParticle.transform.SetPositionAndRotation(parPos,Quaternion.identity);
            fireParticle.SetActive(true);
        }
        if(player.active==false){
            fireParticle.SetActive(false);
        }
        
    }
    IEnumerator RandomSweatPosition(){
        while (true){
            sweatParticle.transform.SetPositionAndRotation(player.transform.position, Quaternion.identity);
            sweatParticle.SetActive(true);
            yield return new WaitForSeconds(5);
            sweatParticle.SetActive(false);
        }
    }
    IEnumerator MoveSmoke(){
        while (true){
            yield return new WaitForSeconds(1);
            for (int i = 0; i <10;i++){
                Vector3 pos = smokeArray[i].transform.position;
                pos.y = surface.transform.position.y;
                smokeArray[i].transform.SetPositionAndRotation(pos,Quaternion.identity);
            }
        }
    }

    IEnumerator ChangeRaiseSpeed(){
        for (int i=0; i< 9; i++) {
            yield return new WaitForSeconds(8+changeFactor*i);
            raiseSpeed += 0.1f;
        }
    }

}
