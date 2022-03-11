using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void startRacing()
    {
        PlayerPrefs.SetInt("macchina_giocatore", 0);
        PlayerPrefs.SetInt("forza", 70);
        PlayerPrefs.SetString("modalita", "racing");
        PlayerPrefs.SetString("player_name", "Vicenzo");
        SceneManager.LoadScene("Game");
    }
    
    public void startTime()
    {
        PlayerPrefs.SetInt("macchina_giocatore", 1);
        PlayerPrefs.SetInt("forza", 80);
        PlayerPrefs.SetString("modalita", "time");
        PlayerPrefs.SetString("player_name", "Vicenzo");
        SceneManager.LoadScene("Game");
    }
    
    public void startMulti()
    {
        PlayerPrefs.SetInt("macchina_giocatore", 2);
        PlayerPrefs.SetInt("forza", 110);
        PlayerPrefs.SetString("modalita", "multiplayer");
        PlayerPrefs.SetString("player_name", "Vicenzo");
        SceneManager.LoadScene("Loading");
        
        
    }
}
