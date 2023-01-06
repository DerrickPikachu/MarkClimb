using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsReceiver : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject lightBlock;
    private int curIndex;
    void Start()
    {
        if(MainManager.Instance!=null){
            SoundManager.instance.ChangeEffectVolume(MainManager.Instance.volume);
            lightBlock.GetComponent<RawImage>().color=new Color(0,0,0,1-MainManager.Instance.brightness);
            Debug.Log("Received "+MainManager.Instance.brightness);
            curIndex = 1;
            player.transform.GetChild(curIndex).gameObject.SetActive(false);
            curIndex = MainManager.Instance.charIndex;
            player.transform.GetChild(curIndex).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
