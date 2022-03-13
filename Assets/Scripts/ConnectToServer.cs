using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    void Start()
    {   
        //ogni volta che ci si connette al server si resetta la classifica in locale
        Classifica.Reset();
        
        //si resettano le variabili per capire se si è partiti o no
        GameManager.flag_started_coundown = false;
        GameManager.start = false;
        
        //connessione
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        //join alla lobby
        PhotonNetwork.JoinLobby();
    } 

     public override void OnJoinedLobby() {
         //settaggio della scena
         SceneManager.LoadScene("Lobby");
    } 
}
