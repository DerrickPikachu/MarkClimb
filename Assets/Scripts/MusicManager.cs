using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioClip[] clips = new AudioClip[3];
    public int whichOne;
    private AudioSource musicSource;
    void Start()
    {
        musicSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        musicSource.PlayOneShot(clips[whichOne]);
    }
}
