using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

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
        PhotonNetwork.CreateRoom(createInput.text, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public void JoinRoom(){
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom(){
        PhotonNetwork.LoadLevel("Game");
    }
}
