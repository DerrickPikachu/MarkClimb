using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static MainManager Instance;
    public int charIndex;
    public float brightness;
    public float volume;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        charIndex = 1;
        brightness =1;
        volume =1;
        DontDestroyOnLoad(gameObject);
    }
}
