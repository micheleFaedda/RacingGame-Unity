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
    public GameObject chooseCarScreen;

    private void Start()
    {
        currentCar = SaveManager.instance.currentCar;
        SelectCar(currentCar);
    }


    public void GoToChooseMode()
    {
        chooseCarScreen.SetActive(true);
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
        nextButton.interactable = (_index != transform.childCount - 1);

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == _index);
        }
    }

    public void ChangeCar(int _change)
    {
        currentCar += _change;
        if (currentCar > transform.childCount - 1)
            currentCar = 0;
        else if (currentCar < 0)
            currentCar = transform.childCount - 1;

        SaveManager.instance.currentCar = currentCar;
        SaveManager.instance.Save();
        SelectCar(currentCar);
    }
}
