
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour
{

    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    private int currentCar;
    
    public Button goToChooseMode;
    public Button unlockCar;
    public Text coins;
    private int numMacchine = 3;

    private void Start()
    {
        currentCar = PlayerPrefs.GetInt("macchina_giocatore");
        SelectCar(currentCar);
        coins.text = PlayerPrefs.GetInt("coins").ToString();
        
        PlayerPrefs.SetInt("0", 1);
        unlockCar.interactable = false;
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
        if (currentCar > transform.childCount - 1)
            currentCar = 0;
        else if (currentCar < 0)
            currentCar = transform.childCount - 1;

        PlayerPrefs.SetInt("macchina_giocatore", currentCar);
        
        
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
