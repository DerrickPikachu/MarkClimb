using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject characterMenu;
    public GameObject difficultyMenu;
    private GameObject curMenu;
    void Start()
    {
        curMenu = mainMenu;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            // Debug.Log("haha");
            curMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
    public void IntoDifficultyMenu(){
        mainMenu.SetActive(false);
        difficultyMenu.SetActive(true);
        curMenu = difficultyMenu;
    }
    public void IntoSettingsMenu(){
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        curMenu = settingsMenu;
    }
    public void IntoCharacterMenu(){
        mainMenu.SetActive(false);
        characterMenu.SetActive(true);
        curMenu = characterMenu;
    }
}
