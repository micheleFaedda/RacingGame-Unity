using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    //public InputField createInput;
    //public InputField joinInput;

    public void CreateRoom(){
        //PhotonNetwork.CreateRoom(createInput.text);
        PhotonNetwork.CreateRoom("bb");
    }

    public void JoinRoom(){
        //PhotonNetwork.JoinRoom(joinInput.text);
        PhotonNetwork.JoinRoom("bb");
    }

    public override void OnJoinedRoom(){
        PhotonNetwork.LoadLevel("Game");
    }
}
