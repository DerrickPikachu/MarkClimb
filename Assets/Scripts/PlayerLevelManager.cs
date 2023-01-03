using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLevelManager : MonoBehaviour
{
    public float levelOneCheckPoint = 20;
    public float levelTwoCheckPoint = 40;
    public float levelThreeCheckPoint = 60;
    public float levelFourCheckPoint = 80;
    public float levelFiveCheckPoint = 100;
    public GameObject[] skills;
    public GameObject levelUI;

    private int currentLevel = 0;
    private float bestHeight = 0;

    // Start is called before the first frame update
    void Start()
    {
        WriteLevelText();
    }

    // Update is called once per frame
    void Update()
    {
        bestHeight = Math.Max(bestHeight, transform.position.y);
        UpdateLevel();
        // HandleKey();
    }

    void UpdateLevel()
    {
        if (currentLevel == 0 && bestHeight > levelOneCheckPoint) {
            currentLevel = 1;
            WriteLevelText();
            UpdateSkill();
        } else if (currentLevel == 1 && bestHeight > levelTwoCheckPoint) {
            currentLevel = 2;
            WriteLevelText();
            UpdateSkill();
        } else if (currentLevel == 2 && bestHeight > levelThreeCheckPoint) {
            currentLevel = 3;
            //doubleJumpFlag = true;
            WriteLevelText();
        } else if (currentLevel == 3 && bestHeight > levelFourCheckPoint) {
            currentLevel = 4;
            WriteLevelText();
        } else if (currentLevel == 4 && bestHeight > levelFiveCheckPoint) {
            currentLevel = 5;
            WriteLevelText();
        }
    }

    void WriteLevelText()
    {
        TextMeshProUGUI text = levelUI.GetComponent<TextMeshProUGUI>();
        text.text = "Level " + currentLevel.ToString();
    }

    void UpdateSkill()
    {
        if (currentLevel <= skills.Length) {
            int skillIdx = currentLevel - 1;
            GameObject skillObj = Instantiate(skills[skillIdx]);
            BaseSkill skillComponent = skillObj.GetComponent<BaseSkill>();
            BaseSkill newCom = gameObject.AddComponent(skillComponent.GetType()) as BaseSkill;
            newCom.key = skillComponent.key;
            Destroy(skillObj)
        }
    }
}
