using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public GameObject resOption;
    public GameObject option;
    public GameObject audioOption;
    
    //Il nome del player ("YourName" di default)
    public TMP_InputField nome;

    void Start()
    {
        //Se è la prima volta che viene aperto il gioco si setta il player name di dafault
        if (!PlayerPrefs.HasKey("player_name"))
        {
            PlayerPrefs.SetString("player_name", "YourName");
        }
 
        //visualizzo a schermo il player name
        nome.text = PlayerPrefs.GetString("player_name");
    }

    //Per passare alla schermata della selezione delle modalità di gioco
    public void ChooseCar()
    {
        PlayerPrefs.SetString(("player_name"), nome.text);
        SceneManager.LoadScene("ChooseCar");
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
        Debug.Log("Quit");
        Application.Quit();
    }
    
}

