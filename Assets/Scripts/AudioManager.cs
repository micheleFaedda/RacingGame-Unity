using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Master Vol"))
        {
            mixer.SetFloat("Master Vol", PlayerPrefs.GetFloat("Master Vol"));
        }

        if (PlayerPrefs.HasKey("Music Vol"))
        {
            mixer.SetFloat("Music Vol", PlayerPrefs.GetFloat("Music Vol"));
        }

        if (PlayerPrefs.HasKey("SFX Vol"))
        {
            mixer.SetFloat("SFX Vol", PlayerPrefs.GetFloat("SFX Vol"));
        }
    }
}

