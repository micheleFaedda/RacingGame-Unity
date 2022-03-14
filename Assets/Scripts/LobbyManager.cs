
using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
          
    }

    private void Update()
    {
       // SetButtons();
    }

    public void BackButton()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        SceneManager.LoadScene("SceltaModalita");
    }
    
    /*Metodo che si occupa di settare il bottone corretto nel caso in cui ci sia o meno la stanza attivata
    void SetButtons()
    {
        if (PhotonNetwork.CountOfPlayersInRooms > 0) //se vi sono player allora la stanza è aperta altrimenti no
        {
            buttonCreate.SetActive(false);
            buttonJoin.SetActive(true);
            buttonJoin.GetComponent<RectTransform>().localPosition = new Vector3(-631f, 43f, 0f);
        }
        else
        {
            buttonJoin.SetActive(false);
            buttonCreate.SetActive(true);
            buttonCreate.GetComponent<RectTransform>().localPosition = new Vector3(-631f, 43f, 0f);

        }
    }*/
}
