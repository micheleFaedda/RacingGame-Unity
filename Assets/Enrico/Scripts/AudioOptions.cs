using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{

    public AudioMixer mixer;
    public TMP_Text masterL, musicL, sfxL;
    public Slider masterS, musicS, sfxS;
    // Start is called before the first frame update

    void Start()
    {
        float vol = 0f;
        mixer.GetFloat("Master Vol", out vol);
        masterS.value = vol;

        mixer.GetFloat("Music Vol", out vol);
        musicS.value = vol;

        mixer.GetFloat("SFX Vol", out vol);
        sfxS.value = vol;

        masterL.text = Mathf.RoundToInt(masterS.value + 80).ToString();
        musicL.text = Mathf.RoundToInt(musicS.value + 80).ToString();
        sfxL.text = Mathf.RoundToInt(sfxS.value + 80).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMasterVolume()
    {
        masterL.text = Mathf.RoundToInt(masterS.value+80).ToString();

        mixer.SetFloat("Master Vol", masterS.value);

        PlayerPrefs.SetFloat("Master Vol", masterS.value);
    }

    public void SetMusicVolume()
    {
        musicL.text = Mathf.RoundToInt(musicS.value + 80).ToString();

        mixer.SetFloat("Music Vol", musicS.value);

        PlayerPrefs.SetFloat("Music Vol", musicS.value);
    }

    public void SetSFXVolume()
    {
        sfxL.text = Mathf.RoundToInt(sfxS.value + 80).ToString();

        mixer.SetFloat("SFX Vol", sfxS.value);

        PlayerPrefs.SetFloat("SFX Vol", sfxS.value);
    }


}
