using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vanish : MonoBehaviour
{
    // public GameObject player;
    public float fadeSpeed = 1.0f;
    public float appearTime = 1.0f;
    public float vanishTime = 2.0f;
    // public float moveSpeed = 5.0f;

    private bool fading = true;
    private float countingTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FadeUpdate();
        FlagUpdate();
    }

    void FixedUpdate()
    {
        // Vector3 playerZPos = new Vector3(0, 0, player.transform.position.z);
        // Vector3 objectZPos = new Vector3(0, 0, transform.position.z);
        // Vector3 direction = playerZPos - objectZPos;
        // GetComponent<Rigidbody>().velocity = direction.normalized * moveSpeed;
    }

    void FadeUpdate()
    {
        Color objectColor = GetComponent<Renderer>().material.color;
        float new_color_a = objectColor.a;
        new_color_a += (fading) ? -fadeSpeed * Time.deltaTime : fadeSpeed * Time.deltaTime;
        new_color_a = Mathf.Min(new_color_a, 1.0f);
        new_color_a = Mathf.Max(new_color_a, 0.0f);
        GetComponent<Renderer>().material.color = new Color(objectColor.r, objectColor.g, objectColor.b, new_color_a);
    }

    void FlagUpdate()
    {
        countingTime += Time.deltaTime;
        if (fading && countingTime >= vanishTime) {
            fading = false;
            countingTime = 0;
        } else if (!fading && countingTime >= appearTime) {
            fading = true;
            countingTime = 0;
        }
    }
}
