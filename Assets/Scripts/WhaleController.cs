using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject water;
    public float swimSpeed;
    
    public float moveRange;
    private bool isWakeUp;
    private float rightLeft;
    void Start()
    {
        isWakeUp = true;
        rightLeft = -1;
        // StartCoroutine(WhaleSleep());
    }

    // Update is called once per frame
    void Update()
    {
    

        Vector3 pos = transform.position;
        pos.y = water.transform.position.y-0.5f;
        transform.SetPositionAndRotation(pos,transform.rotation);

        transform.Translate(-1*Vector3.forward*swimSpeed*Time.deltaTime);
        if (transform.position.z>moveRange && rightLeft == 1){
            rightLeft = -1;
            transform.Rotate(Vector3.up,180);
        }
        if(transform.position.z<-moveRange && rightLeft == -1){
            rightLeft = 1;
            transform.Rotate(Vector3.up,180);
        }

        if (player.transform.position.y <= water.transform.position.y){
            if(Mathf.Abs(player.transform.position.z-(transform.position.z+3*rightLeft))<1){
                player.GetComponent<HealthManager>().health = 0f;
                transform.Translate(Vector3.up);
                SoundManager.instance.PlaySound(SoundClip.MonsterBite);
            }
        }
    
    }
    IEnumerator WhaleSleep(){
        while(true){
            yield return new WaitForSeconds(5);
            isWakeUp =! isWakeUp;
            Debug.Log ("isWakeUp: " + isWakeUp);
        }
        
    }
}
