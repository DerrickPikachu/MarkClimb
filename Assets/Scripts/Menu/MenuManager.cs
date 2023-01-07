using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;

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
    public GameObject[] records = new GameObject[3];

    [System.Serializable]
    class SaveData
    {
        public float bestScore;
        public string timeOfRecord;
    }
    void Start()
    {
        mainMenu.SetActive(true);
        curMenu = mainMenu;
        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        maps.SetActive(false);
        ShowRecords();
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
        MainManager.Instance.brightness=value;
        // Debug.Log("ChangeBrightness:" + ri.color);
    }
    public void StartNew(int sceneIdx)
    {
        Debug.Log("StartNew:" + sceneIdx);
        SceneManager.LoadScene(sceneIdx);
    }
    public void ShowRecords(){
        SaveData[] datas = new SaveData[3];
        for (int i = 1; i <= 3 ; i++){
            datas[i-1]=ReadBest("bestHeight_scene"+i.ToString()+".json");
            if (datas[i-1].bestScore>0){
                records[i-1].GetComponent<TMP_Text>().text = datas[i-1].bestScore.ToString()+"\t"+datas[i-1].timeOfRecord;
            }
            else {
                records[i-1].GetComponent<TMP_Text>().text = "No records found";
            }
        }
    }
    SaveData ReadBest(String fileName){
        String absPath = Application.persistentDataPath +"/"+ fileName;
        SaveData readData = new SaveData();
        if (File.Exists(absPath))
        {
            string jsonContent = File.ReadAllText(absPath);
            readData = JsonUtility.FromJson<SaveData>(jsonContent);
        }
        return readData;
    }
    public void ChangeVolume(float value){
        MainManager.Instance.volume=value;
    }
}
