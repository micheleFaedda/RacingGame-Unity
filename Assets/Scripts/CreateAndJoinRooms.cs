using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    //public InputField createInput;
    //public InputField joinInput;

    public void CreateRoom(){
        //PhotonNetwork.CreateRoom(createInput.text);
        PhotonNetwork.CreateRoom("bb", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public void JoinRoom(){
        //PhotonNetwork.JoinRoom(joinInput.text);
        PhotonNetwork.JoinRoom("bb");
    }

    public override void OnJoinedRoom(){
        PhotonNetwork.LoadLevel("Game");
    }
}
