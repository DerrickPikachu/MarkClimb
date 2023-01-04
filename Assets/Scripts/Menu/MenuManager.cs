using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject characterMenu;
    public GameObject difficultyMenu;
    private GameObject curMenu;
    private Light directionalLight;
    public GameObject lightBlock;

    public GameObject maps;
    void Start()
    {
        mainMenu.SetActive(true);
        curMenu = mainMenu;
        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        maps.SetActive(false);
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
    public void IntoMaps()
    {
        mainMenu.SetActive(false);
        maps.SetActive(true);
        curMenu = maps;
    }
    public void ChangeBrightness(float value)
    {
        // directionalLight.intensity = 2 * value;
        RawImage ri =  lightBlock.GetComponent<RawImage>();
        ri.color = new Color(0,0,0,1-value);
        Debug.Log("ChangeBrightness:" + ri.color);
    }
    public void StartNew(int sceneIdx)
    {
        Debug.Log("StartNew:" + sceneIdx);
        SceneManager.LoadScene(sceneIdx);
    }
}
