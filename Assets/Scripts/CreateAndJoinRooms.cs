using System;
using System.Collections;
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

    public GameObject feedback;

    public void Start()
    {
        createInput.characterLimit = 16;
        joinInput.characterLimit = 16;
        TextMeshProUGUI placeholder = (TextMeshProUGUI)createInput.placeholder;
        placeholder.text = "Enter New Room Name";
        placeholder = (TextMeshProUGUI)joinInput.placeholder;
        placeholder.text = "Enter Room Name";
    }

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
        if(createInput.text.Length >= 4)
        { 
            int numPlayers = int.Parse(GameObject.FindGameObjectWithTag("chosePlayers").GetComponent<Text>().text);
            PlayerPrefs.SetInt("num_multi_players",numPlayers);
            PhotonNetwork.CreateRoom(createInput.text, new RoomOptions { MaxPlayers = (byte) numPlayers }, TypedLobby.Default);
        }
        else
        {
            StartCoroutine(FeedBack("The room name must have 4 characters minimum."));
        }
    }

    public void JoinRoom(){
        if(joinInput.text.Length >= 4){
        PhotonNetwork.JoinRoom(joinInput.text);
        }
        else
        {
            StartCoroutine(FeedBack("The room name must have 4 characters minimum."));
        }
    }

    public override void OnJoinedRoom(){
        PhotonNetwork.LoadLevel("Game");
    }

   public override void OnJoinRoomFailed(short returnCode, string message)
    {
        StartCoroutine(FeedBack("Failed to join room. It is probably already full."));
    }
    
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        StartCoroutine(FeedBack("Failed to create room. It is probably already created."));
    }

    public void AddMinusPlayers(int players)
    {
        players = int.Parse(GameObject.FindGameObjectWithTag("chosePlayers").GetComponent<Text>().text) + players;
        players = Math.Clamp(players, 2, 3);
        
        GameObject.FindGameObjectWithTag("chosePlayers").GetComponent<Text>().text = players + "";
    }

    IEnumerator FeedBack(String f)
    {
        feedback.GetComponent<TMP_Text>().text = f;
        yield return new WaitForSeconds(2);
        feedback.GetComponent<TMP_Text>().text = "";
    }
}
