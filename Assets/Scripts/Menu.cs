using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public GameObject testoCoins;
    public GameObject position;
    public GameObject[] cars;
    public GameObject raceMode;
    public GameObject timeMode;
    public GameObject multiMode;
    private GameObject car;
    private int firstCoins = 250;
    private  int secondCoins = 200;
    private  int thirdCoins = 150;
    private  int fourthCoins = 50;

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

        car = Instantiate(cars[2]);
        car.transform.SetParent(GameObject.FindWithTag("CanvasMods").transform, false);




        /*Da togliere*/
        if (!PlayerPrefs.HasKey("player"))
        {
            PlayerPrefs.SetString("player", "Vincenzo");
        }

        testoCoins.GetComponent<Text>().text = "" + PlayerPrefs.GetInt("coins");
        position.GetComponent<Text>().text = PlayerPrefs.GetString("posizione_gara");
    }

    void Update()
    {
        car.transform.Rotate(0, 50 * Time.deltaTime, 0);

    }

    public void startRacing()
    {
        //Questi due parametri da settare quando viene selezionata la macchina (da qui devono essere tolti)
        PlayerPrefs.SetInt("macchina_giocatore", 0);
        PlayerPrefs.SetInt("forza", 200);

        String laps = GameObject.FindWithTag("ChoseLaps").GetComponent<Text>().text;
        PlayerPrefs.SetInt("num_giri_race", Int32.Parse(laps));

        PlayerPrefs.SetString("modalita", "racing");

        //Questo da settare in partenza, nella scena iniziale (o se non settato deve essere player di default), da qui deve essere tolto
        PlayerPrefs.SetString("player_name", "Vicenzo");


        SceneManager.LoadScene("Game");
    }

    public void startTime()
    {
        //Questi due parametri da settare quando viene selezionata la macchina (da qui devono essere tolti)
        PlayerPrefs.SetInt("macchina_giocatore", 1);
        PlayerPrefs.SetInt("forza", 80);

        PlayerPrefs.SetString("modalita", "time");

        //Questo da settare in partenza, nella scena iniziale (o se non settato deve essere player di default), da qui deve essere tolto
        PlayerPrefs.SetString("player_name", "Vicenzo");


        SceneManager.LoadScene("Game");
    }

    /*Questa e la precedente sono solo di debug, alla fine diventano un unica funzione*/
    public void startMulti()
    {
        //Questi due parametri da settare quando viene selezionata la macchina (da qui devono essere tolti)
        PlayerPrefs.SetInt("macchina_giocatore", 1);
        PlayerPrefs.SetInt("forza", 200);
        PlayerPrefs.SetInt("num_giri_multi", 1);

        PlayerPrefs.SetString("modalita", "multiplayer");

        //Questo da settare in partenza, nella scena iniziale (o se non settato deve essere player di default), da qui deve essere tolto
        PlayerPrefs.SetString("player_name", "Vicenzo");

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
        int max = 5;
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
        int max = 5;
        int min = 1;
        int x = Int32.Parse(laps);
        
        x--;
        x = Math.Clamp(x, min, max);
       
        GameObject.FindWithTag("ChoseLaps").GetComponent<Text>().text = x  + "";
        SetCoinsMenu(x);

    }

    private void SetCoinsMenu(int laps)
    {
        firstCoins = 250;
        secondCoins = 200;
        thirdCoins = 150;
        fourthCoins = 50;

        int variablesCoins = 30 * (laps-1);

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
