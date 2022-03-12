using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Classifica.Reset();
        GameManager.flag_started_coundown = false;
        GameManager.start = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    } 

     public override void OnJoinedLobby() {
         SceneManager.LoadScene("Lobby");
    } 
}
