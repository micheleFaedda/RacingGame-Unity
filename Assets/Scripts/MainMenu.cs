using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public GameObject resOption;
    public GameObject option;
    public GameObject audioOption;
    public GameObject commands;
    
    //Il nome del player ("YourName" di default)
    public TMP_InputField nome;

    public GameObject feedback;
    
    void Start()
    {
        nome.characterLimit = 16;
        
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
        Debug.Log(name.Length);
        if(nome.text.Length >= 4){
            PlayerPrefs.SetString(("player_name"), nome.text);
            SceneManager.LoadScene("ChooseCar");
        }
        else
        {
            StartCoroutine(FeedBack());
        }
        
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
    {   nome.gameObject.SetActive(false);
        option.SetActive(true);
    }
    
    public void Commands()
    {   nome.gameObject.SetActive(false);
        option.SetActive(false);
        commands.SetActive(true);
    }
    
    public void CloseOptions()
    {   nome.gameObject.SetActive(true);
        resOption.SetActive(false);
        option.SetActive(false);
        audioOption.SetActive(false);
        commands.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    
    IEnumerator FeedBack()
    {
        feedback.GetComponent<TMP_Text>().text = "Your name name must have 4 characters minimum.";
        yield return new WaitForSeconds(2);
        feedback.GetComponent<TMP_Text>().text = "";
    }
    
}

