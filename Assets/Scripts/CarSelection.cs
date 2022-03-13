using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour
{

    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    private int currentCar;
    public string race;
    public string time;
    public GameObject chooseModeScreen;
    public Button goToChooseMode;
    public Button unlockCar;
    public Text coins;
    private int numMacchine = 3;

    private void Start()
    {
        PlayerPrefs.SetInt("0", 1);
        SelectCar(0);
        coins.text = PlayerPrefs.GetInt("Coins").ToString();
        unlockCar.interactable = false;

    }


    public void GoToChooseMode()
    {
        chooseModeScreen.SetActive(true);
    }
    public void StartRaceGame()
    {
        SceneManager.LoadScene(race);
    }

    public void StartTimeGame()
    {
        SceneManager.LoadScene(time);
    }
    private void SelectCar(int _index)
    {
        previousButton.interactable = (_index != 0);
        nextButton.interactable = (_index != (transform.childCount)/2 - 1);
        

        for (int i = 0; i < (transform.childCount)/2; i++)
        {

            
            if(PlayerPrefs.GetInt(i.ToString(), 0) == 0)
            {
                
                transform.GetChild(i+numMacchine).gameObject.SetActive(i == _index);
                
                
            }
            else
            {
                
                transform.GetChild(i).gameObject.SetActive(i == _index);
                
               
            }
            
        }



    }

    public void ChangeCar(int _change)
    {
        currentCar += _change;
        if (currentCar > (transform.childCount)/2 - 1)
            currentCar = 0;
        else if (currentCar < 0)
            currentCar = transform.childCount - 1;
        
        SaveManager.instance.currentCar = currentCar;
        SaveManager.instance.Save();
        if(PlayerPrefs.GetInt(currentCar.ToString(),0) == 0)
        {
            goToChooseMode.interactable = false;
            unlockCar.interactable = true;
        }
        else
        {
            goToChooseMode.interactable = true;
            unlockCar.interactable = false;
        }
        SelectCar(currentCar);

    }

    public void UnlockCar()
    {
        if (int.Parse(coins.text) >= 0)
        {
            PlayerPrefs.SetInt(currentCar.ToString(), 1);
            transform.GetChild(currentCar + numMacchine).gameObject.SetActive(false);
            SelectCar(currentCar);
        }
        else
        {
            Debug.Log("Non hai i soldi");
        }
        
    }
}
