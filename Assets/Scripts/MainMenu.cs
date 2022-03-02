using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel;
    public GameObject resOption;
    public GameObject option;
    public GameObject audioOption;
   // public GameObject audioOption;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevel);
    }

    public void ResolutionOption()
    {
        resOption.SetActive(true);
        option.SetActive(false);
    }

    public void AudioOption()
    {
        audioOption.SetActive(true);
        option.SetActive(false);
    }
    public void Options()
    {
        option.SetActive(true);
    }


    public void CloseOptions()
    {
        resOption.SetActive(false);
        option.SetActive(false);
        audioOption.SetActive(false);
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}

