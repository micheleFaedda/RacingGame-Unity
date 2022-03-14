using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gestionePunteggio : MonoBehaviour
{
    private int result;
    private int result2;
    public Text coins;
    public string menu;
    // Start is called before the first frame update
    void Start()
    {
        coins.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoins()
    {

     
        int.TryParse(coins.text,out result);
        coins.text = (result + 1).ToString();
    }

    public void SaveAndMenu()
    {
        
        int.TryParse(coins.text, out result);
        PlayerPrefs.SetInt("Coins",  (result + PlayerPrefs.GetInt("Coins")));
        SceneManager.LoadScene(menu);
    }
}
