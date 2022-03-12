using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
 
    public void Update()
    {
        Debug.Log(PlayerPrefs.GetString("cacca"));
    }

    public void startRacing()
    {
        //Questi due parametri da settare quando viene selezionata la macchina (da qui devono essere tolti)
        PlayerPrefs.SetInt("macchina_giocatore", 0);
        PlayerPrefs.SetInt("forza", 70);
        PlayerPrefs.SetInt("num_giri_race", 1);
        
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
    
    /*Questa e la successiva sono solo di debug, alla fine diventano un unica funzione*/
    public void startMulti80()
    {
        //Questi due parametri da settare quando viene selezionata la macchina (da qui devono essere tolti)
        PlayerPrefs.SetInt("macchina_giocatore", 2);
        PlayerPrefs.SetInt("forza", 80);
        PlayerPrefs.SetInt("num_giri_multi", 1);
        
        PlayerPrefs.SetString("modalita", "multiplayer");
        
        //Questo da settare in partenza, nella scena iniziale (o se non settato deve essere player di default), da qui deve essere tolto
        PlayerPrefs.SetString("player_name", "Vicenzo");
        
        SceneManager.LoadScene("Loading");

    }
    
    /*Questa e la precedente sono solo di debug, alla fine diventano un unica funzione*/
    public void startMulti200()
    {
        //Questi due parametri da settare quando viene selezionata la macchina (da qui devono essere tolti)
        PlayerPrefs.SetInt("macchina_giocatore", 2);
        PlayerPrefs.SetInt("forza", 200);
        PlayerPrefs.SetInt("num_giri_multi", 1);

        PlayerPrefs.SetString("modalita", "multiplayer");
        
        //Questo da settare in partenza, nella scena iniziale (o se non settato deve essere player di default), da qui deve essere tolto
        PlayerPrefs.SetString("player_name", "Vicenzo");
        
        SceneManager.LoadScene("Loading");
        
        
    }
}
