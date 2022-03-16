using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class PauseMenu : MonoBehaviour
{

    public static bool pause = false;
    public GameObject ui;
    public string menu;
   
    void Update()
    {
        //chiamo le funzioni, e quindi entro ed esco la menu di pausa in base al tasto esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    //faccio ripartire il gioco dalla pausa e quindi disattivo l'interfaccia, e faccio tornare timescale a 1 (cioè il tempo tornerà a scorrere normalmente)
    public void Resume()
    {
        ui.SetActive(false);
        Time.timeScale = 1f;
        pause = false;
    }

    //metto in pausa il gioco, attivo quindi l'interfaccia del menu di pausa, e metto timescale a 0 (cioè il tempo si fermerà)
    public void Pause()
    {
        ui.SetActive(true);
        Time.timeScale = 0f;
        pause = true;
    }

    //Torno al menù principale cambiando scena
    public void LoadMenu()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("SceltaModalita");
    }

    //esco dal gioco
    public void QuitGame()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        Debug.Log("Quit");
        Application.Quit();
    }
}