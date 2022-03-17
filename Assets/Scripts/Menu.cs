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
    public GameObject timeResult;
    public GameObject otherResult;
    private GameObject car;
    public static int firstCoins = 500;
    public static  int secondCoins = 350;
    public static int thirdCoins = 250;
    public static int fourthCoins = 200;

    public void Start()
    {   
        //eseguo il reset della classifica
        Classifica.Reset();
        
        //reinizializzo le flag per il game
        GameManager.flag_started_coundown = false;
        GameManager.start = false;
        
        //posiziono le macchine nella camera per mostrarla nella canvas mentre ruota
        foreach (GameObject c in cars)
        {
            c.GetComponent<Rigidbody>().useGravity = false;
            c.gameObject.transform.localScale = new Vector3(20, 20, 20);
            c.gameObject.transform.rotation =
                new Quaternion(-0.0252383146f, 0.983145773F, -0.0892141908F, -0.157569751F);
            c.gameObject.transform.position = new Vector3(-1291.09998f, -164.300003f, -512.099976f);
        }
         
        //istanzio la macchina del giocatore per mostrarla nella canvas
        car = Instantiate(cars[PlayerPrefs.GetInt("macchina_giocatore")]);
        car.transform.SetParent(GameObject.FindWithTag("CanvasMods").transform, false);
        
        //se sto navigando nel menu allora visualizzo la canvas della modalità
        if (PlayerPrefs.GetString("timeResult").Equals("false") && PlayerPrefs.GetString("otherResult").Equals("false"))
        {
            testoCoins.GetComponent<Text>().text = "Actual coins: " + PlayerPrefs.GetInt("coins");
        }
        
        //se si torna da una dalla modalità time attack allora visualizzo il risultato 
        if (PlayerPrefs.GetString("timeResult").Equals("true"))
        {
            GameObject.FindWithTag("CanvasMods").GetComponent<Canvas>().enabled = false;
            GameObject.FindWithTag("CanvasRules").GetComponent<Canvas>().enabled = false;
            GameObject.FindWithTag("CanvasResults").GetComponent<Canvas>().enabled = true;
            timeResult.SetActive(true);
            otherResult.SetActive(false);
            GameObject.FindGameObjectWithTag("CoinsEarn").GetComponent<Text>().text =
                PlayerPrefs.GetInt("CoinsEarn").ToString();
            PlayerPrefs.SetString("timeResult","false");
        }
        
        //se si torna da una gara race o multiplayer allora visualizzo il risultato della gara
        if (PlayerPrefs.GetString("otherResult").Equals("true"))
        {
            GameObject.FindWithTag("CanvasMods").GetComponent<Canvas>().enabled = false;
            GameObject.FindWithTag("CanvasRules").GetComponent<Canvas>().enabled = false;
            GameObject.FindWithTag("CanvasResults").GetComponent<Canvas>().enabled = true;
            timeResult.SetActive(false);
            otherResult.SetActive(true);
            GameObject.FindGameObjectWithTag("Posizione").GetComponent<Text>().text =
                PlayerPrefs.GetString("posizione_gara");
            
            GameObject.FindGameObjectWithTag("CoinsEarn").GetComponent<Text>().text =
                PlayerPrefs.GetInt("CoinsEarn").ToString();
            
            PlayerPrefs.SetString("otherResult","false");
            
        }
        
       
    }
    
    //rotazione della macchina
    void Update()
    {
        car.transform.Rotate(0, 70 * Time.deltaTime, 0);

    }
    
    /*metodo per iniziare la modalità racing*/
    public void startRacing()
    {
        String laps = GameObject.FindWithTag("ChoseLaps").GetComponent<Text>().text;
        PlayerPrefs.SetInt("num_giri_race", Int32.Parse(laps));
        PlayerPrefs.SetString("modalita", "racing");
        SceneManager.LoadScene("Game");
    }
    
    /*metodo per iniziare la modalità time*/
    public void startTime()
    {
        PlayerPrefs.SetString("modalita", "time");
        SceneManager.LoadScene("Game");
    }

    /*Meotodo per iniziare la modalità multiplayer*/
    public void startMulti()
    {
        PlayerPrefs.SetString("modalita", "multiplayer");
        PlayerPrefs.SetInt("num_giri_multi", 3);
        SceneManager.LoadScene("Loading");

    }

    
    /*Metodo per mostrare nella canvas le regole del multiplayer*/
    public void goMulti()
    {
        GameObject.FindWithTag("CanvasMods").GetComponent<Canvas>().enabled = false;
        GameObject.FindWithTag("CanvasRules").GetComponent<Canvas>().enabled = true;
        timeMode.SetActive(false);
        raceMode.SetActive(false);
    }
    
    /*Metodo per mostrare nella canvas le regole del racing*/
    public void goRacing()
    {
        GameObject.FindWithTag("CanvasMods").GetComponent<Canvas>().enabled = false;
        GameObject.FindWithTag("CanvasRules").GetComponent<Canvas>().enabled = true;
        timeMode.SetActive(false);
        multiMode.SetActive(false);

    }
    
    /*Metodo per mostrare nella canvas le regole del timeAttack*/
    public void goTimeAttack()
    {

        GameObject.FindWithTag("CanvasMods").GetComponent<Canvas>().enabled = false;
        GameObject.FindWithTag("CanvasRules").GetComponent<Canvas>().enabled = true;
        raceMode.SetActive(false);
        multiMode.SetActive(false);


    }

    /*Metodo per mostrare nella canvas il menu scelta modalità*/
    public void goBack()
    {
        GameObject.FindWithTag("CanvasMods").GetComponent<Canvas>().enabled = true;
        GameObject.FindWithTag("CanvasRules").GetComponent<Canvas>().enabled = false;
        raceMode.SetActive(true);
        multiMode.SetActive(true);
        timeMode.SetActive(true);
    }
    
    /*Metodo per mostrare nella canvas il menu scelta modalità*/
    public void goBackFromResult()
    {   
        //setto le canvas correttamente
        GameObject.FindWithTag("CanvasMods").GetComponent<Canvas>().enabled = true;
        GameObject.FindWithTag("CanvasRules").GetComponent<Canvas>().enabled = false;
        GameObject.FindWithTag("CanvasResults").GetComponent<Canvas>().enabled = false;
        
        //riattivo le modalità per la selezione futura
        raceMode.SetActive(true);
        multiMode.SetActive(true);
        timeMode.SetActive(true);
        
        //setto le variabili per non visualizzare i risultati
        PlayerPrefs.SetString("otherResult","false");
        PlayerPrefs.SetString("timeResult","false");
        testoCoins.GetComponent<Text>().text = "Actual coins: " + PlayerPrefs.GetInt("coins");
    }
    
    /*Metodo per aggiungere a video i giri*/
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
    
    /*Metodo per diminuire a video i giri*/
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
    /*Metodo per mostrare la scena dello shop*/
    public void goShop()
    {
        SceneManager.LoadScene("ChooseCar");
    }
    
    /*Metodo per mostrare le ricompense in base al numero di giri in racing*/
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
