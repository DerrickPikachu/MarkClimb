using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    // Start is called before the first frame update
    private Light directionalLight;
    void Start()
    {
        directionalLight= GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeBrightness(float value){
        directionalLight.intensity = 2*value;
    }
}
