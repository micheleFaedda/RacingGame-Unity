using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public void BackButton()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        SceneManager.LoadScene("SceltaModalita");
    }
    
    public void CreateRoom(){
        PhotonNetwork.CreateRoom(createInput.text, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public void JoinRoom(){
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom(){
        PhotonNetwork.LoadLevel("Game");
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
