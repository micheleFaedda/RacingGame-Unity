using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class CarSelection : MonoBehaviour
{

    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    private int currentCar;
    
    public GameObject goToChooseMode;
    public GameObject unlockCar;
    public Text coins;
    public GameObject intestCost;
    public GameObject intestTorque;
    public GameObject torque;
    public GameObject cost;
    private int numMacchine = 4;
    
    private void Start()
    {
        if (!PlayerPrefs.HasKey("blue_car"))
        {
            InitializeCars();
        }
        
        currentCar = PlayerPrefs.GetInt("macchina_giocatore");
        
        ChangeCar(currentCar);
        
        coins.text = PlayerPrefs.GetInt("coins").ToString();
        
        
        
    }

    public void BackButton()
    {
        SceneManager.LoadScene("Menu");
    }


    public void GoToChooseMode()
    {
        SceneManager.LoadScene("SceltaModalita");
    }

    private void SelectCar(int _index)
    {
        previousButton.interactable = (_index != 0);
        nextButton.interactable = (_index != transform.childCount - 1);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
        }
        
        switch (_index)
        {
            case 0:
                PlayerPrefs.SetInt("forza", 100);
                break;
            case 1:
                PlayerPrefs.SetInt("forza", 120);
                break;
            case 2:
                PlayerPrefs.SetInt("forza", 180);
                break;
            case 3:
                PlayerPrefs.SetInt("forza", 210);
                break;
        }
    }
    
    /*Metodo per cambiare la macchina nello shop*/
    public void ChangeCar(int _change)
    {
       
        currentCar += _change;
        currentCar = Math.Clamp(currentCar, 0, numMacchine-1); //clamp dell indice passato
        
        PlayerPrefs.SetInt("macchina_giocatore", currentCar);
        
        //selezione a video della macchina
        SelectCar(currentCar);
        
        //attivo le scritte per la forza
        intestTorque.SetActive(true);
        torque.SetActive(true);
        
        //setto la forza a video
        torque.GetComponent<Text>().text = PlayerPrefs.GetInt("forza").ToString();
        
        //setto il costo
        cost.GetComponent<Text>().text = PlayerPrefs.GetInt(currentCar + "_costo").ToString();
        
        if(PlayerPrefs.GetString(currentCar.ToString()).Equals("true")) //se è stata comprata allora setto la sua interfaccia 
        {
           SetInterface(true);
        }
        else
        {
            SetInterface(false);
            
        }
        
    }
    /*Metodo che si occupa dello shop della macchina in base ai coins*/
    public void UnlockCar()
    {
        int coinsPlayer = PlayerPrefs.GetInt("coins");
        int cost = int.Parse(this.cost.GetComponent<Text>().text);
        
        if (coinsPlayer >= cost)
        {
            PlayerPrefs.SetString(currentCar.ToString(), "true");
        
            PlayerPrefs.SetInt("coins",coinsPlayer-cost);
            
            SetInterface(true);
            
            coins.text = PlayerPrefs.GetInt("coins") + "";
;
            SelectCar(currentCar);
        }
        
    }

    /*Metodo che setta l'interfaccia se una macchina è stata comprata o meno*/
    private void SetInterface(bool set)
    {
        if (set)
        {
            
            unlockCar.SetActive(false); 
            goToChooseMode.GetComponent<Button>().interactable = true;
            cost.SetActive(false);
            intestCost.SetActive(false);
        }
        else
        {
            unlockCar.SetActive(true);
            goToChooseMode.GetComponent<Button>().interactable = false;
            cost.SetActive(true);
            intestCost.SetActive(true);
        }
    }
    private void InitializeCars()
    {   
        /*Macchine*/
        PlayerPrefs.SetString("0","true");
        PlayerPrefs.SetString("1","false");
        PlayerPrefs.SetString("2","false");
        PlayerPrefs.SetString("3","false");
        
        /*Costo*/
        PlayerPrefs.SetInt("1_costo",250);
        PlayerPrefs.SetInt("2_costo",1000);
        PlayerPrefs.SetInt("3_costo",2500);

    }
}
