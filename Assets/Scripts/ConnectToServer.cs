using Photon.Pun;
using UnityEngine;
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
        
        //se si è gia connessi ci si riconentte altrimenti nuova connessione
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        
            //connessione
            PhotonNetwork.ConnectUsingSettings();
        
    }

    public override void OnConnectedToMaster() {
        //join alla lobby
        PhotonNetwork.NickName = PlayerPrefs.GetString("player_name");
        PhotonNetwork.JoinLobby();
    } 

     public override void OnJoinedLobby() {
         //settaggio della scena
         SceneManager.LoadScene("Lobby");
    } 
     
     public void BackButton()
     {
         if (PhotonNetwork.IsConnected)
         {
             PhotonNetwork.Disconnect();
         }

         SceneManager.LoadScene("SceltaModalita");
     }
}
