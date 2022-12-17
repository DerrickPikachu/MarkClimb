using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShowMessage : MonoBehaviour
{
    // Start is called before the first frame update
    public string welcomeMessage;
    private int curPos;
    private float tTimer;
    TMP_Text tt;
    public float totalTime;
    void Start()
    {
        tt = GetComponent<TMP_Text>();
        // Debug.Log(tt);
        curPos = 0;
    }

    // Update is called once per frame
    void Update()
    {
        tTimer = tTimer + Time.deltaTime;
        float gap = totalTime/welcomeMessage.Length;
        if (tTimer >= gap)
        {
            curPos++;
            tTimer = 0;
        }
        if (curPos <= welcomeMessage.Length)
        {
            tt.text = welcomeMessage.Substring(0, curPos);
        }
    }
}
