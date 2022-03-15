using System;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public GameObject testoCoins;
    public GameObject[] cars;
    public GameObject raceMode;
    public GameObject timeMode;
    public GameObject multiMode;
    private GameObject car;
    public static int firstCoins = 500;
    public static  int secondCoins = 350;
    public static int thirdCoins = 250;
    public static int fourthCoins = 200;

    public void Start()
    {
        Classifica.Reset();
        GameManager.flag_started_coundown = false;
        GameManager.start = false;
        foreach (GameObject c in cars)
        {
            c.GetComponent<Rigidbody>().useGravity = false;
            c.gameObject.transform.localScale = new Vector3(20, 20, 20);
            c.gameObject.transform.rotation =
                new Quaternion(-0.0252383146f, 0.983145773F, -0.0892141908F, -0.157569751F);
            c.gameObject.transform.position = new Vector3(-1291.09998f, -164.300003f, -512.099976f);
        }

        car = Instantiate(cars[PlayerPrefs.GetInt("macchina_giocatore")]);
        car.transform.SetParent(GameObject.FindWithTag("CanvasMods").transform, false);
        

        testoCoins.GetComponent<Text>().text = "Actual coins: " + PlayerPrefs.GetInt("coins");
       
    }

    void Update()
    {
        car.transform.Rotate(0, 50 * Time.deltaTime, 0);

    }

    public void startRacing()
    {
        String laps = GameObject.FindWithTag("ChoseLaps").GetComponent<Text>().text;
        PlayerPrefs.SetInt("num_giri_race", Int32.Parse(laps));
        PlayerPrefs.SetString("modalita", "racing");
        SceneManager.LoadScene("Game");
    }

    public void startTime()
    {
        PlayerPrefs.SetString("modalita", "time");
        SceneManager.LoadScene("Game");
    }

    /*Questa e la precedente sono solo di debug, alla fine diventano un unica funzione*/
    public void startMulti()
    {
        PlayerPrefs.SetString("modalita", "multiplayer");
        PlayerPrefs.SetInt("num_giri_multi", 1);
        SceneManager.LoadScene("Loading");

    }


    public void goMulti()
    {
        GameObject.FindWithTag("CanvasMods").GetComponent<Canvas>().enabled = false;
        GameObject.FindWithTag("CanvasRules").GetComponent<Canvas>().enabled = true;
        timeMode.SetActive(false);
        raceMode.SetActive(false);
    }

    public void goRacing()
    {
        GameObject.FindWithTag("CanvasMods").GetComponent<Canvas>().enabled = false;
        GameObject.FindWithTag("CanvasRules").GetComponent<Canvas>().enabled = true;
        timeMode.SetActive(false);
        multiMode.SetActive(false);

    }

    public void goTimeAttack()
    {

        GameObject.FindWithTag("CanvasMods").GetComponent<Canvas>().enabled = false;
        GameObject.FindWithTag("CanvasRules").GetComponent<Canvas>().enabled = true;
        raceMode.SetActive(false);
        multiMode.SetActive(false);


    }

    public void goBack()
    {
        GameObject.FindWithTag("CanvasMods").GetComponent<Canvas>().enabled = true;
        GameObject.FindWithTag("CanvasRules").GetComponent<Canvas>().enabled = false;
        raceMode.SetActive(true);
        multiMode.SetActive(true);
        timeMode.SetActive(true);
    }

    public void AddLaps()
    {
        string laps = GameObject.FindWithTag("ChoseLaps").GetComponent<Text>().text;
        int max = 3;
        int min = 1;
        int x = Int32.Parse(laps);
        x++;
        x = Math.Clamp(x, min, max);
       
        GameObject.FindWithTag("ChoseLaps").GetComponent<Text>().text = Math.Clamp(x, min, max) + "";
        SetCoinsMenu(x);
    }

    public void MinusLaps()
    {
        string laps = GameObject.FindWithTag("ChoseLaps").GetComponent<Text>().text;
        int max = 3;
        int min = 1;
        int x = Int32.Parse(laps);
        
        x--;
        x = Math.Clamp(x, min, max);
       
        GameObject.FindWithTag("ChoseLaps").GetComponent<Text>().text = x  + "";
        SetCoinsMenu(x);

    }

    public void goShop()
    {
        SceneManager.LoadScene("ChooseCar");
    }

    private void SetCoinsMenu(int laps)
    {
        firstCoins = 500;
        secondCoins = 350;
        thirdCoins = 250;
        fourthCoins = 200;

        int variablesCoins = 40 * (laps-1);

        firstCoins += variablesCoins;
        secondCoins += variablesCoins;
        thirdCoins += variablesCoins;
        fourthCoins += variablesCoins;

        GameObject.FindWithTag("FirstCoins").GetComponent<Text>().text = firstCoins + " coins";
        GameObject.FindWithTag("SecondCoins").GetComponent<Text>().text = secondCoins + " coins";
        GameObject.FindWithTag("ThirdCoins").GetComponent<Text>().text = thirdCoins + " coins";
        GameObject.FindWithTag("FourthCoins").GetComponent<Text>().text = fourthCoins + " coins";
    }

}
