using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float health;
    public GameObject endUI;
    void Start()
    {
        health = 10;
        endUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ( health <= 0){
            endUI.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    public void HurtByBlock(){
        health -= 10;
    }
    public void Drowning(){
        health -=1;
    }
    public void BackToMenu(){
        SceneManager.LoadScene(0);
    }
}
