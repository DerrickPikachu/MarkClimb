using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    private int curIndex = 1;
    private int rotateSpeed = 100;
    void Start()
    {
        ChangeCharacter(curIndex);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput= Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up,horizontalInput*rotateSpeed*Time.deltaTime);
    }
    void ChangeCharacter(int index){
        if(index>=12){
            index = 1;
        }
        if ( index <=0){
            index = 12;
        }
        GameObject curChild = gameObject.transform.GetChild(curIndex).gameObject;
        curChild.SetActive(false);
        GameObject newChild = gameObject.transform.GetChild(index).gameObject;
        newChild.SetActive(true);
        curIndex = index;
    }
    public void NextCharacter(){
        ChangeCharacter(curIndex+1);
    }
    public void PreviousCharacter(){
        ChangeCharacter(curIndex-1);
    }
    public void ResetCharacter(){
        ChangeCharacter(1);
    }
}
