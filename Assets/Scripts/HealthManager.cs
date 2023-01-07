using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    // Start is called before the first frame update
    float initHealth = 10;
    public float health;
    public GameObject myUI;
    public GameObject healthBar;
    GameObject green;
    RectTransform rtGreen;
    void Start()
    {
        health = initHealth;
        myUI.SetActive(false);
        green = healthBar.transform.Find("Green").gameObject;
        rtGreen = green.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( health <= 0){
            myUI.SetActive(true);
            gameObject.SetActive(false);
        }
        DisplayBar();
    }

    private void DisplayBar(){
        float portion = health / initHealth;
        float width =Mathf.Round(100*portion);

        float xPos= -50+width/2;
        // Debug.Log("xpos "+ xPos);
        rtGreen.localPosition = new Vector3(xPos,0,0);
        rtGreen.sizeDelta = new Vector2(width,10);
    }
    public void HurtByBlock(){
        health -= 5;
    }
    public void Drowning(){
        health -=1;
    }
    public void HurtByMonster(float damage) {
        health -= damage;
    }
    public void BackToMenu(){
        SceneManager.LoadScene(0);
    }
}
