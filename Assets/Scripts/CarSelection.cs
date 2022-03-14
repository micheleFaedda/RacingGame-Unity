
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        InitializeCars();
        currentCar = PlayerPrefs.GetInt("macchina_giocatore");
        SelectCar(currentCar);
        coins.text = PlayerPrefs.GetInt("coins").ToString();
        
        PlayerPrefs.SetInt("0", 1);
        SetInterface(true);
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
    }

    public void ChangeCar(int _change)
    {
       
        currentCar += _change;
        currentCar = Math.Clamp(currentCar, 0, numMacchine);
        PlayerPrefs.SetInt("macchina_giocatore", currentCar);
        
        //attivo le scritte per la forza
        intestTorque.SetActive(true);
        torque.SetActive(true);
        
        switch (currentCar)
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
        
        
        
        if(PlayerPrefs.GetString(currentCar.ToString()).Equals("true"))
        {
           SetInterface(true);
        }
        else
        {
            SetInterface(false);
            
        }
        SelectCar(currentCar);
    }
    
    public void UnlockCar()
    {
        int coinsPlayer = PlayerPrefs.GetInt("coins");
        int cost = int.Parse(this.cost.GetComponent<Text>().text);
        
        if (coinsPlayer >= cost)
        {
            PlayerPrefs.SetString(currentCar.ToString(), "true");
            Debug.Log(coinsPlayer - cost);
            PlayerPrefs.SetInt("coins",coinsPlayer-cost);
            
            SetInterface(true);
            
            coins.text = PlayerPrefs.GetInt("coins") + "";
           // transform.GetChild(currentCar + numMacchine).gameObject.SetActive(false);
            SelectCar(currentCar);
        }
        else
        {
            Debug.Log("Non hai i soldi");
        }
        
    }


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
        PlayerPrefs.SetString("0","true");
        PlayerPrefs.SetString("1","false");
        PlayerPrefs.SetString("2","false");
        PlayerPrefs.SetString("3","false");
    }
}
