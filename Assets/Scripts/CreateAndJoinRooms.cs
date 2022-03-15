using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    
    //Input per sceare la sstanza
    public TMP_InputField createInput;
    
    //Input per collegarsi alla stanza
    public TMP_InputField joinInput;

    //Funzione per tornare al menu per la selezione delle modalità
    public void BackButton()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }

        SceneManager.LoadScene("SceltaModalita");
    }
    
    public void CreateRoom(){
        
        int numPlayers = int.Parse(GameObject.FindGameObjectWithTag("chosePlayers").GetComponent<Text>().text);
        PlayerPrefs.SetInt("num_multi_players",numPlayers);
        PhotonNetwork.CreateRoom(createInput.text, new RoomOptions { MaxPlayers = (byte) numPlayers }, TypedLobby.Default);
    }

    public void JoinRoom(){
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom(){
        PhotonNetwork.LoadLevel("Game");
    }

    public void AddMinusPlayers(int players)
    {
        players = int.Parse(GameObject.FindGameObjectWithTag("chosePlayers").GetComponent<Text>().text) + players;
        players = Math.Clamp(players, 2, 3);
        
        GameObject.FindGameObjectWithTag("chosePlayers").GetComponent<Text>().text = players + "";
    }
}
