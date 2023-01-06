using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class RecordSaver : MonoBehaviour
{
    [System.Serializable]
    class SaveData
    {
        public float bestScore;
        public string timeOfRecord;
    }
    // Start is called before the first frame update
    public GameObject player;
    public float bestHeight;
    public String fileName;
    public GameObject recordPlane;

    void Start()
    {
        HandlePlane(ReadBest(fileName));
    }

    // Update is called once per frame
    void Update()
    {

        if (player.active == false)
        {
            WriteBest(fileName);
        }
        else
        {
            UpdateBestHeight();
        }
    }
    void UpdateBestHeight()
    {
        bestHeight = Math.Max(bestHeight, player.transform.position.y);
        // Debug.Log(bestHeight);
    }
    void WriteBest(String fileName)
    {
        String absPath = Application.persistentDataPath +"/"+ fileName;
        SaveData readData = new SaveData();
        if (File.Exists(absPath))
        {
            string jsonContent = File.ReadAllText(absPath);
            readData = JsonUtility.FromJson<SaveData>(jsonContent);
        }
        SaveData data = new SaveData();
        data.bestScore = bestHeight;
        data.timeOfRecord = DateTime.Now.ToString();
        if (data.bestScore > readData.bestScore)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(absPath, json);
        }
        Debug.Log(absPath);
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
    void HandlePlane(SaveData data){
        recordPlane.transform.SetPositionAndRotation(new Vector3(0,data.bestScore,0),Quaternion.identity);
    }
}
