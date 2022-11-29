using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject characterMenu;
    public GameObject difficultyMenu;
    private GameObject curMenu;
    private Light directionalLight;
    void Start()
    {
        curMenu = mainMenu;
        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Debug.Log("haha");
            curMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
    public void IntoDifficultyMenu()
    {
        mainMenu.SetActive(false);
        difficultyMenu.SetActive(true);
        curMenu = difficultyMenu;
    }
    public void IntoSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        curMenu = settingsMenu;
    }
    public void IntoCharacterMenu()
    {
        mainMenu.SetActive(false);
        characterMenu.SetActive(true);
        curMenu = characterMenu;
    }
    public void ChangeBrightness(float value)
    {
        directionalLight.intensity = 2 * value;
    }
    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }
}
