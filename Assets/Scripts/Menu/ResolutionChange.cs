using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class ResolutionChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeResolution(int mode){
        if(mode == 0)
        {
            Screen.SetResolution(1920,1080,false);
            Debug.Log(mode);
        }
        if(mode == 1)
        {
            Screen.SetResolution(2560,1440,false);
            Debug.Log(mode);
        }
        if(mode == 2)
        {
            Screen.SetResolution(3840,2160,false);
            Debug.Log(mode);
        }

    }
}
