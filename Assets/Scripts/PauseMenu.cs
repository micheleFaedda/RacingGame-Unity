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

    public void Resume()
    {
        ui.SetActive(false);
        Time.timeScale = 1f;
        pause = false;
    }
    public void Pause()
    {
        ui.SetActive(true);
        Time.timeScale = 0f;
        pause = true;
    }

    public void LoadMenu()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("SceltaModalita");
    }

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