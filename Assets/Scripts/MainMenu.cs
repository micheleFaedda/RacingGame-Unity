using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public string chooseCar;
    public GameObject resOption;
    public GameObject option;
    public GameObject audioOption;
    public GameObject nome;
    public GameObject benvenuto;
    public TMPro.TextMeshProUGUI testoBenvenuto;
    public TMPro.TextMeshProUGUI testoNome;
    
    // Start is called before the first frame update
    void Start()
    {

       
        
        if (PlayerPrefs.HasKey("User") || !(PlayerPrefs.GetString("User").Equals("")) ) {
            nome.SetActive(false);
            benvenuto.SetActive(true);
            testoBenvenuto.text = "benvenuto " + PlayerPrefs.GetString("User").ToString();
       }
        else { 

        nome.SetActive(true);
        benvenuto.SetActive(false);
        }
     
    }

    //Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseCar()
    {
        if (!PlayerPrefs.HasKey("User"))
        {
            PlayerPrefs.SetString(("User"), testoNome.text);
        }
            SceneManager.LoadScene(chooseCar);
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

