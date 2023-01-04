using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pasueCanvas;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public bool isPause;
    public int curMenu;
    public GameObject lightBlock;
    void Start()
    {
        isPause = false;
        curMenu =0;
        pasueCanvas.SetActive(true);
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPause){
                pasueCanvas.SetActive(true);
                pauseMenu.SetActive(true);
                settingsMenu.SetActive(false);
                isPause = true;
                Time.timeScale = 0;
                curMenu = 1;
            }
            else if (curMenu==1){
                ResumeGame();
            }
            else if (curMenu==2) {
                pauseMenu.SetActive(true);
                settingsMenu.SetActive(false);
                curMenu = 1;
            }
        }

    }
    public void ResumeGame()
    {
        if (isPause)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
            isPause = false;
            curMenu = 0;
        }

    }
    public void EnterSettingsFromPause(){
        curMenu = 2;
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ExitFromGame(){
        // SceneManager.LoadScene(0);
        Debug.Log("exit from game");
    }
    public void ChangeBrightness(float value){
        // directionalLight.intensity = 2 * value;
        RawImage ri =  lightBlock.GetComponent<RawImage>();
        ri.color = new Color(0,0,0,1-value);
        Debug.Log("ChangeBrightness:" + ri.color);
    }
}
