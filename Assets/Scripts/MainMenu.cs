using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    
    public GameObject resOption;
    public GameObject option;
    public GameObject audioOption;
    public TMP_InputField nome;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("player_name"))
        {
            PlayerPrefs.SetString("player_name", "YourName");
        }
 
        nome.text = PlayerPrefs.GetString("player_name");
    }

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

